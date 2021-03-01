using CMS.DB;
using CMS.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using log4net;
using System.Configuration;
using Newtonsoft.Json;

public partial class purchase_PurchaseDetail : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(purchase_PurchaseDetail));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSave.Visible = Function.CheckButtonPermission("A020102");

            Common.DropDownBind(ddl_project, (int)CodeListType.ProjectName, "");
            Common.DropDownBind(ddl_newCategory, (int)CodeListType.ProductCategery,false);
            Common.DropDownBind(ddl_newUnit, (int)CodeListType.ProductUnit, false);

            string id = Request.QueryString["id"];
            //参数id和orderId指的都是orderId,因为从JqGrid页面超链接过来的默认id是detailId,参数名字id不能修改，所以重新传递ordeId
            //if (!string.IsNullOrEmpty(Request.QueryString["orderId"]))
            //{
            //    id = Request.QueryString["orderId"];
            //}
            orderId.Value = id;//隐藏控件保存id
            if (!string.IsNullOrEmpty(id))
            {
                Load(Convert.ToInt32(id));
                this.btnImportProduct.Visible = false;
            }
        }
    }
    //private void BindPurchaseDetailGrid(int orderId)
    //{
    //    string sql = @"SELECT B.id,A.supplier_id,A.delivery_date,A.unit_price,A.quantity,A.quantity,A.memo,A.price,
    //                    B.product_num,B.product_name,B.product_size,C.name AS category,D.name AS unit
    //                    FROM tb_purchase_orderdetail A
    //                    LEFT JOIN tb_product B ON (A.product_id = B.id)
    //                    LEFT JOIN tb_code_list C ON(B.product_category_id = c.id)
    //                    LEFT JOIN tb_code_list D ON (B.product_unit_id = D.id)
    //                    WHERE A.order_id = " + orderId;
    //    DataTable tb = DBHelper.GetTableBySql(sql);
    //    grid_productList.DataSource = tb;
    //    grid_productList.DataKeyNames = new string[] { "id" };
    //    grid_productList.DataBind();
    //}
    private void Load(int id)
    {
        string sql = @"select tb_purchase_order.*,c.name AS moneyType,tb_contract.contract_amount from tb_purchase_order
                        LEFT JOIN tb_contract ON (tb_purchase_order.contract_id = tb_contract.contract_num AND tb_contract.contract_num != '')
                        LEFT JOIN tb_code_list c ON (tb_contract.money_type = c.id)
                        where tb_purchase_order.id=" + id;
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            txt_applyDate.Value = Common.ConvertToDate(row["apply_date"]);
            txt_orderNum.Value = row["order_num"].ToString();
            //txt_name.Value = row["order_name"].ToString();
            txt_contractNum.Value = row["contract_id"].ToString();
            txt_leader.Value = row["leader"].ToString();
            txt_leader_old.Value = row["leader"].ToString();
            txt_memo.Value = row["memo"].ToString();
            ddl_project.SelectedValue = row["project_id"].ToString();
            txt_amount.Value = row["amount"].ToString();
            moneyType.Value = row["moneyType"].ToString();
            totalSum.Value =row["contract_amount"].ToString();
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        /*
        List<int> ids = new List<int>();
        foreach (GridViewRow row in grid_product.Rows)
        {
            if (((CheckBox)row.FindControl("checkId")).Checked)
            {
                ids.Add(Convert.ToInt32(grid_product.DataKeys[row.RowIndex].Value.ToString()));
            }
        }
        if (ids.Count <= 0) {
            return;
        }
        string sql = @"select A.id,product_num,product_name,product_size,product_category_id,product_unit_id,B.name as category,C.name as unit,
                        '' as supplier_id, '' as delivery_date,'' as unit_price,'' as quantity,'' as memo,'' as price 
                        from tb_product A  
                        left join tb_code_list B on (A.product_category_id = B.id) 
                        left join tb_code_list C on (A.product_unit_id = C.id) 
                        where A.id in ({0})";
        sql = string.Format(sql, string.Join(",", ids));
        DataTable tb = DBHelper.GetTableBySql(  sql);
        grid_productList.DataSource = tb;
        grid_productList.DataKeyNames = new string[] { "id" };
        grid_productList.DataBind();*/
    }
    [WebMethod]
    public static string Test()
    {
        string s = "<tr><td>3</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        return s;
    }
    [WebMethod]
    public static string GetPurchaseDetail(string orderId)
    {
        string sql = @"SELECT B.id,A.supplier_id,A.delivery_date,A.in_warehouse_date,A.unit_price,A.quantity,A.quantity,A.memo,A.price,
                        B.product_num,B.product_name,B.product_size,C.name AS category,D.name AS unit
                        FROM tb_purchase_orderdetail A
                        LEFT JOIN tb_product B ON (A.product_id = B.id)
                        LEFT JOIN tb_code_list C ON(B.product_category_id = c.id)
                        LEFT JOIN tb_code_list D ON (B.product_unit_id = D.id)
                        WHERE A.order_id = " + orderId;
        DataTable tb = DBHelper.GetTableBySql(sql);
        string result = string.Empty;
        DataTable supplierTb = GetSuppliers();
        for (int index = 0; index < tb.Rows.Count; index++)
        {
            result += GetRowHtml(tb.Rows[index], index + 1, supplierTb);
        }
        return result;
    }
    private static DataTable GetSuppliers()
    {
        string sql = "select id,name from tb_code_list where type=" + (int)CodeListType.SupplierName;
        return DBHelper.GetTableBySql(sql);
    }
    [WebMethod]
    public static string GetCheckedProducts(string selectedProductIds,string checkedIds,string rowIndex)
    {
        string sql = @"select A.id,product_num,product_name,product_size,product_category_id,product_unit_id,B.name as category,C.name as unit,
                        '' as supplier_id, '' as delivery_date,'' as in_warehouse_date, '' as unit_price,'' as quantity,'' as memo,'' as price 
                        from tb_product A  
                        left join tb_code_list B on (A.product_category_id = B.id) 
                        left join tb_code_list C on (A.product_unit_id = C.id) 
                        where A.id in ({0})";
        sql = string.Format(sql, checkedIds);
        DataTable tb = DBHelper.GetTableBySql(sql);
        string result = string.Empty;
        List<string> selectedIds = new List<string>(selectedProductIds.Split(','));
        int index = 0;
        DataTable supplierTb = GetSuppliers();
        foreach (DataRow row in tb.Rows)
        {
            string pid = row["id"].ToString();
            if (!selectedIds.Contains(pid))
            {
                result += GetRowHtml(row, Convert.ToInt32(rowIndex) + index + 1, supplierTb);
                index++;
            }
            
        }
        return result;
    }

    private static string GetRowHtml(DataRow row, int index,DataTable supplierTb)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<tr>");
        sb.Append("<td>" + index +"<input name='pid{0}' runat='server' type='hidden' value='" + row["id"].ToString() + "'/></td>");
        sb.Append("<td style='display: none'>" + row["product_num"] + "<input name = 'maxIndex' type = 'hidden' runat = 'server' value = '{0}' /></td>");
        sb.Append("<td>" + row["product_name"] + "</td>");
        sb.Append("<td>" + row["category"] + "</td>");
        sb.Append("<td>" + row["product_size"] + "</td>");
        sb.Append("<td>" + row["unit"] + "</td>");
        sb.Append("<td><input name='txt_quantity{0}' runat='server' type='text' style='width:70px' value='{1}' /></td>");
        sb.Append("<td><input name='txt_supplier{0}' runat='server' type='text' style='width:70px;display:none'/>");
        sb.Append("<select name='ddl_supplier{0}' onchange='SupplierChange({0})'>");
        sb.Append("<option value='0'></option>");
        sb.Append("<option value='00'>+添加</option>");
        foreach (DataRow r in supplierTb.Rows)
        {
            if (row["supplier_id"] == null || row["supplier_id"] == DBNull.Value || row["supplier_id"].ToString() != r["id"].ToString())
            {
                string option = "<option value='{0}'>{1}</option>";
                sb.Append(string.Format(option, r["id"], r["name"]));
            }
            else
            {
                string option = "<option value='{0}'  selected='selected'>{1}</option>";
                sb.Append(string.Format(option, r["id"], r["name"]));
            }

        }
        sb.Append("</select></td>");
        sb.Append("<td><input name='txt_deliveryDate{0}' runat='server' type='date' value='{3}'/></td>");
        sb.Append("<td><input name='txt_unitPrice{0}' runat='server' type='text' style='width:100px' value='{2}'/></td>");
        //sb.Append("<td></td>");
        sb.Append("<td><input name='txt_warehouseDate{0}' runat='server' type='date' value='{5}'/></td>");
        sb.Append("<td><input name='txt_memo{0}' runat='server' type='text' style='width:100px' value='{4}'/></td>");
        sb.Append("<td><a href='javascript: '><span onclick='delProduct(this)'>删除</span></a></td>");
        sb.Append("</tr>");

        string rowHtml = string.Format(sb.ToString(), new object[] { index, row["quantity"].ToString(), row["unit_price"].ToString(), Common.ConvertToDate(row["delivery_date"]), row["memo"].ToString(), Common.ConvertToDate(row["in_warehouse_date"]) }); ;
        return rowHtml;
    }
    private void BindProductGrid()
    {
        string searchKey = txt_searchKey.Value.Trim().Replace("'","");
        string sql = @"select A.id,product_num,product_material,product_name,product_size,product_category_id,product_unit_id,B.name as category,C.name as unit 
                        from tb_product A  
                        left join tb_code_list B on (A.product_category_id = B.id) 
                        left join tb_code_list C on (A.product_unit_id = C.id) where A.del_flag=0 ";
        if (!string.IsNullOrEmpty(searchKey))
        {
            sql = sql + " and (product_num like '%{0}%' or product_name like '%{0}%' or product_size like '%{0}%' or product_material like '%{0}%')";
            sql = string.Format(sql, searchKey);
        }
        if (grid_product.Attributes["SortExpression"] != null)
        {
            sql += " order by " + grid_product.Attributes["SortExpression"] + " " + grid_product.Attributes["SortDirection"];
        }
        DataTable tb = DBHelper.GetTableBySql(sql);
        grid_product.DataSource = tb;
        grid_product.DataKeyNames = new string[] { "id" };
        grid_product.DataBind();
    }

    protected void grid_productList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void grid_productList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList drSupplier = (DropDownList)e.Row.FindControl("ddl_supplier");
            string supplierId = ((HiddenField)e.Row.FindControl("hidden_supplierId")).Value;
            if (drSupplier != null)
            {
                Common.DropDownBind(drSupplier, (int)CodeListType.SupplierName,"");
                drSupplier.SelectedValue = supplierId;
            }
        }
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        // update
        if (!string.IsNullOrEmpty(id))
        {
            UpdateOrder(id);
        }
        else
        {
            InsertOrder();
        }
        //Response.Write("<script defer='defer'>alert('保存成功。');window.location.href='PurchaseList.aspx';</script>");
        //Response.End();
        BackTo();
    }

    private void BackTo()
    {
        string backType = Request.QueryString["backType"];
        if (!string.IsNullOrEmpty(backType) && backType == "detailList")
        {
            Response.Redirect("../purchase/PurchaseDetailList.aspx?searchKey=" + Request.QueryString["searchKey"]);
        }
        else if (!string.IsNullOrEmpty(backType) && backType == "jqGridDetailList")
        {
            Response.Redirect("../purchase/PurchaseDetailListForJqGrid.aspx?searchKey=" + Request.QueryString["searchKey"]);
        }
        else
        {
            Response.Redirect("../purchase/PurchaseList.aspx");
        }
    }
    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        BackTo();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProductGrid();
    }
    #region private
    private void InsertOrder()
    {
        string sql = @"insert into tb_purchase_order (order_num,order_name,contract_id,project_id,
                        apply_date,memo,leader,create_date,creater) values (@num,@name,@contractId,@projectId,@applyDate,@memo,@leader,Now(),@user);select @@IDENTITY";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();
            try
            {
                DbCommand cmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                SetOrderParameters(cmd, access);
                int orderId = Convert.ToInt32(access.ExecuteScalar(cmd));
                for(int i = 1; i<= Convert.ToInt32(pcount.Value);i++)
                {
                    //此行已经删除
                    if (string.IsNullOrEmpty(Request.Form["pid" + i]))
                    {
                        continue;
                    }
                    InsertOrderDetail(access, transaction, orderId.ToString(), i);
                }
                UpdateAmount(orderId, transaction, access, conn);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            Response.Redirect("../purchase/PurchaseList.aspx");
        }
    }
    private static void UpdateAmount(int orderId, DbTransaction trans, DBAccess ac, DbConnection conn)
    {
        string sql = @"UPDATE tb_purchase_order A INNER JOIN
                        (SELECT order_id,SUM(price) price FROM tb_purchase_orderdetail
                        GROUP BY order_id) B ON(A.id = B.order_id)
                        SET A.amount = B.price where A.id = {0}";
        DbCommand cmd = ac.CreateCommand(string.Format(sql, orderId), conn);
        cmd.Transaction = trans;
        ac.ExecuteNonQuery(cmd);
    }
    private void UpdateOrder(string orderId)
    {
        string sql = @"update tb_purchase_order set order_num=@num,order_name=@name,contract_id=@contractId,
                    project_id=@projectId,apply_date=@applyDate,memo=@memo,leader=@leader,update_time=Now(),updater=@user 
                    where id = @id";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();
            try
            {
                DbCommand cmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                SetOrderParameters(cmd, access);
                cmd.Parameters.Add(access.GetParameter("@id", orderId));
                access.ExecuteNonQuery(cmd);
                sql = @"select product_id from tb_purchase_orderDetail where order_id=" + orderId;
                DataTable tb = DBHelper.GetTableBySql(sql);
                List<int> productIds = new List<int>();
                if (tb.Rows.Count > 0)
                {
                    productIds = (from r in tb.AsEnumerable()
                                  select r.Field<int>("product_id")).ToList();
                }
                List<int> existProducts = new List<int>();
                int pCount = Convert.ToInt32(pcount.Value.ToString());
                for (int i = 1; i <= pCount; i++)
                {
                    //此行已经删除
                    if (string.IsNullOrEmpty(Request.Form["pid" + i]))
                    {
                        continue;
                    }
                    int productId = Convert.ToInt32(Request.Form["pid" + i]);
                    existProducts.Add(productId);
                    if (productIds.Contains(productId))
                    {
                        UpdateOrderDetail(access, transaction, orderId, i);
                    }
                    else
                    {
                        InsertOrderDetail(access, transaction, orderId, i);
                    }
                }
                //delete
                sql = "delete from tb_purchase_orderDetail where order_id={0} and product_id not in ({1})";
                sql = string.Format(sql, orderId, string.Join(",", existProducts));
                DbCommand delCmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                access.ExecuteNonQuery(delCmd);

                if (txt_leader.Value.Trim() != txt_leader_old.Value.Trim())
                {
                    DbCommand cmd_leader = access.CreateCommand(string.Format("update tb_purchase_orderdetail set leader='{0}' where order_id={1}",txt_leader.Value.Trim(),orderId), conn);
                    cmd.Transaction = transaction;
                    access.ExecuteNonQuery(cmd_leader);
                }
                UpdateAmount(Int32.Parse(orderId), transaction, access, conn);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

        }
    }
    /*
    private void UpdateOrder_back(string orderId)
    {
        string sql = @"update tb_purchase_order set order_num=@num,order_name=@name,contract_id=@contractId,
                    project_id=@projectId,apply_date=@applyDate,memo=@memo,leader=@leader
                    where id = @id";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();
            try
            {
                DbCommand cmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                SetOrderParameters(cmd, access);
                cmd.Parameters.Add(access.GetParameter("@id", orderId));
                access.ExecuteNonQuery(cmd);
                sql = @"select product_id from tb_purchase_orderDetail where order_id=" + orderId;
                DataTable tb = DBHelper.GetTableBySql(sql);
                List<int> productIds = new List<int>();
                if (tb.Rows.Count > 0)
                {
                    productIds = (from r in tb.AsEnumerable()
                                  select r.Field<int>("product_id")).ToList();
                }
                List<int> existProducts = new List<int>();
                foreach (GridViewRow row in grid_productList.Rows)
                {
                    //string s = ((DataBoundLiteralControl)row.Cells[0].Controls[0]).Text.Trim();
                    //没有找到合适的判断行被删除的办法，暂时用textBox的来判断，因为当本行被删除后文本框的值被清空
                    //用此办法来判断此行是否被删除
                    string delFlag = ((TextBox)row.FindControl("txt_quantity")).Text.Trim();
                    if (delFlag == "")
                    {
                        continue;
                    }
                    int productId = Convert.ToInt32(grid_productList.DataKeys[row.RowIndex][0].ToString());
                    existProducts.Add(productId);
                    if (productIds.Contains(productId))
                    {
                        UpdateOrderDetail(access, transaction, orderId, row);
                    }
                    else
                    {
                        InsertOrderDetail(access, transaction, orderId, row);
                    }
                }

                //delete
                sql = "delete from tb_purchase_orderDetail where order_id={0} and product_id not in ({1})";
                sql = string.Format(sql, orderId, string.Join(",", existProducts));
                DbCommand delCmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                access.ExecuteNonQuery(delCmd);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

        }
    }
    */
    private void SetDetailParameters(DbCommand cmd, DBAccess access, string orderId, int index)
    {
        cmd.Parameters.Add(access.GetParameter("@orderId", orderId));
        cmd.Parameters.Add(access.GetParameter("@productId", Request.Form["pid" + index]));
        if (string.IsNullOrEmpty(Request.Form["txt_supplier" + index].ToString()))
        {
            cmd.Parameters.Add(access.GetParameter("@supplierId", Request.Form["ddl_supplier" + index]));
        }
        else
        {
            string supplier = Request.Form["txt_supplier" + index];
            string sql = "select id from tb_code_list where name = '{0}' and type={1}";
            sql = string.Format(sql, supplier, (int)CodeListType.SupplierName);
            DataTable tb = DBHelper.GetTableBySql(sql);
            if (tb.Rows.Count > 0)
            {
                cmd.Parameters.Add(access.GetParameter("@supplierId", tb.Rows[0][0]));
            }
            else
            {
                sql = "insert into tb_code_list (name,type) values ('{0}',{1}); select @@IDENTITY";
                sql = string.Format(sql, supplier, (int)CodeListType.SupplierName);
                DBAccess ac = DBAccess.CreateInstance();
                int supplierId = 0;
                using (DbConnection conn = ac.GetConnection())
                {
                    conn.Open();
                    DbCommand c = ac.CreateCommand(sql, conn);
                    supplierId = Convert.ToInt32(ac.ExecuteScalar(c));
                }
                cmd.Parameters.Add(access.GetParameter("@supplierId", supplierId));

            }
        }
        cmd.Parameters.Add(access.GetParameter("@deliveryDate", Common.ConvertToDBValue(Request.Form["txt_deliveryDate" + index])));
        cmd.Parameters.Add(access.GetParameter("@inWarehouseDate", Common.ConvertToDBValue(Request.Form["txt_warehouseDate" + index])));

        string unitPrice = Request.Form["txt_unitPrice" + index];
        string quantity = Request.Form["txt_quantity" + index];
        if (string.IsNullOrEmpty(unitPrice))
        {
            cmd.Parameters.Add(access.GetParameter("@unitPrice", unitPrice));
        }
        else
        {
            cmd.Parameters.Add(access.GetParameter("@unitPrice", Convert.ToDecimal(unitPrice)));
        }
        cmd.Parameters.Add(access.GetParameter("@quantity", quantity));
        string price = string.Empty;
        if (string.IsNullOrEmpty(unitPrice) || string.IsNullOrEmpty(quantity))
        {
            cmd.Parameters.Add(access.GetParameter("@price", 0));
        }
        else
        {
            cmd.Parameters.Add(access.GetParameter("@price", Convert.ToDecimal(unitPrice)*Convert.ToInt32(quantity)));
        }
        cmd.Parameters.Add(access.GetParameter("@memo", Request.Form["txt_memo" + index]));
        cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value));
    }
    private void SetOrderParameters(DbCommand cmd,DBAccess access)
    {
        cmd.Parameters.Add(access.GetParameter("@num", txt_orderNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@name", ""));
        cmd.Parameters.Add(access.GetParameter("@contractId", txt_contractNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@projectId", ddl_project.SelectedValue));
        cmd.Parameters.Add(access.GetParameter("@applyDate", Common.ConvertToDBValue(txt_applyDate.Value)));
        cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@memo", txt_memo.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@user", ((CMS.Model.User)HttpContext.Current.Session["User"]).UserName));

    }
    private void UpdateOrderDetail(DBAccess access, DbTransaction transaction, string orderId, int index)
    {
        string sql = @"update tb_purchase_orderDetail set supplier_id=@supplierId,
                        delivery_date=@deliveryDate,in_warehouse_date=@inWarehouseDate,unit_price=@unitPrice,quantity=@quantity,price=@price,memo=@memo 
                        where order_id=@orderId and product_id=@productId";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        SetDetailParameters(cmd, access, orderId, index); ;
        access.ExecuteNonQuery(cmd);
    }
    private void InsertOrderDetail(DBAccess access, DbTransaction transaction, string orderId, int index)
    {
        string sql = @"insert into tb_purchase_orderDetail(order_id,product_id,
                            supplier_id,delivery_date,unit_price,quantity,price,memo,leader,in_warehouse_date)
                                values(@orderId,@productId,@supplierId,@deliveryDate,@unitPrice,
                                @quantity,@price,@memo,@leader,@inWarehouseDate)";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        SetDetailParameters(cmd, access, orderId, index); ;
        access.ExecuteNonQuery(cmd);
    }
    #endregion

    [WebMethod]
    public static string GetContractInfo(string num)
    {
        string sql = @"SELECT b.name moneyType,a.contract_amount 
                        FROM tb_contract a
                        LEFT JOIN tb_code_list b ON (a.money_type = b.id)
                        WHERE a.contract_num='{0}' ";
        sql = string.Format(sql, num.Replace("'", ""));
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            var obj = new
            {
                moneyType = tb.Rows[0]["moneyType"].ToString(),
                amount = Convert.ToInt32(tb.Rows[0]["contract_amount"])
            };
            string result = JsonConvert.SerializeObject(obj);
            return result;
        }
        return "";
    }
    [WebMethod]
    public static string AddNewProduct(string num,string name,string category, string size,string material,string unit)
    {
        string sql = @"select * from tb_product where product_num='{0}' and product_size='{1}' and product_category_id={2} and product_unit_id={3} and product_material='{4}' and product_name='{5}'";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, new object[] { num, size, category, unit, material, name }));
        if (tb.Rows.Count > 0)
        {
            return "false";
        }
        sql = @"insert into tb_product(product_num,product_name,product_size,product_category_id,product_unit_id,product_material)
            values(@num,@name,@size,@category,@unit,@material)";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(sql, conn);
            cmd.Parameters.Add(access.GetParameter("@num", num));
            cmd.Parameters.Add(access.GetParameter("@name", name));
            cmd.Parameters.Add(access.GetParameter("@size", size));
            cmd.Parameters.Add(access.GetParameter("@category", category));
            cmd.Parameters.Add(access.GetParameter("@material", material));
            cmd.Parameters.Add(access.GetParameter("@unit", unit));
            access.ExecuteNonQuery(cmd);
        }
        return "true";
    }
    protected void grid_product_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        string sortDirection = "ASC";
        if (sortExpression == grid_product.Attributes["SortExpression"])
        {
            sortDirection = (grid_product.Attributes["SortDirection"].ToString() ==
            sortDirection ? "DESC" : "ASC");
        }
        grid_product.Attributes["SortExpression"] = sortExpression;
        grid_product.Attributes["SortDirection"] = sortDirection;
        BindProductGrid();
    }

    protected void btnImport_ServerClick(object sender, EventArgs e)
    {
        //HttpPostedFile files = Request.Files["fileUpload"];
        string filePath = GetExcel();
        DataTable tb = ExcelToTable(filePath);
        //ImportTableToDB(tb, "tb_import_temp");
        File.Delete(filePath);
        grid.DataSource = tb;
        grid.DataBind();
    }
    #region Save import
    protected void btnImportSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, int> categoryList = GetCodeList((int)CodeListType.ProductCategery);
        Dictionary<string, int> unitList = GetCodeList((int)CodeListType.ProductUnit);
        Dictionary<string, int> supplierList = GetCodeList((int)CodeListType.SupplierName);

        List<PurchaseDetail> detailList = new List<PurchaseDetail>();
        foreach (GridViewRow row in grid.Rows)
        {
            PurchaseDetail detail = new PurchaseDetail();
            string quantity = ((TextBox)row.FindControl("txt_col7")).Text.Trim();
            detail.Quantity = Convert.ToInt32(string.IsNullOrEmpty(quantity) ? "0" : quantity);
            detail.DeliveryDate = ((TextBox)row.FindControl("txt_col9")).Text.Trim();
            string unitPrice = ((TextBox)row.FindControl("txt_col10")).Text.Trim();
            detail.UnitPrice = Convert.ToDecimal(string.IsNullOrEmpty(unitPrice) ? "0" : unitPrice);
            detail.InWarehouseDate = ((TextBox)row.FindControl("txt_col11")).Text.Trim();
            detail.Memo = ((TextBox)row.FindControl("txt_col12")).Text.Trim();

            Product product = new Product();
            detail.Product = product;

            product.Name = ((TextBox)row.FindControl("txt_col2")).Text.Trim();
            string category = ((TextBox)row.FindControl("txt_col3")).Text.Trim();
            product.Size = ((TextBox)row.FindControl("txt_col4")).Text.Trim();
            product.Material = ((TextBox)row.FindControl("txt_col5")).Text.Trim();
            string unit = ((TextBox)row.FindControl("txt_col6")).Text.Trim();
            string supplier = ((TextBox)row.FindControl("txt_col8")).Text.Trim();

            string sql = "select id from tb_product where product_name=@name and product_size=@size and product_material =@material";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", product.Name);
            parameters.Add("@size", product.Size);
            parameters.Add("@material", product.Material);
            DataTable tb = DBHelper.GetTableBySql(sql, parameters);
            //商品已经存在
            if (tb.Rows.Count > 0)
            {
                product.Id = Convert.ToInt32(tb.Rows[0][0]);
            }
            else//商品不存在，需要添加新商品到数据库
            {
                //category
                if (categoryList.ContainsKey(category))
                {
                    product.CategoryId = categoryList[category];
                }
                else
                {
                    product.CategoryId = AddNewCode(category, (int)CodeListType.ProductCategery);
                    categoryList.Add(category, product.CategoryId);
                }
                //unit
                if (unitList.ContainsKey(unit))
                {
                    product.UnitId = unitList[unit];
                }
                else
                {
                    product.UnitId = AddNewCode(unit, (int)CodeListType.ProductUnit);
                    unitList.Add(unit, product.UnitId);
                }
            }

            //供应商
            if (supplierList.ContainsKey(supplier))
            {
                detail.SupplierId = supplierList[supplier];
            }
            else
            {
                detail.SupplierId = AddNewCode(supplier, (int)CodeListType.SupplierName);
                supplierList.Add(supplier, detail.SupplierId);
            }

            detailList.Add(detail);
        }
        int orderId = InsertOrder(detailList);
        Response.Write("<script>Alert('订单导入成功，您可以继续修改或完善！');</script>");
        Response.Redirect("../purchase/purchaseDetail.aspx?id=" + orderId);

    }
    private int InsertOrder(List<PurchaseDetail> list)
    {
        int orderId;
        string sql = @"insert into tb_purchase_order (order_num,order_name,contract_id,project_id,
                        apply_date,memo,leader,creater,create_date) values (@num,@name,@contractId,@projectId,@applyDate,@memo,@leader,@user,Now());select @@IDENTITY";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();
            try
            {
                DbCommand cmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                SetOrderParameters(cmd, access);
                orderId = Convert.ToInt32(access.ExecuteScalar(cmd));
                foreach (PurchaseDetail detail in list)
                {
                    if (detail.Product.Id == 0)
                    {
                        // TODO: insert product
                        detail.Product.Id = AddNewProduct(access, conn, transaction, detail.Product);
                    }
                    InsertImportOrderDetail(access, transaction, orderId, detail);
                }
                UpdateAmount(orderId, transaction, access, conn);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
        return orderId;
    }
    private void InsertImportOrderDetail(DBAccess access, DbTransaction transaction, int orderId, PurchaseDetail detail)
    {
        string sql = @"insert into tb_purchase_orderDetail(order_id,product_id,
                            supplier_id,delivery_date,unit_price,quantity,price,memo,leader,in_warehouse_date)
                                values(@orderId,@productId,@supplierId,@deliveryDate,@unitPrice,
                                @quantity,@price,@memo,@leader,@warehouseDate)";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        cmd.Parameters.Add(access.GetParameter("@orderId", orderId));
        cmd.Parameters.Add(access.GetParameter("@productId", detail.Product.Id));
        cmd.Parameters.Add(access.GetParameter("@supplierId", detail.SupplierId));
        cmd.Parameters.Add(access.GetParameter("@deliveryDate", Common.ConvertToDBValue(detail.DeliveryDate)));
        cmd.Parameters.Add(access.GetParameter("@warehouseDate", Common.ConvertToDBValue(detail.InWarehouseDate)));
        cmd.Parameters.Add(access.GetParameter("@unitPrice", detail.UnitPrice));
        cmd.Parameters.Add(access.GetParameter("@quantity", detail.Quantity));
        cmd.Parameters.Add(access.GetParameter("@price", detail.UnitPrice * detail.Quantity));
        cmd.Parameters.Add(access.GetParameter("@memo", detail.Memo));
        cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value));
        access.ExecuteNonQuery(cmd);
    }

    public int AddNewProduct(DBAccess access, DbConnection conn, DbTransaction transaction, Product product)
    {
        string sql = @"insert into tb_product(product_num,product_name,product_size,product_category_id,product_unit_id,product_material)
            values(@num,@name,@size,@category,@unit,@material);select @@IDENTITY";
        DbCommand cmd = access.CreateCommand(sql, conn);
        cmd.Transaction = transaction;
        cmd.Parameters.Add(access.GetParameter("@num", ""));
        cmd.Parameters.Add(access.GetParameter("@name", product.Name));
        cmd.Parameters.Add(access.GetParameter("@size", product.Size));
        cmd.Parameters.Add(access.GetParameter("@category", product.CategoryId));
        cmd.Parameters.Add(access.GetParameter("@material", product.Material));
        cmd.Parameters.Add(access.GetParameter("@unit", product.UnitId));
        return Convert.ToInt32(access.ExecuteScalar(cmd));
    }

    private Dictionary<string, int> GetCodeList(int type)
    {
        Dictionary<string, int> list = new Dictionary<string, int>();
        DataTable tb = DBHelper.GetTableBySql(string.Format("select name,id from tb_code_list where type= {0}", type));
        foreach (DataRow row in tb.Rows)
        {
            string name = row["name"].ToString();
            if (!list.ContainsKey(name))
            {
                list.Add(name, Convert.ToInt32(row["id"]));
            }
        }
        return list;

    }
    private int AddNewCode(string name, int type)
    {
        DBAccess ac = DBAccess.CreateInstance();
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            string sql = "insert into tb_code_list (name,type) value (@name,@type);select @@IDENTITY";
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@name", name));
            cmd.Parameters.Add(ac.GetParameter("@type", type));
            return Convert.ToInt32(ac.ExecuteScalar(cmd));
        }
    }
    #endregion
    private void ImportTableToDB(DataTable tb, string tbName)
    {
        DBAccess ac = DBAccess.CreateInstance();
        ac.BulkCopy(tb, tbName);
    }

    private string GetExcel()
    {
        string fileUrl = string.Empty;
        try
        {
            //全名
            //string excelFile = this.txt_file.Value;//this.fileUpload.PostedFile.FileName;
            string excelFile = this.fileUpload.PostedFile.FileName;

            string extention = Path.GetExtension(excelFile);
            string fileName = Path.GetFileNameWithoutExtension(excelFile);
            if (string.IsNullOrEmpty(fileName))
            {
                Response.Write("<script>Alert('请选择excel文件！');</script>");
                return null;
            }
            if (extention == ".xls" || extention == ".xlsx")
            {
                //浏览器安全性限制 无法直接获取客户端文件的真是路径，需要将文件上传到服务器，然后再从服务器获取文件源路径
                //设置上传路径，将文件保存到服务器
                string newFileName = fileName + DateTime.Now.Date.ToString("yyyyMMdd") + extention;
                fileUrl = "c:\\Report\\" + newFileName;
                this.fileUpload.PostedFile.SaveAs(fileUrl);
                return fileUrl;
            }
            else
            {
                Response.Write("<script>alert('请选择excel文件！');</script>");
                return null;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable ExcelToTable(string file)
    {
        DataTable tb = DBHelper.GetTableBySql("select * from tb_import_temp where 1= 2");
        IWorkbook workBook;
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            if (Path.GetExtension(file) == ".xls")
            {
                workBook = new HSSFWorkbook(fs);
            }
            else if (Path.GetExtension(file) == ".xlsx")
            {
                workBook = new XSSFWorkbook(fs);
            }
            else
            {
                workBook = null;
            }
            ISheet sheet = workBook.GetSheetAt(0);
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow row = tb.NewRow();
                // 共16列
                for (int j = 0; j < 16; j++)
                {
                    try
                    {
                        ICell cell = sheet.GetRow(i).GetCell(j);
                        if (j == 8 || j==10)
                        {
                            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                            {
                                row[j] = cell.DateCellValue.ToString("yyyy-MM-dd");
                            }
                        }
                        else
                        {
                            row[j] = cell;
                        }
                    }
                    catch (Exception e)
                    {
                        row[j] = string.Empty;
                    }
                }
                tb.Rows.Add(row);
            }
            return tb;
        }
    }

    protected void btnDownloadTemplate_ServerClick(object sender, EventArgs e)
    {
        string filePath = "c:\\Report\\PurchaseListTemplate.xlsx";

        System.IO.FileInfo file = new System.IO.FileInfo(filePath);
        Response.Clear();
        //下载后的文件名
        Response.AddHeader("Content-Disposition",
            "attachment;filename=PurchaseListTemplate.xlsx");
        Response.AddHeader("Content-Length", file.Length.ToString());
        Response.Charset = "UTF-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.ContentType = "application/vnd.ms-excel";
        Response.WriteFile(filePath);
        Response.End();
    }
}
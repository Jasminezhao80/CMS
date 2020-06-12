using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class purchase_PurchaseDetail_back : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.DropDownBind(ddl_project, (int)CodeListType.ProjectName, "");
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                Load(Convert.ToInt32(id));
                BindPurchaseDetailGrid(Convert.ToInt32(id));
            }
            //txt_searchKey.Value = string.Empty;
            //BindProductGrid();
        }
    }
    private void BindPurchaseDetailGrid(int orderId)
    {
        string sql = @"SELECT B.id,A.supplier_id,A.delivery_date,A.unit_price,A.quantity,A.quantity,A.memo,A.price,
                        B.product_num,B.product_name,B.product_size,C.name AS category,D.name AS unit
                        FROM tb_purchase_orderdetail A
                        LEFT JOIN tb_product B ON (A.product_id = B.id)
                        LEFT JOIN tb_code_list C ON(B.product_category_id = c.id)
                        LEFT JOIN tb_code_list D ON (B.product_unit_id = D.id)
                        WHERE A.order_id = " + orderId;
        DataTable tb = DBHelper.GetTableBySql(sql);
        grid_productList.DataSource = tb;
        grid_productList.DataKeyNames = new string[] { "id" };
        grid_productList.DataBind();
    }
    private void Load(int id)
    {
        string sql = @"select * from tb_purchase_order where id="+id;
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            txt_applyDate.Value = Common.ConvertToDate(row["apply_date"]);
            txt_orderNum.Value = row["order_num"].ToString();
            txt_name.Value = row["order_name"].ToString();
            txt_contractNum.Value = row["contract_id"].ToString();
            txt_leader.Value = row["leader"].ToString();
            txt_memo.Value = row["memo"].ToString();
            ddl_project.SelectedValue = row["project_id"].ToString();
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

    private void BindProductGrid()
    {
        string searchKey = txt_searchKey.Value.Trim();
        string sql = @"select A.id,product_num,product_material,product_name,product_size,product_category_id,product_unit_id,B.name as category,C.name as unit 
                        from tb_product A  
                        left join tb_code_list B on (A.product_category_id = B.id) 
                        left join tb_code_list C on (A.product_unit_id = C.id) ";
        if (!string.IsNullOrEmpty(searchKey))
        {
            sql = sql + " where product_num like '%{0}%' or product_name like '%{0}%' or product_size like '%{0}%' or product_material like '%{0}%'";
            sql = string.Format(sql, searchKey);
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
        Response.Redirect("../purchase/PurchaseList.aspx");

    }

    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("../purchase/PurchaseList.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProductGrid();
    }
    #region private
    private void InsertOrder()
    {
        string sql = @"insert into tb_purchase_order (order_num,order_name,contract_id,project_id,
                        apply_date,memo,leader) values (@num,@name,@contractId,@projectId,@applyDate,@memo,@leader);select @@IDENTITY";
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
                foreach (GridViewRow row in grid_productList.Rows)
                {
                    InsertOrderDetail(access, transaction, orderId.ToString(), row);
                }
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
    private void UpdateOrder(string orderId)
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
    private void SetDetailParameters(DbCommand cmd, DBAccess access, string orderId, GridViewRow row)
    {
        cmd.Parameters.Add(access.GetParameter("@orderId", orderId));
        cmd.Parameters.Add(access.GetParameter("@productId", grid_productList.DataKeys[row.RowIndex].Value));
        cmd.Parameters.Add(access.GetParameter("@supplierId", ((DropDownList)row.FindControl("ddl_supplier")).SelectedValue));
        cmd.Parameters.Add(access.GetParameter("@deliveryDate", Common.ConvertToDBValue(((HtmlInputGenericControl)row.FindControl("txt_deliveryDate")).Value.Trim())));
        cmd.Parameters.Add(access.GetParameter("@unitPrice", ((TextBox)row.FindControl("txt_unitPrice")).Text.Trim()));
        cmd.Parameters.Add(access.GetParameter("@quantity", ((TextBox)row.FindControl("txt_quantity")).Text.Trim()));
        cmd.Parameters.Add(access.GetParameter("@price", ((TextBox)row.FindControl("txt_quantity")).Text.Trim()));
        cmd.Parameters.Add(access.GetParameter("@memo", ((TextBox)row.FindControl("txt_memo")).Text.Trim()));
    }
    private void SetOrderParameters(DbCommand cmd,DBAccess access)
    {
        cmd.Parameters.Add(access.GetParameter("@num", txt_orderNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@name", txt_name.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@contractId", txt_contractNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@projectId", ddl_project.SelectedValue));
        cmd.Parameters.Add(access.GetParameter("@applyDate", Common.ConvertToDBValue(txt_applyDate.Value)));
        cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@memo", txt_memo.Value.Trim()));

    }
    private void UpdateOrderDetail(DBAccess access, DbTransaction transaction, string orderId, GridViewRow row)
    {
        string sql = @"update tb_purchase_orderDetail set supplier_id=@supplierId,
                        delivery_date=@deliveryDate,unit_price=@unitPrice,quantity=@quantity,price=@price,memo=@memo 
                        where order_id=@orderId and product_id=@productId";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        SetDetailParameters(cmd, access, orderId, row); ;
        access.ExecuteNonQuery(cmd);
    }
    private void InsertOrderDetail(DBAccess access, DbTransaction transaction, string orderId, GridViewRow row)
    {
        string sql = @"insert into tb_purchase_orderDetail(order_id,product_id,
                            supplier_id,delivery_date,unit_price,quantity,price,memo)
                                values(@orderId,@productId,@supplierId,@deliveryDate,@unitPrice,
                                @quantity,@price,@memo)";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        SetDetailParameters(cmd, access, orderId, row); ;
        access.ExecuteNonQuery(cmd);
    }
    #endregion
}
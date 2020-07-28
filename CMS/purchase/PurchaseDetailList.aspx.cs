using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Services;
using System.Web.UI.WebControls;
using CMS.Model;
using CMS.DB.Imp;

public partial class purchase_PurchaseDetailList : System.Web.UI.Page
{
    private static DataTable supplierTb;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.DropDownBind(ddl_project, (int)CodeListType.ProjectName, true);
            Common.DropDownBind(ddl_supplier, (int)CodeListType.SupplierName, true);
            string sql = @"select id,name from tb_code_list where type={0} order by name";
            sql = string.Format(sql, (int)CodeListType.SupplierName);
            supplierTb = DBHelper.GetTableBySql(sql);
            if (!string.IsNullOrEmpty(Request.QueryString["searchKey"]))
            {
                string[] searchKeys = Request.QueryString["searchKey"].Split('_');
                ddl_project.SelectedValue = searchKeys[0];
                ddl_supplier.SelectedValue = searchKeys[1];
                ddl_isInWarehouse.SelectedValue = searchKeys[2];
                txt_searchKey.Value = searchKeys[3];
            }
            BindGrid();
        }

    }
    private void BindGrid()
    {
        string sql = @"SELECT A.product_id,A.order_id,A.price,A.quantity,A.in_warehouse_date,A.id,B.order_num,D.name AS projectName,E.name AS category,B.contract_id,B.apply_date,A.delivery_date,
                C.product_name,C.product_size,C.product_material,F.name AS unit,A.unit_price,G.name AS supplier,A.leader,A.memo,A.supplier_id 
                FROM tb_purchase_orderdetail A
                LEFT JOIN tb_purchase_order B ON (A.order_id = B.id)
                LEFT JOIN tb_product C on (A.product_id = C.id)
                LEFT JOIN tb_code_list D ON (B.project_id = D.id)
                LEFT JOIN tb_code_list E ON (C.product_category_id = E.id)
                LEFT JOIN tb_code_list F ON(F.id = C.product_unit_id)
                LEFT JOIN tb_code_list G ON (G.id = A.supplier_id) where B.is_disabled <> 1";
        if (ddl_project.SelectedValue != "0")
        {
            sql += " and B.project_id ='" + ddl_project.SelectedValue + "'";
        }
        if (ddl_supplier.SelectedValue != "0")
        {
            sql += " and A.supplier_id ='" + ddl_supplier.SelectedValue + "'";
        }
        if (ddl_isInWarehouse.SelectedValue == "1")
        {
            sql += " and A.in_warehouse_date is null";
        }
        if (ddl_isInWarehouse.SelectedValue == "2")
        {
            sql += " and A.in_warehouse_date is not null";
        }
        if (!string.IsNullOrEmpty(txt_searchKey.Value.Trim()))
        {
            sql += @" and (B.order_num like '%{0}%' or B.contract_id like '%{0}%' or C.product_name like '%{0}%' or C.product_size like '%{0}%' or E.name like '%{0}%')";
        }
        sql = string.Format(sql, txt_searchKey.Value.Trim().Replace(",", ""));
        sql += " order by order_num DESC,A.id";
        DataTable tb = DBHelper.GetTableBySql(sql);
        //DataTable tb = DBHelper.GetTableBySql(sql, "@key",txt_searchKey.Value.Trim());
        grid_detail.DataSource = tb;
        grid_detail.DataKeyNames = new string[] { "id" };
        grid_detail.DataBind();
    }

    protected string GetSearchKeys()
    {
        return string.Format("{0}_{1}_{2}_{3}", ddl_project.SelectedValue, ddl_supplier.SelectedValue, ddl_isInWarehouse.SelectedValue, txt_searchKey.Value.Trim());
    }
    protected void btn_Search_ServerClick(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddl_project_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddl_supplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void grid_detail_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            //规格
            e.Row.Cells[7].Attributes.Add("title", ((DataRowView)e.Row.DataItem)["product_size"].ToString());
            //类别
            e.Row.Cells[2].Attributes.Add("title", ((DataRowView)e.Row.DataItem)["category"].ToString());
            //名称
            e.Row.Cells[6].Attributes.Add("title", ((DataRowView)e.Row.DataItem)["product_name"].ToString());
            //供应商
            DropDownList supplier = (DropDownList)e.Row.FindControl("ddl_supplierDetail");
            SupplierBind(supplier);
            supplier.SelectedValue = ((DataRowView)e.Row.DataItem)["supplier_id"].ToString();
        }
    }
    private void SupplierBind(DropDownList dropDownList)
    {
        dropDownList.DataSource = supplierTb;
        dropDownList.DataValueField = "id";
        dropDownList.DataTextField = "name";
        dropDownList.DataBind();
        dropDownList.Items.Insert(0, new ListItem("", "0"));
        dropDownList.SelectedIndex = 0;
    }
    [WebMethod]
    public static void SavePrice(int id, double price)
    {
        DBAccess ac = DBAccess.CreateInstance();
        string sql = "update tb_purchase_orderdetail set unit_price = @price,price=@price*quantity where id=@id";
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbTransaction trans = conn.BeginTransaction();
            try
            {
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@id", id));
                cmd.Parameters.Add(ac.GetParameter("@price", price));
                ac.ExecuteNonQuery(cmd);
                UpdateAmount(id, trans, ac, conn);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }
    public static void UpdateAmount(int detailId, DbTransaction trans, DBAccess ac, DbConnection conn)
    {
        string sql = @"UPDATE tb_purchase_order A INNER JOIN
                        (SELECT order_id,SUM(price) price FROM tb_purchase_orderdetail
                        GROUP BY order_id) B ON(A.id = B.order_id)
                        INNER JOIN tb_purchase_orderdetail C ON (C.id = {0} AND A.id = C.order_id)
                        SET A.amount = B.price";
        DbCommand cmd = ac.CreateCommand(string.Format(sql, detailId), conn);
        cmd.Transaction = trans;
        ac.ExecuteNonQuery(cmd);
    }
    [WebMethod]
    public static void SaveQuantity(int id, int quantity)
    {
        DBAccess ac = DBAccess.CreateInstance();
        string sql = "update tb_purchase_orderdetail set quantity = @quantity,price=unit_price*@quantity where id=@id";
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbTransaction trans = conn.BeginTransaction();
            try
            {

                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@id", id));
                cmd.Parameters.Add(ac.GetParameter("@quantity", quantity));
                ac.ExecuteNonQuery(cmd);
                UpdateAmount(id, trans, ac, conn);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }

    [WebMethod]
    public static void UpdateProduct(int id,int productId,string value,string item)
    {
        ProductImp Imp = new ProductImp();
        Product product = Imp.GetProductById(productId);
        //根据修改后的规格型号，检查系统中是否已经存在此商品
        if (item == "name")
        {
            product.Name = value;
        }
        else
        {
            product.Size = value;
        }
        
        int newId = Imp.GetProductId(product.Name, product.Size, product.Material);
        if (newId == 0)
        {
            //不存在此规格型号的商品时，创建新商品
            newId = Imp.InsertProduct(product);
        }
        PurchaseDetailImp detailImp = new PurchaseDetailImp();
        detailImp.UpdateProductId(id, newId);
    }
    [WebMethod]
    public static void SaveWareHouseDate(int id, string date)
    {
        DBAccess ac = DBAccess.CreateInstance();
        string sql = "update tb_purchase_orderdetail set in_warehouse_date = @date where id=@id";
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@id", id));
            cmd.Parameters.Add(ac.GetParameter("@date", Common.ConvertToDBValue(date)));
            ac.ExecuteNonQuery(cmd);
        }
    }

    /// <summary>
    /// item:数据库字段名称
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <param name="item"></param>
    [WebMethod]
    public static void ChangeValue(int id, string value,string item)
    {
        DBAccess ac = DBAccess.CreateInstance();
        string sql = "update tb_purchase_orderdetail set {0} = @value where id=@id";
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbCommand cmd = ac.CreateCommand(string.Format(sql,item), conn);
            cmd.Parameters.Add(ac.GetParameter("@id", id));
            cmd.Parameters.Add(ac.GetParameter("@value", value));
            ac.ExecuteNonQuery(cmd);
        }
    }

    protected void grid_detail_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        grid_detail.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ddl_isInWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void ddl_supplierDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl =(DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.Parent.Parent;
        DBAccess ac = DBAccess.CreateInstance();
        string sql = "update tb_purchase_orderdetail set supplier_id = @value where id=@id";
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@id", grid_detail.DataKeys[row.RowIndex].Value));
            cmd.Parameters.Add(ac.GetParameter("@value", ((DropDownList)sender).SelectedValue));
            ac.ExecuteNonQuery(cmd);
        }
    }
}
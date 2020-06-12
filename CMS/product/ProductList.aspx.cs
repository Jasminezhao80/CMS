using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DB;

public partial class product_ProductList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.DropDownBind(ddl_type, (int)CodeListType.ProductCategery, false);
            Common.DropDownBind(ddl_unit, (int)CodeListType.ProductUnit, false);
            BindGrid();
        }
    }
    private void BindGrid()
    {
        string sql = @"select A.id,product_num,product_name,product_material,product_size,product_category_id,product_unit_id,B.name as category,C.name as unit 
                        from tb_product A  
                        left join tb_code_list B on (A.product_category_id = B.id) 
                        left join tb_code_list C on (A.product_unit_id = C.id) ";
        if (!string.IsNullOrEmpty(txt_search.Value.Trim()))
        {
            sql += " where A.product_name like '%{0}%' or A.product_material like '%{0}%' or A.product_size like '%{0}%' or B.name like '%{0}%'";
        }
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql,txt_search.Value));
        grid_product.DataSource = tb;
        grid_product.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txt_id.Value);
        string sql = string.Empty;
        if (id > 0)
        {
            sql = @"update tb_product set product_num=@num,product_name=@name,product_size=@size,
                    product_unit_id=@unit,product_category_id=@category,product_material=@material where id = " + id;
        }
        else
        {
            sql = @"insert into tb_product(product_num,product_name,product_size,product_category_id,product_unit_id,product_material)
                values(@num,@name,@size,@category,@unit,@material)";
        }
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(sql, conn);
            cmd.Parameters.Add(access.GetParameter("@num", ""));
            cmd.Parameters.Add(access.GetParameter("@name",txt_name.Value.Trim()));
            cmd.Parameters.Add(access.GetParameter("@size",txt_size.Value.Trim()));
            cmd.Parameters.Add(access.GetParameter("@unit", ddl_unit.SelectedItem.Value));
            cmd.Parameters.Add(access.GetParameter("@category", ddl_type.SelectedItem.Value));
            cmd.Parameters.Add(access.GetParameter("@material", txt_material.Value.Trim()));
            access.ExecuteNonQuery(cmd);
        }
        BindGrid();
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        string delSql = "delete from tb_product where id = @id";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(delSql, conn);
            cmd.Parameters.Add(access.GetParameter("@id", (sender as LinkButton).CommandArgument));
            access.ExecuteNonQuery(cmd);
        }
        BindGrid();
    }

    //protected void btnEdit_Click(object sender, EventArgs e)
    protected void btnEdit_Click()
    {
        string id = "2";//(sender as LinkButton).CommandArgument;
        txt_id.Value = id;//hidden textBox to save update row id
        string sql = "select * from tb_product where id = {0}";
        sql = string.Format(sql, id);
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            //txt_num.Value = row["product_num"].ToString();
            txt_name.Value = row["product_name"].ToString();
            txt_size.Value = row["product_size"].ToString();
            ddl_type.SelectedValue = row["product_category_id"].ToString();
            ddl_unit.SelectedValue = row["product_unit_id"].ToString();
            
        }
    }

    protected void Unnamed_ServerClick(object sender, EventArgs e)
    {
        BindGrid();
    }
}
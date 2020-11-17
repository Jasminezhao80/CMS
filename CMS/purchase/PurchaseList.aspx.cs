using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PurchaseList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //按钮权限
            btnAdd.Visible = Function.CheckButtonPermission("A020101");

            Common.DropDownBind(ddl_project, (int)CodeListType.ProjectName, true);
            Common.DropDownBind(ddl_disabled, (int)CodeListType.IsTrueType, true);
            BindGrid();
        }
    }
    private void BindGrid()
    {

        string sql = @"SELECT A.*,B.name AS projectName
                    from tb_purchase_order A
                    LEFT JOIN tb_code_list B ON (A.project_id = B.id)
                    where 1=1 ";
        if (ddl_project.SelectedItem.Value != "0")
        {
            sql += string.Format(" and A.project_id= '{0}'", ddl_project.SelectedItem.Value);
        }
        if (ddl_disabled.SelectedItem.Value != "0")
        {
            sql += string.Format(" and A.is_disabled = {0}", ddl_disabled.SelectedItem.Value == "20" ? "1" : "0");
        }
        if (!string.IsNullOrEmpty(txt_searchKey.Value.Trim()))
        {
            string sqlTemp = @" and (A.order_num like '%{0}%' 
                                or A.contract_id like '%{0}%' 
                                or A.leader like '%{0}%' 
                                or A.memo like '%{0}%')";
            sql += string.Format(sqlTemp, txt_searchKey.Value.Trim().Replace(",", ""));
        }
        sql += " order by A.order_num DESC";
        DataTable tb = DBHelper.GetTableBySql(sql);
        grid_list.DataSource = tb;
        grid_list.DataBind();
        grid_list.DataKeyNames = new string[] { "id"};
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("../purchase/PurchaseDetail.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../purchase/purchaseDetail.aspx?id=" + ((LinkButton)sender).CommandArgument);
    }

    protected void btnImport_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("../purchase/PurchaseImport.aspx");
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        DBAccess ac = DBAccess.CreateInstance();
        int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = "delete from tb_purchase_order where id =" + id;
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Transaction = tran;
                ac.ExecuteNonQuery(cmd);
                sql = "delete from tb_purchase_orderdetail where order_id = " + id;
                DbCommand detailCmd = ac.CreateCommand(sql, conn);
                detailCmd.Transaction = tran;
                ac.ExecuteNonQuery(detailCmd);
                tran.Commit();
                BindGrid();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }


        }
    }

    protected void btnDisable_Click(object sender, EventArgs e)
    {
        DBAccess ac = DBAccess.CreateInstance();
        int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            string sql = "update tb_purchase_order set is_disabled = 1 where id =" + id;
            DbCommand cmd = ac.CreateCommand(sql, conn);
            ac.ExecuteNonQuery(cmd);
            BindGrid();
        }
    }
    protected void btnEnable_Click(object sender, EventArgs e)
    {
        DBAccess ac = DBAccess.CreateInstance();
        int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            string sql = "update tb_purchase_order set is_disabled = 0 where id =" + id;
            DbCommand cmd = ac.CreateCommand(sql, conn);
            ac.ExecuteNonQuery(cmd);
            BindGrid();
        }
    }

    protected void grid_list_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string isDisabled = ((HtmlInputHidden)e.Row.FindControl("txt_isDisabled")).Value.Trim();
            if (isDisabled == "1")
            {
                
                ((HyperLink)e.Row.FindControl("linkNum")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnDisable")).Visible = false;
                ((LinkButton)e.Row.FindControl("btnEdit")).Visible = false;
                e.Row.BackColor = Color.WhiteSmoke;
            }
        }
    }

    protected void ddl_project_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddl_disabled_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btSearch_ServerClick(object sender, EventArgs e)
    {
        BindGrid();
    }
}
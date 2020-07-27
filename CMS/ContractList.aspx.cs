using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CMS.DB;
using System.Data.Common;

public partial class PurchaseList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         //string.IsNullOrEmpty(Request.QueryString["type"])
        if (!IsPostBack)
        {
            Dictionary<DropDownList, int[]> dropList = new Dictionary<DropDownList, int[]>();
            dropList.Add(ddl_category, new int[] { (int)CodeListType.ContractType});
            dropList.Add(ddl_type, new int[] { (int)CodeListType.PurchaseType,(int)CodeListType.ProjectType });
            dropList.Add(ddl_project, new int[] { (int)CodeListType.ProjectName });
            dropList.Add(ddl_IsComplete, new int[] { (int)CodeListType.IsTrueType });
            dropList.Add(ddl_IsAppointment, new int[] { (int)CodeListType.IsTrueType });
            Common.DropDownBind(dropList,true);
            if (Request.QueryString["search"] != null)
            {
                string[] searchStr = Request.QueryString["search"].ToString().Split('_');
                ddl_category.SelectedValue = searchStr[0];
                ddl_type.SelectedValue = searchStr[1];
                ddl_project.SelectedValue = searchStr[2];
                ddl_IsAppointment.SelectedValue = searchStr[3];
                ddl_IsComplete.SelectedValue = searchStr[4];
                txt_contractNum.Value = searchStr[5];
            }
            GridDataBind();
            //添加新合同权限
            this.btNew.Visible = Function.CheckButtonPermission("A010101");
            btSearch.Visible = Function.CheckButtonPermission("A010104");

        }
    }

    protected string GetSearchQueryStr()
    {
        return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", new object[] { ddl_category.SelectedItem.Value,ddl_type.SelectedItem.Value,ddl_project.SelectedItem.Value,
            ddl_IsAppointment.SelectedItem.Value,ddl_IsComplete.SelectedItem.Value,txt_contractNum.Value.Trim()});

    }
    private void GridDataBind()
    {
        string sql = @"select ROW_NUMBER() over(order by tb_contract.id) as rowNum,purchase_type,tb_contract.id,
                            pType.name as projectName,contract_num,contract_name,
                            contract_amount,contract_client,delivery_date,
                            cType.name as contractType,c.name as contractCategory,cdList.name as is_complete,codeList.name as is_appointment,signature_date,
                            first_date,first_amount,first_pay_date,first_pay_amount,
                            second_date,second_amount,second_pay_date,second_pay_amount,
                            third_date,third_amount,third_pay_date,third_pay_amount,
                            fourth_date,fourth_amount,fourth_pay_date,fourth_pay_amount 
                            from tb_contract
                            left join tb_code_list cType on (cType.id = tb_contract.purchase_type)
                            left join tb_code_list pType on (tb_contract.project_code = pType.id) 
                            left join tb_code_list c on (c.id = tb_contract.contract_type) 
                            left join tb_code_list codeList on(codeList.id = tb_contract.is_appointment)
                            left join tb_code_list cdList on (cdList.id = tb_contract.is_complete)  
                            where delFlag=0";
        if (ddl_category.SelectedItem.Value != "0")
        {
            sql += string.Format(" and tb_contract.contract_type = '{0}'", ddl_category.SelectedItem.Value);
        }
        if (ddl_type.SelectedItem.Value != "0")
        {
            sql += string.Format(" and tb_contract.purchase_type = '{0}'", ddl_type.SelectedItem.Value);
        }
        if (ddl_project.SelectedItem.Value != "0")
        {
            sql += string.Format(" and tb_contract.project_code= '{0}'", ddl_project.SelectedItem.Value);
        }
        if (ddl_IsAppointment.SelectedItem.Value != "0")
        {
            sql += " and tb_contract.is_appointment =" + ddl_IsAppointment.SelectedItem.Value;
        }
        if (ddl_IsComplete.SelectedItem.Value != "0")
        {
            sql += " and tb_contract.is_complete = " + ddl_IsComplete.SelectedItem.Value;
        }
        if (!string.IsNullOrEmpty(txt_contractNum.Value.Trim()))
        {
            string searchText = txt_contractNum.Value.Trim().Replace(",", "");
            sql += " and (tb_contract.contract_num like '%{0}%' ";
            sql += " or tb_contract.contract_name like '%{0}%'";
            sql += " or pType.name like '%{0}%'";
            sql += " or tb_contract.contract_client like '%{0}%')";
            sql = string.Format(sql, searchText);

        }
        sql += " order by signature_date DESC";
        DataTable tb = DBHelper.GetTableBySql(sql);
        //foreach (DataRow row in tb.Rows)
        //{
        //    int id = Convert.ToInt32(row["id"]);
        //    Common.UpdateContractStatus(id);
        //}
        this.GridView1.DataSource = tb;
        GridView1.DataKeyNames = new string[] { "id"};
        GridView1.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string url = "ContractDetail.aspx?contractId=" + (sender as LinkButton).CommandArgument;
        url += "&search=" + GetSearchQueryStr();
       //Response.Redirect("PurchaseDetail.aspx?contractId=" + (sender as LinkButton).CommandArgument);
        Server.Transfer(url);

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridDataBind();
    }
    protected void btSearch_ServerClick(object sender, EventArgs e)
    {
        GridDataBind();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        GridDataBind();
    }

    protected void ddl_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddl_category.SelectedItem.Value == "0")
        {
            Common.DropDownBind(ddl_type, new int[] { (int)CodeListType.PurchaseType, (int)CodeListType.ProjectType },true);
        }
        else if (this.ddl_category.SelectedItem.Value == ((int)CodeList.ContractType_Purchase).ToString())
        {
            Common.DropDownBind(ddl_type, (int)CodeListType.PurchaseType,true);
        }
        else if (this.ddl_category.SelectedItem.Value == ((int)CodeList.ContractType_Project).ToString())
        {
            Common.DropDownBind(ddl_type, (int)CodeListType.ProjectType,true);
        }

        GridDataBind();
    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataBind();
    }

    protected void ddl_project_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataBind();
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        DBAccess access = DBAccess.CreateInstance();
        //string delSql = @"delete from tb_contract where id =@id";
        string delSql = @"update tb_contract set delFlag=1,deleter=@user,delete_time=Now() where id =@id";
        using (DbConnection conn = access.GetConnection())
        {
            
            conn.Open();
            DbCommand cmd = access.CreateCommand(delSql, conn);
            cmd.Parameters.Add(access.GetParameter("@id", (sender as LinkButton).CommandArgument));
            cmd.Parameters.Add(access.GetParameter("@user", ((CMS.Model.User)HttpContext.Current.Session["User"]).UserName));

            access.ExecuteNonQuery(cmd);
        }
        GridDataBind();

    }

    protected void ddl_IsAppointment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataBind();
    }

    protected void ddl_IsComplete_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string url = "ContractDetail.aspx?contractId=" + GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            url += "&search=" + GetSearchQueryStr();
            e.Row.Attributes.Add("OnDblClick", "javascript:location.href='" + url + "';");
        }
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        string url = "ContractDetail.aspx?search=" + GetSearchQueryStr();
        Server.Transfer(url);
    }
}
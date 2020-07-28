using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DB;
using CMS.Model;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string sql = "select * from tb_users where name ='{0}' and password='{1}'";
        sql = string.Format(sql, this.txtUserName.Value, this.txtPass.Value);
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            HttpContext.Current.Session["User"] = Function.GetUser(tb.Rows[0]["name"].ToString(),Int32.Parse(tb.Rows[0]["id"].ToString()));
            Response.Redirect("ContractList.aspx");
        }
        else
        {
            ClientScript.RegisterStartupScript(GetType(), "", "alert('用户名或密码错误，请重新输入。')",true);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Model;

public partial class UserControl_Top : System.Web.UI.UserControl
{
    public string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["User"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }
        username = ((User)(HttpContext.Current.Session["User"])).UserName;

    }
}
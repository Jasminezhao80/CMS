using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Left : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["User"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }
        //合同管理明细
        CheckPermission(this.A0101, "A0101");
        //合同汇总统计
        CheckPermission(this.A0102, "A0102");
        //合同管理
        CheckPermission(this.A01, "A01");

        //采购管理
        CheckPermission(this.A02,"A02");
        CheckPermission(this.A0201, "A0201");
        CheckPermission(this.A0202, "A0202");
        //CheckPermission(this.A0202_new, "A0202");
        CheckPermission(this.A0203, "A0203");
        CheckPermission(this.A0204, "A0204");

        //基础信息管理
        CheckPermission(this.A03, "A03");
        CheckPermission(this.A0301, "A0301");
        CheckPermission(this.A0302, "A0302");
    }
    private void CheckPermission(System.Web.UI.HtmlControls.HtmlContainerControl obj, string code) {
        if (!Function.ChechMenuPermission(code))
        {
            obj.Style.Add("display", "none");
        }
    }
    //private void CheckPermission(System.Web.UI.HtmlControls.HtmlContainerControl obj, List<string> codes)
    //{
    //    foreach (string code in codes)
    //    {
    //        if (Function.CheckButtonPermission(code))
    //        {
    //            return;
    //        }
    //    }
    //    obj.Style.Add("display", "none");
    //}
}
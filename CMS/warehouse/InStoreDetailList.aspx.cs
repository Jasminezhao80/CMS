using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DB;
public partial class warehouse_InStoreDetailList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridDataBind();
        }
    }
    private void GridDataBind()
    {
        string sql = @"SELECT T2.product_name,T2.product_size,T2.product_material,T1.quantity,T1.instore_date,T3.order_num  
                        FROM tb_instore_detail T1
                        INNER JOIN tb_product T2 ON(T1.product_id = T2.id)
                        LEFT JOIN tb_purchase_order T3 on (T3.id = T1.purchase_order_id)";
        gridList.DataSource = DBHelper.GetTableBySql(sql);
        gridList.DataBind();
    }
}
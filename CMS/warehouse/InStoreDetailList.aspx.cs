using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DB;
using CMS.Bll;
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
        string sql = @"SELECT T1.id,T1.product_id, T2.product_name,T2.product_size,T2.product_material,T1.quantity,T1.instore_date,T3.order_num  
                        FROM tb_instore_detail T1
                        INNER JOIN tb_product T2 ON(T1.product_id = T2.id)
                        LEFT JOIN tb_purchase_order T3 on (T3.id = T1.purchase_order_id)";
        gridList.DataSource = DBHelper.GetTableBySql(sql);
        gridList.DataBind();
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        object[] arguments = ((LinkButton)sender).CommandArgument.Split(';');
        int id = Convert.ToInt32(arguments[0]);
        int productId = Convert.ToInt32(arguments[2]);
        int quantity = Convert.ToInt32(arguments[1]);
        InstoreBll bll = new InstoreBll();
        //判断此商品的库存量是否大于取消入库的数量，如果库存量大于取消入库的数量，则此商品未出库，可以取消入库。反之，则不能取消入库。
        int leftQuantity = bll.GetProductLeftQuantity(productId);
        if (leftQuantity >= quantity)
        {
            bll.Delete(id);
            GridDataBind();
        }
        else
        {
            Common.Alert("此入库单已经出库不能删除！",this);
        }

    }
}
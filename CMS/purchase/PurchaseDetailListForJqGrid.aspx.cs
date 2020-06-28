using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DB;
using Newtonsoft.Json;

public partial class purchase_PurchaseDetailListForJqGrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static string GetJsonString()
    {
        string sql = @"SELECT A.product_id,A.order_id,A.price,A.quantity,A.in_warehouse_date,A.id,B.order_num,D.name AS projectName,E.name AS category,B.contract_id,B.apply_date,A.delivery_date,
                C.product_name,C.product_size,C.product_material,F.name AS unit,A.unit_price,G.name AS supplier,A.leader,A.memo,A.supplier_id 
                FROM tb_purchase_orderdetail A
                LEFT JOIN tb_purchase_order B ON (A.order_id = B.id)
                LEFT JOIN tb_product C on (A.product_id = C.id)
                LEFT JOIN tb_code_list D ON (B.project_id = D.id)
                LEFT JOIN tb_code_list E ON (C.product_category_id = E.id)
                LEFT JOIN tb_code_list F ON(F.id = C.product_unit_id)
                LEFT JOIN tb_code_list G ON (G.id = A.supplier_id) where B.is_disabled <> 1  order by order_num DESC,A.id";
        DataTable tb = DBHelper.GetTableBySql(sql);
        string result = JsonConvert.SerializeObject(tb);
        return result;
    }

}
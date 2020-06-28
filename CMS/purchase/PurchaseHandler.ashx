<%@ WebHandler Language="C#" Class="PurchaseHandler" %>

using System;
using System.Web;
using CMS.DB;
using Newtonsoft.Json;
using System.Data;

public class PurchaseHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string sql = @"SELECT A.product_id,A.order_id,A.price,A.quantity,A.in_warehouse_date,A.id,B.order_num,D.name AS projectName,E.name AS category,B.contract_id,B.apply_date,A.delivery_date,
        C.product_name,C.product_size,C.product_material,F.name AS unit,A.unit_price,G.name AS supplier,A.leader,A.memo,A.supplier_id 
        FROM tb_purchase_orderdetail A
        LEFT JOIN tb_purchase_order B ON (A.order_id = B.id)
        LEFT JOIN tb_product C on (A.product_id = C.id)
        LEFT JOIN tb_code_list D ON (B.project_id = D.id)
        LEFT JOIN tb_code_list E ON (C.product_category_id = E.id)
        LEFT JOIN tb_code_list F ON(F.id = C.product_unit_id)
        LEFT JOIN tb_code_list G ON (G.id = A.supplier_id) where B.is_disabled <> 1   order by order_num DESC,A.id";
        DataTable tb = DBHelper.GetTableBySql(sql);
        string result = JsonConvert.SerializeObject(tb);

        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
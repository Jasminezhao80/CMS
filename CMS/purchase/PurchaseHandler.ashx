<%@ WebHandler Language="C#" Class="PurchaseHandler" %>

using System;
using System.Web;
using CMS.DB;
using Newtonsoft.Json;
using System.Data;
using System.Text;
public class PurchaseHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string function = context.Request["Function"];
        if (function == "Supplier")
        {
            string sqlStr = @"select id,name from tb_code_list where type={0} order by name";
            sqlStr = string.Format(sqlStr, (int)CodeListType.SupplierName);
            DataTable supplierTb = DBHelper.GetTableBySql(sqlStr);
            StringBuilder builder = new StringBuilder();

            if (supplierTb.Rows.Count > 0)
            {
                foreach (DataRow row in supplierTb.Rows)
                {
                    builder.Append("<option value=" + row["id"] + ">" + row["name"] + "</option>");
                }
            }
            context.Response.Write("<select>"+builder.ToString() + "</select>");
            return;
        }
        //string type = context.Request["cellType"];
        string sql = @"SELECT H.quantity as instore_quantity,A.product_id,A.order_id,A.price,A.quantity,A.in_warehouse_date,A.id,B.order_num,D.name AS projectName,E.name AS category,B.contract_id,B.apply_date,A.delivery_date,
        C.product_name,C.product_size,C.product_material,F.name AS unit,A.unit_price,G.name AS supplier,A.leader,A.memo,A.supplier_id 
        FROM tb_purchase_orderdetail A
        LEFT JOIN tb_purchase_order B ON (A.order_id = B.id)
        LEFT JOIN tb_product C on (A.product_id = C.id)
        LEFT JOIN tb_code_list D ON (B.project_id = D.id)
        LEFT JOIN tb_code_list E ON (C.product_category_id = E.id)
        LEFT JOIN tb_code_list F ON(F.id = C.product_unit_id)
        LEFT JOIN tb_code_list G ON (G.id = A.supplier_id) 
        Left join tb_instore_detail H on (A.id = H.purchase_order_detail_id)        
        where B.is_disabled <> 1 and C.instore_flag = 1  ";
        if (context.Request["pId"] != "0")
        {
            sql += " and B.project_id ='" + context.Request["pId"] + "'";
        }
        if (context.Request["pSupplier"] != "0")
        {
            sql += " and A.supplier_id ='" + context.Request["pSupplier"] + "'";
        }
        if (context.Request["pWarehouse"] == "1")
        {
            sql += " and A.in_warehouse_date is null";
        }
        if (context.Request["pWarehouse"] == "2")
        {
            sql += " and A.in_warehouse_date is not null";
        }
        if (!string.IsNullOrEmpty(context.Request["pSearch"]))
        {
            sql += @" and (B.order_num like '%{0}%' or B.contract_id like '%{0}%' or C.product_name like '%{0}%' or C.product_size like '%{0}%' or E.name like '%{0}%')";
            sql = string.Format(sql, context.Request["pSearch"].Trim().Replace(",", ""));
        }
        sql += " order by order_num DESC,A.id";
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
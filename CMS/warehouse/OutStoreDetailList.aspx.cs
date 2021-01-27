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

public partial class warehouse_OutStoreDetailList : System.Web.UI.Page
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
        string sql = @"select T1.*,T2.product_name,T2.product_size,T2.product_material,T3.name as typeName,
                    CONCAT(T4.contract_num,':',T4.contract_name) as contractName  
                    from tb_outstore_detail T1
                    LEFT JOIN tb_product T2 ON(T1.product_id = T2.id)
                    Left join tb_contract T4 on (T4.id = T1.contract_id)
                    Left join tb_const T3 on (T3.id = T1.type) 
                    ";
        gridList.DataSource = DBHelper.GetTableBySql(sql);
        gridList.DataBind();
    }
}
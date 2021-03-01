using CMS.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class warehouse_OutStoreManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.FillDropDown(this.ddl_outStoreType, (int)CodeListType.OutStoreType);
            GridDataBind();
        }
    }
    private void GridDataBind()
    {
        string sql = @"SELECT T1.*,(case when T2.outTotal IS NULL then 0 ELSE T2.outTotal END) AS outTotal,T3.product_name,T3.product_size,T3.product_material  
                    from
                    (SELECT product_id,SUM(quantity) AS inTotal FROM tb_instore_detail
                    GROUP BY product_id) T1
                    LEFT JOIN 
                    (SELECT product_id,SUM(quantity) AS outTotal FROM tb_outstore_detail
                    GROUP BY product_id) T2 ON (T1.product_id = T2.product_id)
                    inner JOIN tb_product T3 ON(T1.product_id = T3.id)
                    ";
        if (txt_search.Value.Trim() != "")
        {
            sql += " where T3.product_name like '%{0}%' or T3.product_size like '%{0}%' or T3.product_material like '%{0}%' ";
            sql = string.Format(sql, txt_search.Value.Trim());
        }
        gridList.DataSource = DBHelper.GetTableBySql(sql);
        gridList.DataBind();
    }


    //protected void ddl_outStoreType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //销售出库
    //    if (ddl_outStoreType.SelectedValue == CodeList.OutStore_Sale.ToString())
    //    {
    //        div_Borrow.Visible = false;
    //        div_Produce.Visible = false;
    //        div_Sale.Visible = true;
    //    }
    //    //生产领用
    //    if (ddl_outStoreType.SelectedValue == CodeList.OutStore_Produce.ToString())
    //    {
    //        div_Borrow.Visible = false;
    //        div_Produce.Visible = true;
    //        div_Sale.Visible = false;
    //    }
    //    //借用
    //    if (ddl_outStoreType.SelectedValue == CodeList.OutStore_Borrow.ToString())
    //    {
    //        div_Borrow.Visible = true;
    //        div_Produce.Visible = false;
    //        div_Sale.Visible = false;
    //    }
    //}

    protected void btn_submit_ServerClick(object sender, EventArgs e)
    {
        string sql = @"insert into tb_outstore_detail (product_id,quantity,type,outstore_person,outstore_date,outstore_project,contract_id)
                    values (@productId,@quantity,@type,@person,@date,@project,@contractId) ";
  
        DBAccess ac = DBAccess.CreateInstance();
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@productId",this.txt_productid.Value));
            cmd.Parameters.Add(ac.GetParameter("@quantity", this.txt_quantity.Value));
            cmd.Parameters.Add(ac.GetParameter("@type", ddl_outStoreType.SelectedValue));
            cmd.Parameters.Add(ac.GetParameter("@date", Common.ConvertToDate(txt_outStoreTime.Value)));
            cmd.Parameters.Add(ac.GetParameter("@person", txt_person.Value));
            //借用
            if (this.ddl_outStoreType.SelectedValue == ((int)CodeList.OutStore_Borrow).ToString())
            {
                cmd.Parameters.Add(ac.GetParameter("@project", txt_borrowProject.Value));
                cmd.Parameters.Add(ac.GetParameter("@contractId", DBNull.Value));
            }
            //生产领用
            if (this.ddl_outStoreType.SelectedValue == ((int)CodeList.OutStore_Produce).ToString())
            {
                cmd.Parameters.Add(ac.GetParameter("@project", txt_part.Value));
                cmd.Parameters.Add(ac.GetParameter("@contractId", DBNull.Value));
            }
            //销售
            if (this.ddl_outStoreType.SelectedValue == ((int)CodeList.OutStore_Sale).ToString())
            {

                cmd.Parameters.Add(ac.GetParameter("@project", DBNull.Value));
                cmd.Parameters.Add(ac.GetParameter("@contractId", txt_contractId.Value));
            }
            //
            ac.ExecuteNonQuery(cmd);
        }
        GridDataBind();
    }

    [WebMethod]
    public static void DoOutStore()
    {

    }
    [WebMethod]
    public static string GetName(string searchKey)
    {
        if (string.IsNullOrEmpty(searchKey))
        {
            return "";
        }
        string sql = @"select id,contract_num,contract_name from tb_contract where contract_type={0} and (contract_num like '%{1}%' or contract_name like '%{1}%')";
        sql = string.Format(sql, (int)CodeList.ContractType_Project, searchKey);
        DataTable tb = DBHelper.GetTableBySql(sql);
        string result = JsonConvert.SerializeObject(tb);
        return result;
    }

    protected void gridList_SelectedIndexChanged(object sender, EventArgs e)
    {

        this.txt_store.Value = gridList.SelectedRow.Cells[6].Text;//现有库存;
    }

    protected void btnOut_Click(object sender, EventArgs e)
    {
        this.txt_store.Value = gridList.SelectedRow.Cells[6].Text;//现有库存;
    }

    protected void btn_search_ServerClick(object sender, EventArgs e)
    {
        GridDataBind();
    }
}
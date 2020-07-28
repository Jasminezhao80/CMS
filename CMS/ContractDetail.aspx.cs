using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CMS.DB;
using System.Data.Common;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetSaveButtonPermission();

            FillPercentageDDL();
            if (Request.QueryString["search"] != null)
            {
                ViewState["search"] = Request.QueryString["search"].ToString();
            }
            //从报表页面传递过来的日期参数
            if (Request.QueryString["reportStatus"] != null)
            {
                ViewState["reportStatus"] = Request.QueryString["reportStatus"];
            }
            Dictionary<DropDownList, int[]> drList = new Dictionary<DropDownList, int[]>();
            drList.Add(ddl_category, new int[] { (int)CodeListType.ContractType });
            //drList.Add(ddl_type, new int[] { (int)CodeListType.PurchaseType });
            drList.Add(ddl_project, new int[] { (int)CodeListType.ProjectName });
            drList.Add(ddl_moneyTypy, new int[] { (int)CodeListType.MoneyType });
            drList.Add(ddl_isDelivery, new int[] { (int)CodeListType.IsTrueType });
            Common.DropDownBind(drList, false);
            string purchaseType = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["contractId"]))
            {
                int id = Int32.Parse(Request.QueryString["contractId"]);
                string sql = @"select tb_contract.id,contract_type,purchase_type,contract_name,contract_num,contract_amount,
                                        contract_client,project_code,finish_date,first_date,first_amount,
                                        second_date,second_amount,third_date,third_amount,fourth_date,fourth_amount,last_date,last_amount,
                                        tb_contract.memo,first_pay_date,first_pay_amount,
                                        second_pay_date,second_pay_amount,third_pay_date,third_pay_amount,fourth_pay_date,fourth_pay_amount,last_pay_date,
                                        last_pay_amount,leader,delivery_date,signature_date,money_type,is_delivery,
                                        codeList.name as is_appointment,cdList.name as is_complete 
                                        from tb_contract 
                                        left join tb_code_list codeList on(codeList.id = tb_contract.is_appointment)
                                        left join tb_code_list cdList on (cdList.id = tb_contract.is_complete)  
                                        where tb_contract.id =" + id.ToString();

                DataTable tb = DBHelper.GetTableBySql(sql);
                DataRow dr = tb.Rows[0];
                ddl_category.SelectedValue = dr["contract_type"].ToString();
                purchaseType = dr["purchase_type"].ToString();
                ddl_project.SelectedValue = dr["project_code"].ToString();

                contractName.Value = dr["contract_name"].ToString();
                contractNum.Value = dr["contract_num"].ToString();
                totalSum.Value = dr["contract_amount"].ToString();
                string statusStr = "履约:{0} 完成:{1} 完成时间:{2}";
                status.InnerText = String.Format(statusStr, dr["is_appointment"].ToString(), dr["is_complete"].ToString(), Common.ConvertToDate(dr["finish_date"]));
                firstDate.Value = Common.ConvertToDate(dr["first_date"]);
                secondDate.Value = Common.ConvertToDate(dr["second_date"]);
                thirdDate.Value = Common.ConvertToDate(dr["third_date"]);
                fourthDate.Value = Common.ConvertToDate(dr["fourth_date"]);
                lastDate.Value = Common.ConvertToDate(dr["last_date"]);
                deliveryDate.Value = Common.ConvertToDate(dr["delivery_date"]);

                firstPayDate.Value = Common.ConvertToDate(dr["first_pay_date"]);
                secondPayDate.Value = Common.ConvertToDate(dr["second_pay_date"]);
                thirdPayDate.Value = Common.ConvertToDate(dr["third_pay_date"]);
                fourthPayDate.Value = Common.ConvertToDate(dr["fourth_pay_date"]);
                lastPayDate.Value = Common.ConvertToDate(dr["last_pay_date"]);
                signatureDate.Value = Common.ConvertToDate(dr["signature_date"]);

                txt_firstPay.Value = dr["first_pay_amount"].ToString();
                txt_secondPay.Value = dr["second_pay_amount"].ToString();
                txt_thirdPay.Value = dr["third_pay_amount"].ToString();
                txt_fourthPay.Value = dr["fourth_pay_amount"].ToString();
                txt_lastPay.Value = dr["last_pay_amount"].ToString();

                firstAmount.Value = dr["first_amount"].ToString();
                secondAmount.Value = dr["second_amount"].ToString();
                thirdAmount.Value = dr["third_amount"].ToString();
                fourthAmount.Value = dr["fourth_amount"].ToString();
                lastAmount.Value = dr["last_amount"].ToString();

                txt_leader.Value = dr["leader"].ToString();
                contractClient.Value = dr["contract_client"].ToString();

                ddl_moneyTypy.SelectedValue = dr["money_type"].ToString();
                ddl_isDelivery.SelectedValue = dr["is_delivery"].ToString();
                txt_memo.Value = dr["memo"].ToString();
            }

            string contractType = ddl_category.SelectedValue;
            if (contractType == ((int)CodeList.ContractType_Purchase).ToString())
            {
                Common.DropDownBind(ddl_type, (int)CodeListType.PurchaseType, false);
            }
            if (contractType == ((int)CodeList.ContractType_Project).ToString())
            {
                Common.DropDownBind(ddl_type, (int)CodeListType.ProjectType, false);
            }
            if (!string.IsNullOrEmpty(purchaseType))
            {
                ddl_type.SelectedValue = purchaseType;

            }
        }
    }
    private void SetSaveButtonPermission()
    {
        string id = Request.QueryString["contractId"];
        //保存按钮权限
        if (string.IsNullOrEmpty(id))
        {
            //新增权限
            btnSave.Visible = Function.CheckButtonPermission("A010101");
        }
        else
        {
            //修改权限
            btnSave.Visible = Function.CheckButtonPermission("A010102");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string id = Request.QueryString["contractId"];
            string sqlStr = "";
            if (string.IsNullOrEmpty(id))
            {
                sqlStr = @"insert into tb_contract (contract_type,purchase_type,contract_name,contract_num,project_code,contract_amount,
                            first_date,first_amount,second_date,second_amount,third_date,third_amount,last_date,last_amount,memo,
                            contract_client,is_appointment,is_complete,first_pay_date,first_pay_amount,
                            second_pay_date,second_pay_amount,third_pay_date,third_pay_amount,last_pay_date,last_pay_amount,leader,
                            delivery_date,signature_date,fourth_date,fourth_amount,fourth_pay_date,fourth_pay_amount,money_type,is_delivery,create_time,creater)
                        values(@contractType,@type,@name,@num,@project,@amount,@firstDate,@firstAmount,@secondDate,@secondAmount,
                                @thirdDate,@thirdAmount,@lastDate,@lastAmount,@memo,@client,@appointment,@complete,
                            @firstPayDate,@firstPayAmount,@secondPayDate,@secondPayAmount,@thirdPayDate,@thirdPayAmount,
                            @lastPayDate,@lastPayAmount,@leader,@deliveryDate,@signatureDate,@fourthDate,@fourthAmount,@fourthPayDate,@fourthPayAmount,@moneyType,@isDelivery,Now(),@user)";

            }
            else
            {
                sqlStr = @"update tb_contract set contract_type=@contractType,purchase_type=@type,contract_name=@name,
                                contract_num=@num,project_code=@project,contract_amount=@amount,
                                first_date=@firstDate,first_amount=@firstAmount,
                                second_date=@secondDate,second_amount=@secondAmount,third_date=@thirdDate,
                                third_amount=@thirdAmount,last_date=@lastDate,last_amount=@lastAmount,memo=@memo,
                                contract_client=@client,
                                first_pay_date=@firstPayDate,first_pay_amount=@firstPayAmount,second_pay_date=@secondPayDate,
                                second_pay_amount=@secondPayAmount,third_pay_date=@thirdPayDate,third_pay_amount=@thirdPayAmount,
                                last_pay_date=@lastPayDate,last_pay_amount=@lastPayAmount,leader=@leader,delivery_date=@deliveryDate,
                                signature_date=@signatureDate,fourth_date=@fourthDate,fourth_amount=@fourthAmount,
                                fourth_pay_date=@fourthPayDate,fourth_pay_amount=@fourthPayAmount,money_type=@moneyType,is_delivery=@isDelivery,update_time=Now(),updater=@user     
                                where id=" + id;
            }
            DBAccess access = DBAccess.CreateInstance();
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(sqlStr, conn);
                cmd.Parameters.Add(access.GetParameter("@contractType", ddl_category.SelectedItem.Value));
                cmd.Parameters.Add(access.GetParameter("@type", ddl_type.SelectedItem.Value));
                cmd.Parameters.Add(access.GetParameter("@name", contractName.Value));
                cmd.Parameters.Add(access.GetParameter("@num", contractNum.Value));
                cmd.Parameters.Add(access.GetParameter("@project", ddl_project.SelectedItem.Value));
                cmd.Parameters.Add(access.GetParameter("@amount", totalSum.Value));

                cmd.Parameters.Add(access.GetParameter("@appointment", (int)CodeList.IsTrue_Y));
                cmd.Parameters.Add(access.GetParameter("@complete", (int)CodeList.IsTrue_N));
                cmd.Parameters.Add(access.GetParameter("@isDelivery", ddl_isDelivery.SelectedItem.Value));

                //cmd.Parameters.Add(access.GetParameter("@finishDate", ConvertToDBValue(finishDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@firstDate", ConvertToDBValue(firstDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@firstAmount", firstAmount.Value));
                cmd.Parameters.Add(access.GetParameter("@secondDate", ConvertToDBValue(secondDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@secondAmount", secondAmount.Value));
                cmd.Parameters.Add(access.GetParameter("@thirdDate", ConvertToDBValue(thirdDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@thirdAmount", thirdAmount.Value));
                cmd.Parameters.Add(access.GetParameter("@lastDate", ConvertToDBValue(lastDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@lastAmount", lastAmount.Value));
                cmd.Parameters.Add(access.GetParameter("@memo", txt_memo.Value));
                cmd.Parameters.Add(access.GetParameter("@client", contractClient.Value));

                cmd.Parameters.Add(access.GetParameter("@firstPayDate", ConvertToDBValue(firstPayDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@firstPayAmount", txt_firstPay.Value));
                cmd.Parameters.Add(access.GetParameter("@secondPayDate", ConvertToDBValue(secondPayDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@secondPayAmount", txt_secondPay.Value));
                cmd.Parameters.Add(access.GetParameter("@thirdPayDate", ConvertToDBValue(thirdPayDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@thirdPayAmount", txt_thirdPay.Value));
                cmd.Parameters.Add(access.GetParameter("@lastPayDate", ConvertToDBValue(lastPayDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@lastPayAmount", txt_lastPay.Value));
                cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value));
                cmd.Parameters.Add(access.GetParameter("@deliveryDate", ConvertToDBValue(deliveryDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@signatureDate", ConvertToDBValue(signatureDate.Value)));

                cmd.Parameters.Add(access.GetParameter("@fourthDate", ConvertToDBValue(fourthDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@fourthAmount", txt_fourthPay.Value));
                cmd.Parameters.Add(access.GetParameter("@fourthPayDate", ConvertToDBValue(fourthPayDate.Value)));
                cmd.Parameters.Add(access.GetParameter("@fourthPayAmount", txt_fourthPay.Value));
                cmd.Parameters.Add(access.GetParameter("@moneyType", ddl_moneyTypy.SelectedItem.Value));
                cmd.Parameters.Add(access.GetParameter("@user", ((CMS.Model.User)HttpContext.Current.Session["User"]).UserName));

                access.ExecuteNonQuery(cmd);

            }
            if (!string.IsNullOrEmpty(id))
            {
                Common.UpdateContractStatus(Convert.ToInt32(id));
            }

            BackTo();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddl_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddl_category.SelectedItem.Value == ((int)CodeList.ContractType_Purchase).ToString())
        {
            Common.DropDownBind(ddl_type, (int)CodeListType.PurchaseType,false);
        }
        else if (this.ddl_category.SelectedItem.Value == ((int)CodeList.ContractType_Project).ToString())
        {
            Common.DropDownBind(ddl_type, (int)CodeListType.ProjectType,false);
        }

    }
    private object ConvertToDBValue(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return DBNull.Value;
        }
        else
        {
            return str;
        }
    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedItem.Value == ((int)CodeList.PurchaseType_External).ToString())
        {
            ddl_moneyTypy.SelectedValue = ((int)CodeList.MoneyType_America).ToString();
        }
        else
        {
            ddl_moneyTypy.SelectedValue = ((int)CodeList.MoneyType_RMB).ToString();
        }
    }
    private void BackTo()
    {
        //Request.QueryString["type"]，识别跳转之前的页面id
        if (string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            Server.Transfer("~/ContractList.aspx?type='detail'&search=" + ViewState["search"]);
        }
        else
        {
            Response.Redirect("~/SummaryReport.aspx?reportStatus=" + ViewState["reportStatus"] + "&tabIndex="+Request.QueryString["tabIndex"]);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BackTo();
    }
    private void FillPercentageDDL()
    {
        string sql = @"select code, name from tb_keyvalue";
        DataTable tb = DBHelper.GetTableBySql(sql);
        BindDDL(ddl_firstAmount, tb);
        BindDDL(ddl_secondAmount, tb);
        BindDDL(ddl_thirdAmount, tb);
        BindDDL(ddl_fourthAmount, tb);
        BindDDL(ddl_lastAmount, tb);
    }
    private void BindDDL(DropDownList list,DataTable tb)
    {
        list.DataSource = tb;
        list.DataValueField = "code";
        list.DataTextField = "name";
        list.DataBind();
        list.Items.Insert(0, new ListItem("", "0"));
    }
}
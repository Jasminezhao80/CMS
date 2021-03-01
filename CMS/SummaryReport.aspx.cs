using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.DB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public partial class SummaryReport : System.Web.UI.Page
{
    private decimal grid1_contractCount = 0;
    private int grid1_newCount = 0;
    private int grid1_weekFinishCount = 0;
    private decimal grid1_finishCount = 0;
    private int grid1_delayCount = 0;

    //protected void Page_UnLoad(object sender, EventArgs e)
    //{
    //    Session["ActiveReport"] = txt_hidden.Text;
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //按钮权限
            this.btnSearch.Visible = Function.CheckButtonPermission("A010202");
            btnReport.Visible = Function.CheckButtonPermission("A010201");
            DateTime lastSaturday = DateTime.Now;
            DateTime now = DateTime.Now;
            int week = Convert.ToInt32(now.DayOfWeek);
            switch (week)
            {
                case 6:
                    lastSaturday = now.AddDays(-7);
                    break;
                case 7:
                    lastSaturday = now.AddDays(-8);
                    break;
                default:
                lastSaturday = now.AddDays(-8 - week);
                    break;
            }
            if (Request.QueryString["reportStatus"] != null)
            {
                //从明细页面返回
                string[] requestStr = Request.QueryString["reportStatus"].Split('_');
                weekFrom.Value = Common.ConvertToDate(requestStr[0]);
                weekTo.Value = Common.ConvertToDate(requestStr[1]);
                nextWeekFrom.Value = Common.ConvertToDate(requestStr[2]);
                nextWeekTo.Value = Common.ConvertToDate(requestStr[3]);
                BindData();
                SetDefaultActiveTab();
            }
            else
            {
                weekFrom.Value = Common.ConvertToDate(lastSaturday);
                weekTo.Value = Common.ConvertToDate(lastSaturday.AddDays(6));
                nextWeekFrom.Value = Common.ConvertToDate(lastSaturday.AddDays(7));
                nextWeekTo.Value = Common.ConvertToDate(lastSaturday.AddDays(13));
                BindData();
            }

            
        }
    }

    private void SetDefaultActiveTab()
    {
        int tabIndex = Convert.ToInt32(Request.QueryString["tabIndex"]);
        this.li_report1.Attributes.Remove("class");
        this.report1.Attributes["class"] = "tab-pane fade";
        HtmlGenericControl li = (HtmlGenericControl)tab_title.FindControl("li_report" + tabIndex);
        HtmlGenericControl div = (HtmlGenericControl)tab_content.FindControl("report" + tabIndex);
        li.Attributes.Add("class", "active");
        div.Attributes["class"] = "tab-pane fade in active";

    }
    private void BindData()
    {
        ViewState["reportStatus"] = string.Format("{0}_{1}_{2}_{3}", new string[] { weekFrom.Value,
        weekTo.Value,nextWeekFrom.Value,nextWeekTo.Value});

        Grid1Bind();
        GridBind_DelayReport((int)CodeList.ContractType_Project, GridView2);
        GridBind_DelayReport((int)CodeList.ContractType_Purchase, GridView3);
        GridBind_WarningReport((int)CodeList.ContractType_Project, GridView4);
        GridBind_WarningReport((int)CodeList.ContractType_Purchase, GridView5);
        Grid6Bind();
    }

    protected void TabClick()
    {
        //string s = "";
    }
    private void Grid1Bind()
    {
        string sql = @"select typeCode.name as type,count(*) as contractCount,count(case when tb_contract.is_complete={4} then 1 else null end) as completeCount, count(case when tb_contract.signature_date >= '{0}' and tb_contract.signature_date <= '{1}' then 1 else null end) as weekAdded,count(case when tb_contract.finish_date >= '{0}' and tb_contract.finish_date <= '{1}' then 1 else null end) as weekComplete,
                        count(case when (first_date < GETDATE() and first_pay_date is null) or (second_date < GETDATE() and second_pay_date is null) or (third_date < GETDATE() and third_pay_date is null) or (fourth_date < GETDATE() and fourth_pay_date is null) or (last_date < GETDATE() and last_pay_date is null) then 1 else null end) as delayCount 
                        from tb_contract
                        inner join tb_code_list typeCode on (typeCode.id=tb_contract.purchase_type)
                        where tb_contract.contract_type = {2} and tb_contract.delFlag=0 
                        group by typeCode.name
                        union all 
                        (select projectCode.name as type,count(*) as contractCount,count(case when tb_contract.is_complete={4} then 1 else null end) as completeCount,count(case when tb_contract.signature_date >= '{0}' and tb_contract.signature_date <= '{1}' then 1 else null end) as weekAdded,count(case when tb_contract.finish_date >= '{0}' and tb_contract.finish_date <= '{1}' then 1 else null end) as weekComplete,
                        count(case when (first_date < GETDATE() and first_pay_date is null) or (second_date < GETDATE() and second_pay_date is null) or (third_date < GETDATE() and third_pay_date is null) or (fourth_date < GETDATE() and fourth_pay_date is null) or (last_date < GETDATE() and last_pay_date is null) then 1 else null end) as delayCount                         
                        from tb_contract
                        inner join tb_code_list projectCode on (projectCode.id=tb_contract.project_code)
                        where tb_contract.contract_type = {3} and tb_contract.delFlag=0 
                        group by projectCode.name)";
        sql = string.Format(sql, new object[] { weekFrom.Value, weekTo.Value,(int)CodeList.ContractType_Purchase,(int)CodeList.ContractType_Project,(int)CodeList.IsTrue_Y});
        GridView1.DataSource = DBHelper.GetTableBySql(sql);
        GridView1.DataKeyNames = new string[] { "contractCount" };
        GridView1.DataBind();
    }
    private void GridBind_DelayReport(int type,GridView grid)
    {
        string sql = @"select tb_contract.memo,tb_contract.id,contract_num,cCode.name as purchaseType,pCode.name as projectName,contract_name,contract_client,first_date,first_pay_date,first_amount,second_date,second_pay_date,second_amount,third_date,
        third_pay_date,third_amount,fourth_date,fourth_pay_date,fourth_amount,last_date,last_pay_date,last_amount,'' as delayName,0 as delayAmount,GETDATE() as delayDate 
        from tb_contract
        left join tb_code_list pCode on (pCode.id = tb_contract.project_code)
        left join tb_code_list cCode on (cCode.id = tb_contract.purchase_type)
        where tb_contract.delFlag=0 and contract_type = '{0}' and ((first_date < GETDATE() and first_pay_date is null) or (second_date < GETDATE() and second_pay_date is null) or (third_date < GETDATE() and third_pay_date is null) or (fourth_date < GETDATE() and fourth_pay_date is null) or (last_date < GETDATE() and last_pay_date is null))";
        sql = string.Format(sql, type);
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            foreach (DataRow row in tb.Rows)
            {
                if (row["first_date"] != null && row["first_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["first_date"]);
                    if (date < DateTime.Now && row["first_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第一阶段延期";
                        row["delayAmount"] = row["first_amount"];
                        row["delayDate"] = row["first_date"];
                        continue;
                    }
                }
                if (row["second_date"] != null && row["second_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["second_date"]);
                    if (date < DateTime.Now && row["second_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第二阶段延期";
                        row["delayAmount"] = row["second_amount"];
                        row["delayDate"] = row["second_date"];
                        continue;
                    }
                }

                if (row["third_date"] != null && row["third_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["third_date"]);
                    if (date < DateTime.Now && row["third_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第三阶段延期";
                        row["delayAmount"] = row["third_amount"];
                        row["delayDate"] = row["third_date"];

                        continue;
                    }
                }
                if (row["fourth_date"] != null && row["fourth_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["fourth_date"]);
                    if (date < DateTime.Now && row["fourth_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第四阶段延期";
                        row["delayAmount"] = row["fourth_amount"];
                        row["delayDate"] = row["fourth_date"];

                        continue;
                    }
                }
                if (row["last_date"] != null && row["last_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["last_date"]);
                    if (date < DateTime.Now && row["last_pay_date"] is DBNull)
                    {
                        row["delayName"] = "质保金延期";
                        row["delayAmount"] = row["last_amount"];
                        row["delayDate"] = row["last_date"];
                        continue;
                    }
                }
            }
            grid.DataSource = tb;
            grid.DataBind();
        }
    }
    private void GridBind_WarningReport(int type, GridView grid)
    {
        string sql = @"select tb_contract.memo,tb_contract.id,contract_num,cCode.name as purchaseType,pCode.name as projectName,contract_name,contract_client,first_date,first_pay_date,first_amount,second_date,second_pay_date,second_amount,third_date,
        third_pay_date,third_amount,fourth_date,fourth_pay_date,fourth_amount,last_date,last_pay_date,last_amount,'' as delayName,0 as delayAmount,GETDATE() as delayDate 
        from tb_contract
        left join tb_code_list pCode on (pCode.id = tb_contract.project_code)
        left join tb_code_list cCode on (cCode.id = tb_contract.purchase_type)
        where contract_type = '{2}' and tb_contract.delFlag=0 and (((first_date >= '{0}' and first_date <= '{1}') and first_pay_date is null) or (second_date >= '{0}' and second_date <= '{1}' and second_pay_date is null) or (third_date >= '{0}' and third_date <= '{1}' and third_pay_date is null) or (fourth_date >= '{0}' and fourth_date <= '{1}' and fourth_pay_date is null) or (last_date >= '{0}' and last_date <= '{1}' and last_pay_date is null))";
        sql = string.Format(sql, nextWeekFrom.Value,nextWeekTo.Value, type);
        DataTable tb = DBHelper.GetTableBySql(sql);
        if (tb.Rows.Count > 0)
        {
            DateTime from = Convert.ToDateTime(nextWeekFrom.Value);
            DateTime to = Convert.ToDateTime(nextWeekTo.Value);

            foreach (DataRow row in tb.Rows)
            {
                if (row["first_date"] != null && row["first_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["first_date"]);
                    if (date >= from && date <= to && row["first_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第一阶段";
                        row["delayAmount"] = row["first_amount"];
                        row["delayDate"] = row["first_date"];
                        continue;
                    }
                }
                if (row["second_date"] != null && row["second_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["second_date"]);
                    if (date >= from && date <= to && row["second_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第二阶段";
                        row["delayAmount"] = row["second_amount"];
                        row["delayDate"] = row["second_date"];
                        continue;
                    }
                }

                if (row["third_date"] != null && row["third_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["third_date"]);
                    if (date >= from && date <= to && row["third_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第三阶段";
                        row["delayAmount"] = row["third_amount"];
                        row["delayDate"] = row["third_date"];

                        continue;
                    }
                }
                if (row["fourth_date"] != null && row["fourth_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["fourth_date"]);
                    if (date >= from && date <= to && row["fourth_pay_date"] is DBNull)
                    {
                        row["delayName"] = "第四阶段";
                        row["delayAmount"] = row["fourth_amount"];
                        row["delayDate"] = row["fourth_date"];

                        continue;
                    }
                }
                if (row["last_date"] != null && row["last_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(row["last_date"]);
                    if (date >= from && date <= to && row["last_pay_date"] is DBNull)
                    {
                        row["delayName"] = "质保金";
                        row["delayAmount"] = row["last_amount"];
                        row["delayDate"] = row["last_date"];
                        continue;
                    }
                }
            }
            grid.DataSource = tb;
            grid.DataBind();
        }
    }

    private void Grid6Bind()
    {
        string sql = @"SELECT tb_contract.memo,tb_contract.id,proCode.name as projectName,pcode.name AS purchaseType,contract_num,contract_name,contract_client,signature_date,contract_amount
                    FROM tb_contract 
                    LEFT JOIN tb_code_list pcode ON (tb_contract.purchase_type = pcode.id) 
                    LEFT JOIN tb_code_list proCode ON (tb_contract.project_code = proCode.id) 
                    WHERE tb_contract.delFlag=0 and signature_date >= '{0}' AND signature_date <= '{1}'";
        sql = string.Format(sql, weekFrom.Value, weekTo.Value);
        GridView6.DataSource = DBHelper.GetTableBySql(sql);
        GridView6.DataBind();
    }
    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnReport_ServerClick(object sender, EventArgs e)
    {
        try
        {
            HSSFWorkbook workbook = null;
            //FileStream fs = new FileStream("", FileMode.Open);
            string filePath = ConfigurationManager.AppSettings["TempleteFilePath"].ToString();
            filePath = Server.MapPath(filePath);
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new HSSFWorkbook(stream);
                stream.Close();
            }
            ISheet sheet = workbook.GetSheetAt(0);
            int startRow = 3;
            int rowIndex = startRow;

            #region Report1
            for (int i = 1; i <= GridView1.Rows.Count; i++)
            {
                GridViewRow gridRow = GridView1.Rows[i - 1];
                IRow excelRow = sheet.GetRow(startRow).CopyRowTo(startRow + i);

                excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
                excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim());
                excelRow.GetCell(2).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim()));
                excelRow.GetCell(3).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim()));
                excelRow.GetCell(4).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[4].Controls[0]).Text.Trim()));
                excelRow.GetCell(5).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[5].Controls[0]).Text.Trim()));
                //excelRow.GetCell(6).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[6].Controls[0]).Text.Trim());
                excelRow.GetCell(7).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[7].Controls[0]).Text.Trim()));
                excelRow.GetCell(6).SetCellFormula("F" + (startRow + i + 1) + "/C" + (startRow + i + 1));
            }
            rowIndex = GridView1.Rows.Count + 5;//report1 sum row
            IRow sumRow = sheet.GetRow(rowIndex);
            sumRow.Cells[2].SetCellFormula(sumRow.Cells[2].CellFormula);
            sumRow.Cells[3].SetCellFormula(sumRow.Cells[3].CellFormula);
            sumRow.Cells[4].SetCellFormula(sumRow.Cells[4].CellFormula);
            sumRow.Cells[5].SetCellFormula(sumRow.Cells[5].CellFormula);
            sumRow.Cells[6].SetCellFormula(sumRow.Cells[6].CellFormula);
            //延期合同数 合计
            sumRow.Cells[7].SetCellFormula(sumRow.Cells[7].CellFormula);

            sheet.ShiftRows(startRow + 1, 1000, -1);
            #endregion

            #region report 2
            rowIndex += 7;//report2 start row
            GenerateDelayReport(rowIndex, sheet, GridView2);
            GenerateDelayReport(GridView2.Rows.Count + rowIndex + 7, sheet, GridView3);
            #endregion

            #region Report3 预警合同
            // 第一行数据的开始行
            rowIndex = rowIndex + GridView2.Rows.Count + 7 + GridView3.Rows.Count + 8;
            GenerateWarningReport(rowIndex, sheet, GridView4);//预警收款合同
            //预警付款合同
            rowIndex = rowIndex + GridView4.Rows.Count + 7;
            if (GridView5.Rows.Count > 0)
            {
                GenerateWarningReport(rowIndex, sheet, GridView5);//预警付款合同
            }
            #endregion
            #region 签订的新合同

            if (GridView6.Rows.Count > 0)
            {
                rowIndex = rowIndex + GridView5.Rows.Count + 6;
                GenerateNewSignReport(rowIndex, sheet, GridView6);
            }
            #endregion

            string newFile = "c:\\Report\\ContractReport.xls";
            using (FileStream stream2 = File.Open(newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(stream2);
                stream2.Close();
            }
            System.IO.FileInfo file = new System.IO.FileInfo(newFile);
            Response.Clear();
            //下载后的文件名
            Response.AddHeader("Content-Disposition",
                "attachment;filename=ContractReport.xls");
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Response.WriteFile(newFile);
            //if (File.Exists(newFile))
            //{
            //    File.Delete(newFile);
            //}
            Response.End();
    
        }
        catch (Exception ex)
        {
            throw ex;
        }
 
        //打开文件
        //System.Diagnostics.Process.Start(newFile);
    }

    private void GenerateNewSignReport(int tempRowIndex, ISheet sheet, GridView gridView)
    {
        for (int i = 1; i <= gridView.Rows.Count; i++)
        {
            GridViewRow gridRow = gridView.Rows[i - 1];
            IRow excelRow = sheet.GetRow(tempRowIndex).CopyRowTo(tempRowIndex + i);
            //序号
            excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
            //合同类型
            excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim());
            //合同编号(隐藏列)
            excelRow.GetCell(2).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim());
            //合同内容
            excelRow.GetCell(3).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[5].Controls[0]).Text.Trim());
            //合同双方
            excelRow.GetCell(4).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[6].Controls[0]).Text.Trim());
            // 签订日期
            excelRow.GetCell(5).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[7].Controls[0]).Text.Trim());
            //金额
            string amount = ((DataBoundLiteralControl)gridRow.Cells[8].Controls[0]).Text.Trim();
            excelRow.GetCell(6).SetCellValue(Convert.ToDouble(amount));
            excelRow.GetCell(7).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[9].Controls[0]).Text.Trim());
        }
        sheet.ShiftRows(tempRowIndex + 1, 1000, -1);
        IRow sumRow = sheet.GetRow(gridView.Rows.Count + tempRowIndex);
        sumRow.Cells[6].SetCellFormula(string.Format("SUM(G{0}:G{1})", tempRowIndex + 1, gridView.Rows.Count + tempRowIndex));

    }

    private void GenerateWarningReport(int tempRowIndex, ISheet sheet, GridView gridView)
    {
        //总数
        sheet.GetRow(tempRowIndex - 2).GetCell(1).SetCellValue(gridView.Rows.Count);
        for (int i = 1; i <= gridView.Rows.Count; i++)
        {
            GridViewRow gridRow = gridView.Rows[i - 1];
            IRow excelRow = sheet.GetRow(tempRowIndex).CopyRowTo(tempRowIndex + i);
            //序号
            excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
            //所属项目
            excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim());
            //合同编号(隐藏列)
            excelRow.GetCell(2).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim());
            //合同内容
            excelRow.GetCell(3).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[5].Controls[0]).Text.Trim());
            //合同双方
            excelRow.GetCell(4).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[6].Controls[0]).Text.Trim());
            // 延期内容
            string warningName = string.Format("{0}\r\n({1})", ((DataBoundLiteralControl)gridRow.Cells[7].Controls[0]).Text.Trim(), ((DataBoundLiteralControl)gridRow.Cells[8].Controls[0]).Text.Trim());
            excelRow.GetCell(5).SetCellValue(warningName);
            excelRow.GetCell(7).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[9].Controls[0]).Text.Trim()));
        }
        sheet.ShiftRows(tempRowIndex + 1, 1000, -1);
        IRow sumRow = sheet.GetRow(gridView.Rows.Count + tempRowIndex);
        sumRow.Cells[7].SetCellFormula(string.Format("SUM(H{0}:H{1})", tempRowIndex + 1, gridView.Rows.Count + tempRowIndex));

    }
    private void GenerateDelayReport(int tempRowIndex, ISheet sheet,GridView gridView)
    {
        sheet.GetRow(tempRowIndex - 2).GetCell(1).SetCellValue(gridView.Rows.Count);
        for (int i = 1; i <= gridView.Rows.Count; i++)
        {
            GridViewRow gridRow = gridView.Rows[i - 1];
            IRow excelRow = sheet.GetRow(tempRowIndex).CopyRowTo(tempRowIndex + i);
            excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
            excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim());
            //合同编号(隐藏列)
            excelRow.GetCell(2).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim());
            excelRow.GetCell(3).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[4].Controls[0]).Text.Trim());
            excelRow.GetCell(4).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[5].Controls[0]).Text.Trim());
            //DateTime delayDate = ((DataBoundLiteralControl)grid2Row.Cells[6].Controls[0]).Text.Trim();
            // 延期内容
            string delayName = string.Format("{0}({1})", ((DataBoundLiteralControl)gridRow.Cells[6].Controls[0]).Text.Trim(), ((DataBoundLiteralControl)gridRow.Cells[7].Controls[0]).Text.Trim());
            excelRow.GetCell(5).SetCellValue(delayName);
            excelRow.GetCell(7).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[8].Controls[0]).Text.Trim()));
        }
        //"SUM(H17:H17)"
        sheet.ShiftRows(tempRowIndex + 1, 1000, -1);
        IRow sumRow = sheet.GetRow(gridView.Rows.Count + tempRowIndex);
        sumRow.Cells[7].SetCellFormula(string.Format("SUM(H{0}:H{1})", tempRowIndex + 1, gridView.Rows.Count + tempRowIndex));

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "合计";
            e.Row.Cells[2].Text = grid1_contractCount.ToString();
            e.Row.Cells[3].Text = grid1_newCount.ToString();
            e.Row.Cells[4].Text = grid1_weekFinishCount.ToString();
            e.Row.Cells[5].Text = grid1_finishCount.ToString();
            e.Row.Cells[7].Text = grid1_delayCount.ToString();
            decimal d = grid1_finishCount / grid1_contractCount;
            e.Row.Cells[6].Text = Convert.ToInt32(d * 100) + "%";
            return;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            grid1_contractCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[2].Controls[0]).Text.Trim());
            grid1_newCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[3].Controls[0]).Text.Trim());
            grid1_weekFinishCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[4].Controls[0]).Text.Trim());
            grid1_finishCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[5].Controls[0]).Text.Trim());
            grid1_delayCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[7].Controls[0]).Text.Trim());
            //e.Row.Attributes.Add("OnDblClick", "javascript:location.href='PurchaseDetail.aspx?contractId=" + GridView1.DataKeys[e.Row.RowIndex].Value.ToString() + "';");
        }

    }

    protected void txt_hidden_TextChanged(object sender, EventArgs e)
    {
        Session["ActiveReport"] = txt_hidden.Text;
    }

    protected void txt_hidden_Unload(object sender, EventArgs e)
    {
        Session["ActiveReport"] = txt_hidden.Text;

    }
}
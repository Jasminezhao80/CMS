using CMS.DB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class purchase_PurchaseOrderReport : System.Web.UI.Page
{
    private int oldNotFinish = 0;
    private int finishedOldOrder = 0;
    private int monthNewOrder = 0;
    private int monthFinishOrder = 0;

    private int report2_totalCount = 0;
    private int report2_finishedCount = 0;

    private int report3_OrderCount = 0;
    private int report3_DetailCount = 0;
    private int report3_FinishCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            DateTime now = DateTime.Now;
            dateFrom.Value = now.AddMonths(-1).ToString("yyyy-MM-dd");
            dateTo.Value = now.ToString("yyyy-MM-dd");
            BindGrid1();
            BindGrid2();
            BindGrid3();
        }
    }

    private void BindGrid3()
    {
        string sql = @"SELECT B.apply_date,COUNT(DISTINCT B.order_num) AS orderCount,COUNT(A.id) detailCount,COUNT(case when A.in_warehouse_date >='{0}' AND A.in_warehouse_date <='{1}' then 1 ELSE NULL END) finishCount
        FROM tb_purchase_orderdetail A
        INNER JOIN tb_purchase_order B ON (A.order_id = B.id)
        WHERE B.is_disabled <>1 AND B.apply_date >='{0}' AND B.apply_date <= '{1}'
        GROUP BY B.apply_date";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, dateFrom.Value, dateTo.Value));
        Dictionary<int, WeekRepor> dic = new Dictionary<int, WeekRepor>();
        GregorianCalendar gc = new GregorianCalendar();
        int fromWeek = gc.GetWeekOfYear(Convert.ToDateTime(dateFrom.Value), CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
        int endWeek = gc.GetWeekOfYear(Convert.ToDateTime(dateTo.Value), CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
        List<WeekRepor> dataSource = new List<WeekRepor>();
        for (int i = fromWeek;i<=endWeek; i++)
        {
            WeekRepor r = new WeekRepor();
            r.Week = i;
            r.OrderCount = 0;
            r.DetailCount = 0;
            r.FinishCount = 0;
            dic.Add(i, r);
            dataSource.Add(r);
        }
        foreach (DataRow row in tb.Rows)
        {
            string applyDate = Common.ConvertToDate(row["apply_date"]);
            int weekOfYear = gc.GetWeekOfYear(Convert.ToDateTime(row["apply_date"]), CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
            if (dic.ContainsKey(weekOfYear))
            {
                WeekRepor report = dic[weekOfYear];
                report.OrderCount += Convert.ToInt32(row["orderCount"]);
                report.DetailCount += Convert.ToInt32(row["detailCount"]);
                report.FinishCount += Convert.ToInt32(row["finishCount"]);
            }
        }
        grid3.DataSource = dataSource;
        grid3.DataBind();
    }
    private void BindGrid1()
    {
        string sql = @"SELECT B.leader,COUNT(case when B.apply_date <= '{0}' and A.in_warehouse_date IS NULL then 1 ELSE NULL end) oldNotFinish,
        COUNT(case when B.apply_date <= '{0}' AND A.in_warehouse_date>='{0}' AND A.in_warehouse_date <='{1}' then 1 ELSE NULL end) finishedOldOrder,
        COUNT(case when B.apply_date >='{0}' AND B.apply_date <= '{1}' then 1 ELSE NULL end) monthNewOrder,
        COUNT(case when B.apply_date >='{0}' AND B.apply_date <= '{1}' AND A.in_warehouse_date>='{0}' AND A.in_warehouse_date <='{1}' then 1 ELSE NULL end) monthFinishOrder
        FROM tb_purchase_orderdetail A
        INNER JOIN tb_purchase_order B ON (A.order_id = B.id)
        WHERE B.is_disabled <> 1
        GROUP BY B.leader";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql,dateFrom.Value,dateTo.Value));
        grid1.DataSource = tb;
        grid1.DataBind();
    }

    private void BindGrid2()
    {
        string sql = @"SELECT C.name AS projectName,COUNT(*) AS totalCount,COUNT(case when A.in_warehouse_date >='{0}' AND A.in_warehouse_date <= '{1}' then 1 ELSE NULL END) finishedCount
            FROM tb_purchase_orderdetail A
            INNER JOIN tb_purchase_order B ON (A.order_id = B.id)
            LEFT JOIN tb_code_list C ON(B.project_id = C.id)
            WHERE B.is_disabled <>1 AND B.apply_date >='{0}' AND B.apply_date <= '{1}'
            GROUP BY C.name";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, dateFrom.Value, dateTo.Value));
        grid2.DataSource = tb;
        grid2.DataBind();
    }
    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        BindGrid1();
        BindGrid2();
        BindGrid3();
        string tabIndex = txt_tabIndex.Value;
        this.li_report1.Attributes.Remove("class");
        this.li_report2.Attributes.Remove("class");
        this.li_report3.Attributes.Remove("class");
        HtmlGenericControl li = (HtmlGenericControl)tab_title.FindControl("li_report" + tabIndex);
        li.Attributes.Add("class", "active");

        report1.Attributes["class"] = "tab-pane fade";
        report2.Attributes["class"] = "tab-pane fade";
        report3.Attributes["class"] = "tab-pane fade";

        HtmlGenericControl div = (HtmlGenericControl)tab_content.FindControl("report" + tabIndex);
        div.Attributes["class"] = "tab-pane fade in active";
    }

    protected void btnReport_ServerClick(object sender, EventArgs e)
    {
        try
        {
            XSSFWorkbook workbook = null;
            string filePath = ConfigurationManager.AppSettings["OrderSummaryTempleteFilePath"].ToString();
            filePath = Server.MapPath(filePath);
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new XSSFWorkbook(stream);
                stream.Close();
            }

            ISheet sheet = workbook.GetSheetAt(0);
            int startRow = 3;//第一个数据行
            int rowIndex = startRow;

            //sheet.GetRow(1).Cells[0].SetCellValue("月份汇总统计");
            #region 周汇总
            for (int i = 1; i <= grid3.Rows.Count; i++)
            {
                GridViewRow gridRow = grid3.Rows[i - 1];
                IRow excelRow = sheet.GetRow(startRow).CopyRowTo(startRow + i);

                excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
                excelRow.GetCell(1).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim()));
                excelRow.GetCell(2).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim()));
                excelRow.GetCell(3).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim()));
                //完成率
                excelRow.GetCell(4).SetCellFormula(string.Format("D{0}/C{0}",startRow + i+1));
            }
            sheet.ShiftRows(startRow + 1, 1000, -1);
            int sumRowIndex = startRow + grid3.Rows.Count;
            IRow sumRow = sheet.GetRow(sumRowIndex);
            sumRow.Cells[1].SetCellFormula(string.Format("SUM(B{0}:B{1})",startRow+1, sumRowIndex));
            sumRow.Cells[2].SetCellFormula(string.Format("SUM(C{0}:C{1})", startRow+1, sumRowIndex));
            sumRow.Cells[3].SetCellFormula(string.Format("SUM(D{0}:D{1})", startRow+1, sumRowIndex));
            sumRow.Cells[4].SetCellFormula(string.Format("D{0}/C{0}",sumRowIndex+1));
            #endregion

            #region 人员汇总
            startRow = sumRowIndex + 4;//模板行
            for (int i = 1; i <= grid1.Rows.Count; i++)
            {
                GridViewRow gridRow = grid1.Rows[i - 1];
                IRow excelRow = sheet.GetRow(startRow).CopyRowTo(startRow + i);
                //序号
                excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
                //姓名
                excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim());
                //累计剩余项
                excelRow.GetCell(2).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim()));
                //完成项
                excelRow.GetCell(3).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim()));
                //完成率
                excelRow.GetCell(4).SetCellFormula(string.Format("D{0}/C{0}", startRow + i + 1));
                //本月新增项
                excelRow.GetCell(5).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[5].Controls[0]).Text.Trim()));
                //本月完成
                excelRow.GetCell(6).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[6].Controls[0]).Text.Trim()));
                //月度完成率
                excelRow.GetCell(7).SetCellFormula(string.Format("G{0}/F{0}", startRow + i + 1));
                //总完成率
                excelRow.GetCell(8).SetCellFormula(string.Format("(D{0}+G{0})/(C{0}+F{0})", startRow + i + 1));
            }
            sheet.ShiftRows(startRow + 1, 1000, -1);
            sumRowIndex = startRow + grid1.Rows.Count;
            sumRow = sheet.GetRow(sumRowIndex);
            //累计剩余项
            sumRow.GetCell(2).SetCellFormula(string.Format("SUM(C{0}:C{1})", startRow + 1,sumRowIndex));
            //完成项
            sumRow.GetCell(3).SetCellFormula(string.Format("SUM(D{0}:D{1})", startRow + 1, sumRowIndex));
            //完成率
            sumRow.GetCell(4).SetCellFormula(string.Format("D{0}/C{0}", sumRowIndex + 1));
            //本月新增项
            sumRow.GetCell(5).SetCellFormula(string.Format("SUM(F{0}:F{1})", startRow + 1, sumRowIndex));
            //本月完成
            sumRow.GetCell(6).SetCellFormula(string.Format("SUM(G{0}:G{1})", startRow + 1, sumRowIndex));
            //月度完成率
            sumRow.GetCell(7).SetCellFormula(string.Format("G{0}/F{0}", sumRowIndex + 1));
            //总完成率
            sumRow.GetCell(8).SetCellFormula(string.Format("(D{0}+G{0})/(C{0}+F{0})", sumRowIndex + 1));
            #endregion

            #region 项目汇总
            startRow = sumRowIndex + 4;//模板行
            for (int i = 1; i <= grid2.Rows.Count; i++)
            {
                GridViewRow gridRow = grid2.Rows[i - 1];
                IRow excelRow = sheet.GetRow(startRow).CopyRowTo(startRow + i);

                excelRow.GetCell(0).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[0].Controls[0]).Text.Trim()));
                excelRow.GetCell(1).SetCellValue(((DataBoundLiteralControl)gridRow.Cells[1].Controls[0]).Text.Trim());
                excelRow.GetCell(2).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[2].Controls[0]).Text.Trim()));
                excelRow.GetCell(3).SetCellValue(Int32.Parse(((DataBoundLiteralControl)gridRow.Cells[3].Controls[0]).Text.Trim()));
                //完成率
                excelRow.GetCell(4).SetCellFormula(string.Format("D{0}/C{0}", startRow + i + 1));
            }
            sheet.ShiftRows(startRow + 1, 1000, -1);
            sumRowIndex = startRow + grid2.Rows.Count;
            sumRow = sheet.GetRow(sumRowIndex);
            sumRow.Cells[2].SetCellFormula(string.Format("SUM(C{0}:C{1})", startRow + 1, sumRowIndex));
            sumRow.Cells[3].SetCellFormula(string.Format("SUM(D{0}:D{1})", startRow + 1, sumRowIndex));
            sumRow.Cells[4].SetCellFormula(string.Format("D{0}/C{0}", sumRowIndex + 1));
            #endregion
            string newFile = "c:\\Report\\OrderSummaryReport.xlsx";
            using (FileStream stream2 = File.Open(newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(stream2);
                stream2.Close();
            }
            System.IO.FileInfo file = new System.IO.FileInfo(newFile);
            Response.Clear();
            //下载后的文件名
            Response.AddHeader("Content-Disposition",
                "attachment;filename=OrderSummaryReport.xlsx");
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

    }

    protected void grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计";
            e.Row.Cells[1].Text = report3_OrderCount.ToString();
            e.Row.Cells[2].Text = report3_DetailCount.ToString();
            e.Row.Cells[3].Text = report3_FinishCount.ToString();
            e.Row.Cells[4].Text = Common.PercentageFormat(report3_FinishCount, report3_DetailCount);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            report3_OrderCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[1].Controls[0]).Text.Trim());
            report3_DetailCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[2].Controls[0]).Text.Trim());
            report3_FinishCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[3].Controls[0]).Text.Trim());
        }
    }

    protected void grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "合计";
            e.Row.Cells[2].Text = report2_totalCount.ToString();
            e.Row.Cells[3].Text = report2_finishedCount.ToString();
            e.Row.Cells[4].Text = Common.PercentageFormat(report2_finishedCount,report2_totalCount);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            report2_totalCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[2].Controls[0]).Text.Trim());
            report2_finishedCount += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[3].Controls[0]).Text.Trim()); 
        }
    }
    protected void grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "合计";
            e.Row.Cells[2].Text = oldNotFinish.ToString();
            e.Row.Cells[3].Text = finishedOldOrder.ToString();
            e.Row.Cells[4].Text = Common.PercentageFormat(finishedOldOrder, oldNotFinish);
            e.Row.Cells[5].Text = monthNewOrder.ToString();
            e.Row.Cells[6].Text = monthFinishOrder.ToString();
            e.Row.Cells[7].Text = Common.PercentageFormat(monthFinishOrder, monthNewOrder);
            e.Row.Cells[8].Text = Common.PercentageFormat(finishedOldOrder + monthFinishOrder, oldNotFinish + monthNewOrder);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DataRowView dr = e.Row.DataItem as DataRowView;
            oldNotFinish += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[2].Controls[0]).Text.Trim());
            finishedOldOrder += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[3].Controls[0]).Text.Trim());
            monthNewOrder += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[5].Controls[0]).Text.Trim());
            monthFinishOrder += Convert.ToInt32(((DataBoundLiteralControl)e.Row.Cells[6].Controls[0]).Text.Trim());
        }
    }
}
public class WeekRepor
{
    public WeekRepor()
    { }
    private int week;
    private int orderCount;
    private int detailCount;
    private int finishCount;

    public int Week { get => week; set => week = value; }
    public int OrderCount { get => orderCount; set => orderCount = value; }
    public int DetailCount { get => detailCount; set => detailCount = value; }
    public int FinishCount { get => finishCount; set => finishCount = value; }
}
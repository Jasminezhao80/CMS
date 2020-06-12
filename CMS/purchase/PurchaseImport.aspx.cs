using CMS.DB;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using NPOI.SS.Util;
using CMS.Model;

public partial class purchase_PurchaseImport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.DropDownBind(ddl_project, (int)CodeListType.ProjectName, "");
        }
    }

    protected void btnImport_ServerClick(object sender, EventArgs e)
    {
        HttpPostedFile  files = Request.Files["fileUpload"];
        string filePath = GetExcel();
        DataTable tb = ExcelToTable(filePath);
        ImportTableToDB(tb, "tb_import_temp");

        grid.DataSource = tb;
        grid.DataBind();

    }

    private void ImportTableToDB(DataTable tb,string tbName)
    {
        DBAccess ac = DBAccess.CreateInstance();
        ac.BulkCopy(tb, tbName);
    }
    private string GetExcel()
    {
        string fileUrl = string.Empty;
        try
        {
            //全名
            
            string excelFile = this.fileUpload.PostedFile.FileName;
            string extention = Path.GetExtension(excelFile);
            string fileName = Path.GetFileNameWithoutExtension(excelFile);
            if (string.IsNullOrEmpty(fileName))
            {
                Response.Write("<script>Alert('请选择excel文件！');</script>");
                return null;
            }
            if (extention == ".xls" || extention == ".xlsx")
            {
                //浏览器安全性限制 无法直接获取客户端文件的真是路径，需要将文件上传到服务器，然后再从服务器获取文件源路径
                //设置上传路径，将文件保存到服务器
                string newFileName = fileName + DateTime.Now.Date.ToString("yyyyMMdd") +  extention;
                fileUrl = "c:\\Report\\" + newFileName;
                this.fileUpload.PostedFile.SaveAs(fileUrl);
                return fileUrl;
            }
            else
            {
                Response.Write("<script>alert('请选择excel文件！');</script>");
                return null;
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable ExcelToTable(string file)
    {
        DataTable tb = DBHelper.GetTableBySql("select * from tb_import_temp where 1= 2");
        IWorkbook workBook;
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            if (Path.GetExtension(file) == ".xls")
            {
                workBook = new HSSFWorkbook(fs);
            }
            else if (Path.GetExtension(file) == ".xlsx")
            {
                workBook = new XSSFWorkbook(fs);
            }
            else
            {
                workBook = null;
            }
            ISheet sheet = workBook.GetSheetAt(0);
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow row = tb.NewRow();
                // 共16列
                for (int j = 0; j < 16; j++)
                {
                    try
                    {
                        ICell cell = sheet.GetRow(i).GetCell(j);
                        if (j == 8)
                        {
                            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                            {
                                row[j] = cell.DateCellValue.ToString("yyyy-MM-dd");
                            }
                        }
                        else
                        {
                            row[j] = cell;
                        }
                    }
                    catch (Exception e)
                    {
                        row[j] = string.Empty;
                    }
                }
                tb.Rows.Add(row);
            }
            return tb;
        }
    }

    private Dictionary<string, int> GetCodeList(int type)
    {
        Dictionary<string, int> list = new Dictionary<string, int>();
        DataTable tb = DBHelper.GetTableBySql(string.Format("select name,id from tb_code_list where type= {0}",type));
        foreach (DataRow row in tb.Rows)
        {
            string name = row["name"].ToString();
            if (!list.ContainsKey(name))
            {
                list.Add(name, Convert.ToInt32(row["id"]));
            }
        }
        return list;

    }
    private int AddNewCode(string name,int type)
    {
        DBAccess ac = DBAccess.CreateInstance();
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            string sql = "insert into tb_code_list (name,type) value (@name,@type);select @@IDENTITY";
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@name", name));
            cmd.Parameters.Add(ac.GetParameter("@type", type));
            return Convert.ToInt32(ac.ExecuteScalar(cmd));
        }
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        Dictionary<string, int> categoryList = GetCodeList((int)CodeListType.ProductCategery);
        Dictionary<string, int> unitList = GetCodeList((int)CodeListType.ProductUnit);
        Dictionary<string, int> supplierList = GetCodeList((int)CodeListType.SupplierName);

        List<PurchaseDetail> detailList = new List<PurchaseDetail>();
        foreach (GridViewRow row in grid.Rows)
        {
            PurchaseDetail detail = new PurchaseDetail();
            string quantity = ((TextBox)row.FindControl("txt_col7")).Text.Trim();
            detail.Quantity = Convert.ToInt32(string.IsNullOrEmpty(quantity) ? "0" : quantity);
            detail.DeliveryDate = ((TextBox)row.FindControl("txt_col9")).Text.Trim();
            string unitPrice = ((TextBox)row.FindControl("txt_col10")).Text.Trim();
            detail.UnitPrice = Convert.ToDecimal(string.IsNullOrEmpty(unitPrice) ? "0": unitPrice);
            detail.Memo = ((TextBox)row.FindControl("txt_col11")).Text.Trim();

            Product product = new Product();
            detail.Product = product;

            product.Name = ((TextBox)row.FindControl("txt_col2")).Text.Trim();
            string category = ((TextBox)row.FindControl("txt_col3")).Text.Trim();
            product.Size = ((TextBox)row.FindControl("txt_col4")).Text.Trim();
            product.Material = ((TextBox)row.FindControl("txt_col5")).Text.Trim();
            string unit = ((TextBox)row.FindControl("txt_col6")).Text.Trim();
            string supplier = ((TextBox)row.FindControl("txt_col8")).Text.Trim();

            string sql = "select id from tb_product where product_name=@name and product_size=@size and product_material =@material";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", product.Name);
            parameters.Add("@size", product.Size);
            parameters.Add("@material", product.Material);
            DataTable tb = DBHelper.GetTableBySql(sql, parameters);
            //商品已经存在
            if (tb.Rows.Count > 0)
            {
                product.Id = Convert.ToInt32(tb.Rows[0][0]);
            }
            else//商品不存在，需要添加新商品到数据库
            {
                //category
                if (categoryList.ContainsKey(category))
                {
                    product.CategoryId = categoryList[category];
                }
                else
                {
                    product.CategoryId = AddNewCode(category, (int)CodeListType.ProductCategery);
                    categoryList.Add(category, product.CategoryId);
                }
                //unit
                if (unitList.ContainsKey(unit))
                {
                    product.UnitId = unitList[unit];
                }
                else
                {
                    product.UnitId = AddNewCode(unit, (int)CodeListType.ProductUnit);
                    unitList.Add(unit, product.UnitId);
                }
            }

            //供应商
            if (supplierList.ContainsKey(supplier))
            {
                detail.SupplierId = supplierList[supplier];
            }
            else
            {
                detail.SupplierId = AddNewCode(supplier, (int)CodeListType.SupplierName);
                supplierList.Add(supplier, detail.SupplierId);
            }

            detailList.Add(detail);
        }
        InsertOrder(detailList);
    }

    private void SetOrderParameters(DbCommand cmd, DBAccess access)
    {
        cmd.Parameters.Add(access.GetParameter("@num", txt_orderNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@name", ""));
        cmd.Parameters.Add(access.GetParameter("@contractId", txt_contractNum.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@projectId", ddl_project.SelectedValue));
        cmd.Parameters.Add(access.GetParameter("@applyDate", Common.ConvertToDBValue(txt_applyDate.Value)));
        cmd.Parameters.Add(access.GetParameter("@leader", txt_leader.Value.Trim()));
        cmd.Parameters.Add(access.GetParameter("@memo", txt_memo.Value.Trim()));
    }

    private void InsertOrderDetail(DBAccess access, DbTransaction transaction, int orderId, PurchaseDetail detail)
    {
        string sql = @"insert into tb_purchase_orderDetail(order_id,product_id,
                            supplier_id,delivery_date,unit_price,quantity,price,memo)
                                values(@orderId,@productId,@supplierId,@deliveryDate,@unitPrice,
                                @quantity,@price,@memo)";
        DbCommand cmd = access.CreateCommand(sql, transaction.Connection);
        cmd.Transaction = transaction;
        cmd.Parameters.Add(access.GetParameter("@orderId", orderId));
        cmd.Parameters.Add(access.GetParameter("@productId", detail.Product.Id));
        cmd.Parameters.Add(access.GetParameter("@supplierId", detail.SupplierId));
        cmd.Parameters.Add(access.GetParameter("@deliveryDate", Common.ConvertToDBValue(detail.DeliveryDate)));
        cmd.Parameters.Add(access.GetParameter("@unitPrice", detail.UnitPrice));
        cmd.Parameters.Add(access.GetParameter("@quantity", detail.Quantity));
        cmd.Parameters.Add(access.GetParameter("@price", detail.UnitPrice * detail.Quantity));
        cmd.Parameters.Add(access.GetParameter("@memo", detail.Memo));
        access.ExecuteNonQuery(cmd);
    }

    private void InsertOrder(List<PurchaseDetail> list)
    {
        string sql = @"insert into tb_purchase_order (order_num,order_name,contract_id,project_id,
                        apply_date,memo,leader) values (@num,@name,@contractId,@projectId,@applyDate,@memo,@leader);select @@IDENTITY";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();
            try
            {
                DbCommand cmd = access.CreateCommand(sql, conn);
                cmd.Transaction = transaction;
                SetOrderParameters(cmd, access);
                int orderId = Convert.ToInt32(access.ExecuteScalar(cmd));
                foreach (PurchaseDetail detail in list)
                {
                    if (detail.Product.Id == 0)
                    {
                        // TODO: insert product
                        detail.Product.Id = AddNewProduct(access, conn, transaction, detail.Product);
                    }
                    InsertOrderDetail(access, transaction, orderId, detail);
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            Response.Redirect("../purchase/PurchaseList.aspx");
        }
    }

    public int AddNewProduct(DBAccess access,DbConnection conn, DbTransaction transaction,Product product)
    {
        string sql = @"insert into tb_product(product_num,product_name,product_size,product_category_id,product_unit_id,product_material)
            values(@num,@name,@size,@category,@unit,@material);select @@IDENTITY";
        DbCommand cmd = access.CreateCommand(sql, conn);
        cmd.Transaction = transaction;
        cmd.Parameters.Add(access.GetParameter("@num", ""));
        cmd.Parameters.Add(access.GetParameter("@name", product.Name));
        cmd.Parameters.Add(access.GetParameter("@size", product.Size));
        cmd.Parameters.Add(access.GetParameter("@category", product.CategoryId));
        cmd.Parameters.Add(access.GetParameter("@material", product.Material));
        cmd.Parameters.Add(access.GetParameter("@unit", product.UnitId));
        return Convert.ToInt32(access.ExecuteScalar(cmd));
    }
}

using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InfoManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid((int)CodeListType.ProjectName, grid_project);
            BindGrid((int)CodeListType.ProjectType, grid_type);
            BindGrid((int)CodeListType.ProductCategery, grid_orderType);
            BindGrid((int)CodeListType.SupplierName, grid_supplier);
            BindGrid((int)CodeListType.ProductUnit, grid_productUnit);
        }
        RefreshTab();
    }
    private void RefreshTab()
    {
        li_project.Attributes.Remove("class");
        li_type.Attributes.Remove("class");
        li_supplier.Attributes.Remove("class");
        li_unit.Attributes.Remove("class");
        li_orderType.Attributes.Remove("class");

        div_project.Attributes["class"] = "tab-pane fade";
        div_supplier.Attributes["class"] = "tab-pane fade";
        div_type.Attributes["class"] = "tab-pane fade";
        div_productUnit.Attributes["class"] = "tab-pane fade";
        div_orderType.Attributes["class"] = "tab-pane fade";
        switch (ViewState["Active"])
        {
            case 1:
                li_project.Attributes.Add("class", "active");
                div_project.Attributes["class"] = "tab-pane fade in active";
                break;
            case 2:
                li_type.Attributes.Add("class","active");
                div_type.Attributes["class"] = "tab-pane fade in active";
                break;
            case 3:
                li_supplier.Attributes.Add("class", "active");
                div_supplier.Attributes["class"] = "tab-pane fade in active";
                break;
            case 4:
                li_orderType.Attributes.Add("class", "active");
                div_orderType.Attributes["class"] = "tab-pane fade in active";
                break;
            case 5:
                li_unit.Attributes.Add("class", "active");
                div_productUnit.Attributes["class"] = "tab-pane fade in active";
                break;
            default:
                li_project.Attributes.Add("class", "active");
                div_project.Attributes["class"] = "tab-pane fade in active";
                break;
        }
    }
    private void BindGrid(int type,GridView grid)
    {
        string sql = @"select id,name from tb_code_list where type=" + type + " order by name";
        grid.DataSource = DBHelper.GetTableBySql(sql);
        grid.DataKeyNames = new string[] { "id"};
        grid.DataBind();
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ViewState["Active"] = 1;

        AddNew((int)CodeListType.ProjectName,txt_newProjectName.Value.Trim(),grid_project);
    }
    private void AddNew(int codeType,string name,GridView grid)
    {
        string insSql = @"insert into tb_code_list (name,type) values (@name,@type)";
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(insSql, conn);
            cmd.Parameters.Add(access.GetParameter("@name", name));
            cmd.Parameters.Add(access.GetParameter("@type", codeType));
            access.ExecuteNonQuery(cmd);
        }
        BindGrid(codeType, grid);
        RefreshTab();
    }
    protected void btnAddType_Click(object sender, EventArgs e)
    {
        ViewState["Active"] = 2;

        AddNew((int)CodeListType.ProjectType,txt_newProjectType.Value.Trim(), grid_type);
    }

   protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        int type = 0;
        GridView grid = (GridView)sender;
        grid.EditIndex = -1;
        switch (grid.ID)
        {
            case "grid_project":
                type = (int)CodeListType.ProjectName;
                break;
            case "grid_type":
                type = (int)CodeListType.ProjectType;
                break;
            case "grid_orderType":
                type = (int)CodeListType.ProductCategery;
                break;
            case "grid_supplier":
                type = (int)CodeListType.SupplierName;
                break;

        }
        BindGrid(type, grid);
    }
 
    protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int type = 0;
        GridView grid = (GridView)sender;
        switch (grid.ID)
        {
            case "grid_project":
                type = (int)CodeListType.ProjectName;
                ViewState["Active"] = 1;
                break;
            case "grid_type":
                type = (int)CodeListType.ProjectType;
                ViewState["Active"] = 2;
                break;
            case "grid_orderType":
                type = (int)CodeListType.ProductCategery;
                ViewState["Active"] = 4;
                break;
            case "grid_supplier":
                type = (int)CodeListType.SupplierName;
                ViewState["Active"] = 3;
                break;
            case "grid_productUnit":
                type = (int)CodeListType.ProductUnit;
                ViewState["Active"] = 5;
                break;

        }
        
        grid.EditIndex = e.NewEditIndex;
        BindGrid(type, grid);
        RefreshTab();
    }

    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int type = 0;
        string editValue = string.Empty;
        GridView grid = (GridView)sender;
        switch (grid.ID)
        {
            case "grid_project":
                type = (int)CodeListType.ProjectName;
                editValue = "txt_projectName";
                break;
            case "grid_type":
                type = (int)CodeListType.ProjectType;
                editValue = "txt_projectType";
                break;
            case "grid_orderType":
                type = (int)CodeListType.ProductCategery;
                editValue = "txt_orderType";
                break;
            case "grid_supplier":
                type = (int)CodeListType.SupplierName;
                editValue = "txt_supplier";
                break;
            case "grid_productUnit":
                type = (int)CodeListType.ProductUnit;
                editValue = "txt_unit";
                break;

        }

        Update(grid, e.RowIndex, editValue);
        BindGrid(type, grid);
    }

    protected void btnAddSupplier_Click(object sender, EventArgs e)
    {
        ViewState["Active"] = 3;
        AddNew((int)CodeListType.SupplierName, txt_newSupplier.Value.Trim(), grid_supplier);
        txt_newSupplier.Value = "";
    }
    


    private void Update(GridView grid, int rowIndex, string txtName)
    {
        string upSql = @"update tb_code_list set name = @name
                        where id = @id";
        string name = ((TextBox)grid.Rows[rowIndex].FindControl(txtName)).Text.Trim();
        int id = Int32.Parse(grid.DataKeys[rowIndex].Value.ToString());
        DBAccess access = DBAccess.CreateInstance();
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(upSql, conn);
            cmd.Parameters.Add(access.GetParameter("@name", name));
            cmd.Parameters.Add(access.GetParameter("@id", id));
            access.ExecuteNonQuery(cmd);
        }
        grid.EditIndex = -1;

    }




    protected void btnAddNewOrderType_Click(object sender, EventArgs e)
    {
        ViewState["Active"] = 4;

        AddNew((int)CodeListType.ProductCategery, txt_newOrderType.Value.Trim(), grid_orderType);
    }

    protected void btnAddUnit_Click(object sender, EventArgs e)
    {
        ViewState["Active"] = 5;

        AddNew((int)CodeListType.ProductUnit, txt_newUnit.Value.Trim(), grid_productUnit);
    }
}
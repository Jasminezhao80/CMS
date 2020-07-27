using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_UserManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    private void BindGrid()
    {
        DataTable tb = DBHelper.GetTableBySql("select * from tb_users");
        userGrid.DataSource = tb;
        userGrid.DataBind();
    }

    protected void userGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //txtUserId.Value = userGrid.DataKeys[e.Row.RowIndex].Value.ToString();
            //e.Row.Attributes.Add("OnClick","LoadPermission("+e.Row.RowIndex+");");
            e.Row.Attributes.Add("OnClick", "LoadPermission(" + userGrid.DataKeys[e.Row.RowIndex].Value + "," + e.Row.RowIndex +");");
        }
    }
    [WebMethod]
    public static string SaveUser(int id,string userName,string password,string memo)
    {
        if (id > 0)
        {
            //修改
            string sql = "select * from tb_users where name ='{0}' and id != {1} ";
            DataTable tb = DBHelper.GetTableBySql(string.Format(sql, userName, id));
            if (tb.Rows.Count > 0)
            {
                return "false";
            }
            sql = "update tb_users set name=@name,password=@pass,memo=@memo where id=@id";
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@id", id));
                cmd.Parameters.Add(ac.GetParameter("@name", userName));
                cmd.Parameters.Add(ac.GetParameter("@pass", password));
                cmd.Parameters.Add(ac.GetParameter("@memo", memo));
                ac.ExecuteNonQuery(cmd);
            }

        }
        else
        {
            //新增
            string sql = "select * from tb_users where name ='{0}'";
            DataTable tb = DBHelper.GetTableBySql(string.Format(sql, userName));
            if (tb.Rows.Count > 0)
            {
                return "false";
            }
            sql = "insert into tb_users(name,password,memo) values(@name,@password,@memo)";
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@name", userName));
                cmd.Parameters.Add(ac.GetParameter("@password", password));
                cmd.Parameters.Add(ac.GetParameter("@memo", memo));
                ac.ExecuteNonQuery(cmd);

            }
        }
        return "true";
    }
    private bool CheckUserName(string userName)
    {
        string sql = "select * from tb_users where name ='{0}'";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, userName));
        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        if (!CheckUserName(this.txtUser.Value))
        {
            ClientScript.RegisterStartupScript(GetType(), "", "alert('用户名已经存在！')");
            return;
        }
        if (string.IsNullOrEmpty(this.txtUserId.Value))
        {
            string sql = "insert into tb_users(name,password,memo) values(@name,@password,@memo)";
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@name", txtUser.Value.Trim()));
                cmd.Parameters.Add(ac.GetParameter("@password", txtPass.Value.Trim()));
                cmd.Parameters.Add(ac.GetParameter("@memo", txtMemo.Value.Trim()));
                ac.ExecuteNonQuery(cmd);
               
            }
        }
        else
        {
            string sql = "update tb_users set name=@name,password=@pass,memo=@memo where id=@id";
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@id", this.txtUserId.Value));
                cmd.Parameters.Add(ac.GetParameter("@pass", this.txtPass.Value));
                cmd.Parameters.Add(ac.GetParameter("@name", this.txtUser.Value));
                cmd.Parameters.Add(ac.GetParameter("@memo", this.txtMemo.Value));
                ac.ExecuteNonQuery(cmd);
            }
        }
        BindGrid();
    }
    [WebMethod]
    public static List<string> GetUser(int id)
    {
        string sql = "select * from tb_users where id=" + id;
        DataTable tb = DBHelper.GetTableBySql(sql);
        List<string> list = new List<string>();
        if (tb.Rows.Count > 0)
        {
            list.Add(tb.Rows[0]["name"].ToString());
            list.Add(tb.Rows[0]["password"].ToString());
            list.Add(tb.Rows[0]["memo"].ToString());
        }
        return list;
    }
   [WebMethod]
    public static List<string> GetPermission(int userId)
    {
        string sql = "select permission_code from tb_user_permission  where user_id = {0}";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, userId));
        List<string> permissions = tb.AsEnumerable().Select(d => d.Field<string>("permission_code")).ToList();
        return permissions;
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        string sql = "delete from tb_users where id=@id";
        DBAccess ac = DBAccess.CreateInstance();
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbCommand cmd = ac.CreateCommand(sql, conn);
            cmd.Parameters.Add(ac.GetParameter("@id", (sender as LinkButton).CommandArgument));
            ac.ExecuteNonQuery(cmd);
        }
        BindGrid();
    }
    [WebMethod]
    public static void SavePermission(string codeList, int userId)
    {
        List<string> list = codeList.Split(',').ToList();
        if (list.Count == 0) return;

        string delSql = string.Format("delete from tb_user_permission where user_id = {0}",userId);
        DBAccess ac = DBAccess.CreateInstance();
        using (DbConnection conn = ac.GetConnection())
        {
            conn.Open();
            DbTransaction tran = conn.BeginTransaction();
            try {
                DbCommand cmd = ac.CreateCommand(delSql, conn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                foreach (string code in list)
                {
                    DbCommand insCmd = ac.CreateCommand(string.Format("insert into tb_user_permission (user_id,permission_code) values ({0},'{1}')",userId,code), conn);
                    insCmd.Transaction = tran;
                    insCmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
            catch (Exception ex) {
                tran.Rollback();
             }
        }
    }
}
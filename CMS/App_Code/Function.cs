using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CMS.DB;
using CMS.Model;

/// <summary>
/// Function 的摘要说明
/// </summary>
public class Function
{
    public Function()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //

    }
    public static bool ChechMenuPermission(string code)
    {
        if (HttpContext.Current.Session["User"] == null)
        {
            return false;
        }
        else
        {
            User user = (User)HttpContext.Current.Session["User"];
            if (user.PermissionCode == null || user.PermissionCode.Count == 0)
            {
                return false;
            }
            else
            {

                foreach (string c in user.PermissionCode)
                {
                    if (c.StartsWith(code))
                    {
                        return true;
                    }
                }
                return false;

            }
        }
    }
    public static bool CheckButtonPermission(string code)
    {
        if (HttpContext.Current.Session["User"] == null)
        {
            return false;
        }
        else
        {
            User user = (User)HttpContext.Current.Session["User"];
            if (user.PermissionCode == null || user.PermissionCode.Count == 0)
            {
                return false;
            }
            else
            {
                return user.PermissionCode.Contains(code);
            }
        }
    }
    public static User GetUser(string userName, int userId)
    {
        User user = new User();
        user.UserId = userId;
        user.UserName = userName;

        string sql = "select permission_code from tb_user_permission where user_id={0}";
        DataTable tb = DBHelper.GetTableBySql(string.Format(sql, userId));
        if (tb.Rows.Count > 0)
        {
            List<string> list = new List<string>();
            foreach (DataRow row in tb.Rows)
            {
                list.Add(row["permission_code"].ToString());
            }
            user.PermissionCode = list;
        }
        return user;
    }
}
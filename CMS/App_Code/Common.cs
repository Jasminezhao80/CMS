using CMS.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Common 的摘要说明
/// </summary>
public class Common
{
    public Common()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public static string ConvertToDate(object obj)
    {
        if (obj is null || obj is DBNull || obj.ToString() == string.Empty)
        {
            return string.Empty;
        }
        return Convert.ToDateTime(obj).ToString("yyyy-MM-dd");
    }
    public static void DropDownBind(Dictionary<DropDownList, int[]> dropList, bool isAddAll)
    {
        string sql = @"select id,name,type from tb_code_list order by name";
        DataTable tb = DBHelper.GetTableBySql(sql);
        List<KeyValue> list = new List<KeyValue>();
        foreach (DataRow row in tb.Rows)
        {
            KeyValue item = new KeyValue();
            item.Key = Convert.ToInt32(row["id"]);
            item.Value = row["name"].ToString();
            item.Type = Convert.ToInt32(row["type"]);
            list.Add(item);
        }
        foreach (var item in dropList)
        {
            DropDownList dropDownList = item.Key;
            var source = from s in list
                         where item.Value.Contains(s.Type)
                         select s;
            dropDownList.DataSource = source;
            dropDownList.DataValueField = "Key";
            dropDownList.DataTextField = "Value";
            dropDownList.DataBind();
            if (isAddAll)
            {
                dropDownList.Items.Insert(0, new ListItem("全部", "0"));
                dropDownList.SelectedIndex = 0;
            }
        }
    }

    public static void DropDownBind(DropDownList dropDownList, int type, string defaultValue)
    {
        string sql = @"select id,name from tb_code_list where type={0} order by name";
        sql = string.Format(sql, type);
        DataTable tb = DBHelper.GetTableBySql(sql);
        dropDownList.DataSource = tb;
        dropDownList.DataValueField = "id";
        dropDownList.DataTextField = "name";
        dropDownList.DataBind();

        dropDownList.Items.Insert(0, new ListItem(defaultValue, "0"));
        dropDownList.SelectedIndex = 0;

    }
    public static void DropDownBind(DropDownList dropDownList, int[] types, bool isAddAll)
    {
        Dictionary<DropDownList, int[]> list = new Dictionary<DropDownList, int[]>();
        list.Add(dropDownList, types);
        DropDownBind(list, isAddAll);
    }
    public static void DropDownBind(DropDownList dropDownList, int type, bool isAddAll)
    {
        DropDownBind(dropDownList, new int[] { type }, isAddAll);
    }
    public static void UpdateContractStatus(int contractId)
    {
        string selSql = "select * from tb_contract where id = " + contractId;
        DataTable tb = DBHelper.GetTableBySql(selSql);
        if (tb.Rows.Count == 0)
        {
            return;
        }
        DataRow row = tb.Rows[0];
        DBAccess access = DBAccess.CreateInstance();
        string upSql = string.Empty;
        if ((row["first_date"] != null && row["first_date"] != DBNull.Value && Convert.ToDateTime(row["first_date"]) < DateTime.Now && row["first_pay_date"] is DBNull)
            || (row["second_date"] != null && row["second_date"] != DBNull.Value && Convert.ToDateTime(row["second_date"]) < DateTime.Now && row["second_pay_date"] is DBNull)
            || (row["third_date"] != null && row["third_date"] != DBNull.Value && Convert.ToDateTime(row["third_date"]) < DateTime.Now && row["third_pay_date"] is DBNull)
            || (row["fourth_date"] != null && row["fourth_date"] != DBNull.Value && Convert.ToDateTime(row["fourth_date"]) < DateTime.Now && row["fourth_pay_date"] is DBNull)
            || (row["last_date"] != null && row["last_date"] != DBNull.Value && Convert.ToDateTime(row["last_date"]) < DateTime.Now && row["last_pay_date"] is DBNull))
        {
            upSql = "update tb_contract set is_appointment = '{0}',is_complete = '{0}',finish_date = null where id = " + contractId;
            upSql = string.Format(upSql, ((int)CodeList.IsTrue_N).ToString());
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(upSql, conn);
                access.ExecuteNonQuery(cmd);
            }
            return;
        }
        else
        {
            upSql = "update tb_contract set is_appointment = '{0}' where id = " + contractId;
            upSql = string.Format(upSql, ((int)CodeList.IsTrue_Y).ToString());
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(upSql, conn);
                access.ExecuteNonQuery(cmd);
            }
        }
        if ((row["first_date"] != DBNull.Value && row["first_pay_date"] == DBNull.Value)
            || (row["second_date"] != DBNull.Value && row["second_pay_date"] == DBNull.Value)
            || (row["third_date"] != DBNull.Value && row["third_pay_date"] == DBNull.Value)
            || (row["fourth_date"] != DBNull.Value && row["fourth_pay_date"] == DBNull.Value)
            || (row["last_date"] != DBNull.Value && row["last_pay_date"] == DBNull.Value)
            || Convert.ToInt32(row["is_delivery"]) == (int)CodeList.IsTrue_Y && row["delivery_date"] == DBNull.Value)
        {
            upSql = "update tb_contract set is_complete = '{0}',finish_date = null where id = " + contractId;
            upSql = string.Format(upSql, ((int)CodeList.IsTrue_N).ToString());
        }
        else
        {
            upSql = @"UPDATE tb_contract SET is_complete = {0},finish_date=
                        (SELECT MAX(a.dt)
                        FROM ((SELECT first_pay_date AS dt FROM tb_contract WHERE id={1}) UNION (SELECT second_pay_date AS dt FROM tb_contract WHERE id ={1}) UNION (SELECT third_pay_date AS dt FROM tb_contract WHERE id ={1}) UNION (SELECT fourth_pay_date AS dt FROM tb_contract WHERE id ={1}) UNION (SELECT last_pay_date AS dt FROM tb_contract WHERE id ={1}) UNION (SELECT delivery_date AS dt FROM tb_contract WHERE id ={1})) a
                        ) WHERE id = {1}";
            upSql = string.Format(upSql, ((int)CodeList.IsTrue_Y).ToString(), contractId);
        }
        using (DbConnection conn = access.GetConnection())
        {
            conn.Open();
            DbCommand cmd = access.CreateCommand(upSql, conn);
            access.ExecuteNonQuery(cmd);
        }
    }
    public static object ConvertToDBValue(string str)
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
    public static string PercentageFormat(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            return "0%";
        }
        else
        {
            double result = Convert.ToDouble(numerator) / Convert.ToDouble(denominator)*100;
            return result.ToString("F0") + "%";
        }
    }
}

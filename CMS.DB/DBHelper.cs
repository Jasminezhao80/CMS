using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using System.Data.Common;

namespace CMS.DB
{
    public class DBHelper
    {
        //public static DataTable GetTableBySql(string selSql)
        //{
        //    DataTable tb = new DataTable();
        //    string sqlStr = ConfigurationManager.ConnectionStrings["DBStr"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(sqlStr))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(selSql,conn);
        //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //        ad.Fill(tb);
        //    }
        //    return tb;
        //}

        public static DataTable GetTableBySql(string sql)
        {
            DataTable tb = new DataTable();
            DBAccess access = DBAccess.CreateInstance();
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(sql, conn);
                tb = access.ExecuteDataTable(cmd);
            }
            return tb;
        }

        public static DataTable GetTableBySql(string sql,string parameter,string value)
        {
            DataTable tb = new DataTable();
            DBAccess access = DBAccess.CreateInstance();
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(sql, conn);

                cmd.Parameters.Add(access.GetParameter(parameter, value));

                tb = access.ExecuteDataTable(cmd);
            }
            return tb;
        }

        public static DataTable GetTableBySql(string sql, Dictionary<string,object> parameters)
        {
            DataTable tb = new DataTable();
            DBAccess access = DBAccess.CreateInstance();
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(sql, conn);
                foreach (KeyValuePair<string,object> item in parameters)
                {
                    cmd.Parameters.Add(access.GetParameter(item.Key, item.Value));
                }
                tb = access.ExecuteDataTable(cmd);
            }
            return tb;
        }

        public static void UpdateContractStatus()
        {

        }
    }

}

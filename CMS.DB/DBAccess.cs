using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB
{
    public abstract class DBAccess
    {
        private const string DefaultConnectionStringName = "DBStr";
        private string connectionString;
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        public static DBAccess CreateInstance()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[DefaultConnectionStringName];
            string conStr = settings.ConnectionString;
            string provider = settings.ProviderName;
            DBAccess access = CreateInstance(provider);
            access.connectionString = conStr;
            return access;
        }
        private static DBAccess CreateInstance(string provider)
        {
            DBAccess access = null;
            switch (provider)
            {
                case "MySql.Data.MySqlClient":
                    access = new MySqlAccess();
                    break;
                case "System.Data.SqlClient":
                    access = new SqlAccess();
                    break;
                
            }
            return access;
        }

        public virtual DbConnection GetConnection()
        {
            return this.GetConnection(this.connectionString);
        }
        public abstract DbConnection GetConnection(string connectionString);
        public abstract DbDataAdapter GetDataAdapter();

        public virtual DbCommand CreateCommand(string sql,DbConnection conn)
        {
            DbCommand cmd = CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            return cmd;
        }
        public abstract DbCommand CreateCommand();

        public abstract void BulkCopy(DataTable source,string destination);
        public virtual DataTable ExecuteDataTable(DbCommand cmd)
        {
            DbDataAdapter ad = GetDataAdapter();
            ad.SelectCommand = cmd;
            CleanParameterSyntax(cmd);
            DataTable tb = new DataTable();
            ad.Fill(tb);
            return tb;
        }
        public abstract DbParameter GetParameter(string name, object value);
        public virtual int ExecuteNonQuery(DbCommand cmd)
        {
            CleanParameterSyntax(cmd);
            return cmd.ExecuteNonQuery();
        }

        public virtual object ExecuteScalar(DbCommand cmd)
        {
            try
            {
                object value;
                CleanParameterSyntax(cmd);
                 value = cmd.ExecuteScalar();
                return value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected abstract void CleanParameterSyntax(DbCommand command);

    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace CMS.DB
{
    internal class MySqlAccess:DBAccess
    {
        public MySqlAccess()
        { }

        public override void BulkCopy(DataTable source, string destination)
        {
            using (MySqlConnection conn = new MySqlConnection(base.ConnectionString))
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        cmd.CommandText = String.Format("delete from {0};select * from {0};",destination);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.UpdateBatchSize = 1000;
                        using (MySqlCommandBuilder cb = new MySqlCommandBuilder(da))
                        {
                            da.Update(source);
                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }

                }

            }
         
        }

        public override DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        public override DbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override DbDataAdapter GetDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        public override DbParameter GetParameter(string name, object value)
        {
            return new MySqlParameter(name, value);

        }

        protected override void CleanParameterSyntax(DbCommand command)
        {
            try
            {
                if (command.CommandType == CommandType.Text)
                {
                    command.CommandText = command.CommandText.Replace("@", "?");
                    command.CommandText = command.CommandText.Replace("??", "@@");

                    command.CommandText = command.CommandText.Replace("GETDATE()", "NOW()");
                }
                foreach (DbParameter p in command.Parameters)
                {
                    p.ParameterName = p.ParameterName.Replace("@", "?");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

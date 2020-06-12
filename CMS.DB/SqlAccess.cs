using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB
{
    internal class SqlAccess : DBAccess
    {
        public override DbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public override DbDataAdapter GetDataAdapter()
        {
            return new SqlDataAdapter();
        }
        public override DbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        public override DbParameter GetParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        protected override void CleanParameterSyntax(DbCommand command)
        {
            //throw new NotImplementedException();
        }

        public override void BulkCopy(DataTable source, string destination)
        {
            throw new NotImplementedException();
        }
    }
}

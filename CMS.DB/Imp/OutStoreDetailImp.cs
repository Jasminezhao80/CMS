using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB.Imp
{
    public class OutStoreDetailImp
    {
        public void Delete(int id)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                string sql = "delete from tb_outstore_detail where id = " + id;
                DbCommand cmd = ac.CreateCommand(sql, conn);
                ac.ExecuteNonQuery(cmd);
            }
        }
    }
}

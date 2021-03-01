using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB.Imp
{
    public class PurchaseDetailImp
    {
        public void UpdateProductId(int id,int productId)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                string sql = "update tb_purchase_orderdetail set product_id={0} where id = {1}";
                DbCommand cmd = ac.CreateCommand(string.Format(sql, productId, id),conn);
                ac.ExecuteNonQuery(cmd);
            }
        }
  
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB.Imp
{
    public class InstoreDetailImp
    {
        public void Delete(int id)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                string sql = "delete from tb_instore_detail where id = " + id; 
                DbCommand cmd = ac.CreateCommand(sql, conn);
                ac.ExecuteNonQuery(cmd);
            }
        }
        /// <summary>
        /// 获取此商品的库存量
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int GetProductLeftQuantity(int productId)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT T1.inTotal-T2.outTotal
                                FROM 
                                (SELECT SUM(quantity) AS inTotal FROM tb_instore_detail WHERE product_id ={0}) T1,
                                (SELECT SUM(quantity) AS outTotal FROM tb_outstore_detail WHERE product_id ={0}) T2";
                DbCommand cmd = ac.CreateCommand(string.Format(sql,productId), conn);
                object result = ac.ExecuteScalar(cmd);
                return Convert.ToInt32(result);
            }
        }
    }
}

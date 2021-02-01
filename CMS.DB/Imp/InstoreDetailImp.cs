using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB.Imp
{
    public class InstoreDetailImp
    {
        public void DeleteById(int id)
        {
            Delete(id, null);
        }
        public void DeleteByPurchaseDetailId(int purchaseDetailId)
        {
            Delete(null, purchaseDetailId);
        }
        /// <summary>
        /// id:tb_instore_detail.id
        /// </summary>
        /// <param name="id"></param>
         private void Delete(int? id, int? purchaseDetailId)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand("sp_cancelInstore", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(ac.GetParameter("@purchaseDetailId", purchaseDetailId));
                cmd.Parameters.Add(ac.GetParameter("@instoreId", id));
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
                string sql = @"SELECT T1.inTotal - T2.outTotal
FROM 
(SELECT (case when SUM(quantity) IS NULL then 0 ELSE SUM(quantity) end) AS inTotal FROM tb_instore_detail WHERE product_id ={0}) T1,
(SELECT (case when SUM(quantity) IS NULL then 0 ELSE SUM(quantity) END ) AS outTotal FROM tb_outstore_detail WHERE product_id ={0}) T2";
                DbCommand cmd = ac.CreateCommand(string.Format(sql,productId), conn);
                object result = ac.ExecuteScalar(cmd);
                return Convert.ToInt32(result);
            }
        }
        public void SaveInWareHouse(int id, object date,int? inQuantity,string userName)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                DbCommand cmd = ac.CreateCommand("sp_updateInstore", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(ac.GetParameter("@detailOrderId", id));
                cmd.Parameters.Add(ac.GetParameter("@loginUser", userName));
                cmd.Parameters.Add(ac.GetParameter("@inputDate", date));
                cmd.Parameters.Add(ac.GetParameter("@inQuantity", inQuantity));
                ac.ExecuteNonQuery(cmd);
            }
        }
    }
}

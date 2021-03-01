using CMS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DB.Imp
{
    public class ProductImp
    {
        public int InsertProduct(Product product)
        {
            DBAccess ac = DBAccess.CreateInstance();
            using (DbConnection conn = ac.GetConnection())
            {
                conn.Open();
                string sql = @"insert into tb_product (product_name,product_size,
                            product_material,product_unit_id,product_category_id) 
                        values (@name,@size,@material,@unit,@category);select @@identity";
                DbCommand cmd = ac.CreateCommand(sql, conn);
                cmd.Parameters.Add(ac.GetParameter("@name", product.Name));
                cmd.Parameters.Add(ac.GetParameter("@size", product.Size));
                cmd.Parameters.Add(ac.GetParameter("@material", product.Material));
                cmd.Parameters.Add(ac.GetParameter("@unit", product.UnitId));
                cmd.Parameters.Add(ac.GetParameter("@category", product.CategoryId));
                return Convert.ToInt32(ac.ExecuteScalar(cmd));
            }
        }
        public Product GetProductById(int id)
        {
            string sql = "select * from tb_product where id =" + id;
            DataTable tb = DBHelper.GetTableBySql(sql);
            if (tb.Rows.Count > 0)
            {
                DataRow row = tb.Rows[0];
                Product product = new Product();
                product.Id = id;
                product.Material = row["product_material"].ToString();
                product.Size = row["product_size"].ToString();
                product.Name = row["product_name"].ToString();
                product.CategoryId = Convert.ToInt32(row["product_category_id"]);
                product.UnitId = Convert.ToInt32(row["product_unit_id"]);
                return product;
            }
            return null;
        }
        public int GetProductId(string name,string size,string material)
        {
            string sql = "select id from tb_product where product_name='{0}' and product_size='{1}' and product_material='{2}'";
            DataTable tb = DBHelper.GetTableBySql(string.Format(sql,name,size,material));
            if (tb.Rows.Count > 0)
            {
                return Convert.ToInt32(tb.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="material"></param>
        /// <param name="categoryId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public Product GetProduct(string name,string size,string material,string categoryId,string unitId)
        {
            string sql = "select * from tb_product where product_name='{0}' and product_size = '{1}' and product_material ='{2}' and product_category_id={3} and product_unit_id={4} ";
            DataTable tb = DBHelper.GetTableBySql(string.Format(sql,new object[] {name,size,material,Convert.ToInt32(categoryId),Convert.ToInt32(unitId) }));
            if (tb.Rows.Count > 0)
            {
                DataRow row = tb.Rows[0];
                Product product = new Product();
                product.Id = Convert.ToInt32(row["id"].ToString());
                product.Material = row["product_material"].ToString();
                product.Size = row["product_size"].ToString();
                product.Name = row["product_name"].ToString();
                product.CategoryId = Convert.ToInt32(row["product_category_id"]);
                product.UnitId = Convert.ToInt32(row["product_unit_id"]);
                return product;
            }
            return null;
        }

        /// <summary>
        /// 如果此商品没有被采购单引用，则直接删除。如果此商品已经开过采购单，则逻辑删除，只更新del_flag=1，并不会实际删除。
        /// </summary>
        /// <param name="productId"></param>
        public void DeleteProduct(int productId)
        {
            string delSql = "";
            //判断此商品是否有采购单引用
            if (CheckProductRef(productId))
            {
                //有采购单引用，则逻辑删除
                delSql = "update tb_product set del_flag = 1 where id = @id";

            }
            else {
                delSql = "delete from tb_product where id = @id";
               
            }
            DBAccess access = DBAccess.CreateInstance();
            using (DbConnection conn = access.GetConnection())
            {
                conn.Open();
                DbCommand cmd = access.CreateCommand(delSql, conn);
                cmd.Parameters.Add(access.GetParameter("@id", productId));
                access.ExecuteNonQuery(cmd);
            }
        }
        /// <summary>
        /// 检验商品是否被采购单引用，如果有采购单引用则不能删除，只能做删除标识
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>true:有采购单引用此商品
        /// false:没有采购单引用此商品
        /// </returns>
        public bool CheckProductRef(int productId)
        {
            DataTable tb = DBHelper.GetTableBySql("select id from tb_purchase_orderdetail where product_id =" + productId);
            if (tb == null || tb.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}

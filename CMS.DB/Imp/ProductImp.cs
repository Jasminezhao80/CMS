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
    }
}

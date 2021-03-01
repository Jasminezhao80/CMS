using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;
using CMS.DB;
using CMS.DB.Imp;
namespace CMS.Bll
{
    public class ProductBll
    {
        ProductImp imp = new ProductImp();
        public Product GetProductById(int id)
        {
            return imp.GetProductById(id);
        }
        public Product GetProduct(string name,string size,string material,string categoryId,string unitId)
        {
            return imp.GetProduct(name,size,material,categoryId,unitId);
        }
        public void Delete(int productId)
        {
            imp.DeleteProduct(productId);
        }

    }
}

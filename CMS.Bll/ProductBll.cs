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
        public Product GetProductById(int id)
        {
            ProductImp imp = new ProductImp();
            return imp.GetProductById(id);
        }
    }
}

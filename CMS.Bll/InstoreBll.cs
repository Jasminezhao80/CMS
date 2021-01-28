using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DB.Imp;
namespace CMS.Bll
{
    public class InstoreBll
    {
        InstoreDetailImp imp = null;

        public InstoreBll() {
            imp = new InstoreDetailImp();
        }
        public void Delete(int id)
        {
            imp.Delete(id);
        }
        public int GetProductLeftQuantity(int productId)
        {
            return imp.GetProductLeftQuantity(productId);
        }
    }
}

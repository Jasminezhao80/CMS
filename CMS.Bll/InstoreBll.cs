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
        public void DeleteByPurchaseDetailId(int purchaseDetailId)
        {
            imp.DeleteByPurchaseDetailId(purchaseDetailId);
        }
        public void DeleteById(int id)
        {
            imp.DeleteById(id);
        }
        public int GetProductLeftQuantity(int productId)
        {
            return imp.GetProductLeftQuantity(productId);
        }
        public void SaveInWareHouse(int id, object date, int? inQuantity, string userName)
        {
            imp.SaveInWareHouse(id, date, inQuantity, userName);
        }
    }
}

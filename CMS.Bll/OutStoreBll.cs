using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DB.Imp;
namespace CMS.Bll
{
    public class OutStoreBll
    {
        OutStoreDetailImp imp = new OutStoreDetailImp();
        public void Delete(int id)
        {
            imp.Delete(id);
        
        }
    }
}

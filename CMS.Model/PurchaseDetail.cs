using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class PurchaseDetail
    {
        private int id;
        private int productId;

        public int Id { get => id; set => id = value; }
        public int ProductId { get => productId; set => productId = value; }
    }
}

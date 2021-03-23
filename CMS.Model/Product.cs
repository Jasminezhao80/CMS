using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class Product
    {
        private int id;
        private string name;
        private string size;
        private string material;
        private int categoryId;
        private int unitId;
        private int instoreFlag;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Size { get => size; set => size = value; }
        public string Material { get => material; set => material = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public int UnitId { get => unitId; set => unitId = value; }
        public int InstoreFlag { get => instoreFlag; set => instoreFlag = value; }
    }
}

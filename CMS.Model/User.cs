using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class User
    {
        private string userName;
        private int userId;
        private List<string> permissionCode;
        public string UserName { get => userName; set => userName = value; }
        public int UserId { get => userId; set => userId = value; }
        public List<string> PermissionCode { get => permissionCode; set => permissionCode = value; }
    }
}

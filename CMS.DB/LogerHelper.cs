using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CMS.DB
{
    public class LogerHelper
    {
        public static void Info(Type type,string info)
        {
            ILog log = LogManager.GetLogger(type);
            log.Info(info);

    }
}
}

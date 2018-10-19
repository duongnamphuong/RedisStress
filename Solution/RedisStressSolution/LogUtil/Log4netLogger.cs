using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUtil
{
    public static class Log4netLogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Info(object message)
        {
            log.Info(message);
        }
        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }
    }
}

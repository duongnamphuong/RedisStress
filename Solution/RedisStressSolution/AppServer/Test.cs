using LogUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppServer
{
    class Test
    {
        static void Main(string[] args)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Running Windows service as a Console...");
            var service1 = new AppService();
            try
            {
                service1.RunStartActions();
            }
            catch (Exception e)
            {
                LogUtil.Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, "Error when trying to start Windows service as a Console.", e);
            }
            Console.Write("Press Enter to terminate this Console...");
            Console.ReadLine();
            service1.RunStopActions();
        }
    }
}

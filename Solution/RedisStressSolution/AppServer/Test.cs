using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServer
{
    class Test
    {
        static void Main(string[] args)
        {
            LogUtil.Log4netLogger.Info("Running Windows service as a Console");
            try
            {
                var service1 = new Service1();
                service1.RunStartActions();
            }
            catch (Exception e)
            {
                LogUtil.Log4netLogger.Error("Error when trying to start Windows service as a Console", e);
            }
            Console.Write("Press Enter to terminate this Console");
            Console.ReadLine();
        }
    }
}

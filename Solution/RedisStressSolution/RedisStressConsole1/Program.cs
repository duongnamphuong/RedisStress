using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisStressConsole1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting to server...");
            StackExchange.Redis.IServer server = null;
            StackExchange.Redis.IDatabase cache = null;
            try
            {
                server = RedisConnectorHelper.Connection.GetServer(ConfigurationManager.AppSettings["redisserver"]);
                cache = RedisConnectorHelper.Connection.GetDatabase();
                Console.WriteLine("Connect to {0} successfully", ConfigurationManager.AppSettings["redisserver"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}

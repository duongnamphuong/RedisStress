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
            DateTime start, end;
            var rnd = new Random();
            try
            {
                server = RedisConnectorHelper.Connection.GetServer(ConfigurationManager.AppSettings["redisserver"]);
                cache = RedisConnectorHelper.Connection.GetDatabase();
                Console.WriteLine("Connect to {0} successfully", ConfigurationManager.AppSettings["redisserver"]);
                
                var ProductKeys = server.Keys(pattern: "Product*");

                #region delete
                Console.WriteLine("DELETE all \"Product*\"-pattern keys in Redis...");
                start = DateTime.Now;
                foreach (var key in ProductKeys)
                {
                    cache.KeyDelete(key);
                }
                end = DateTime.Now;
                Console.WriteLine("There are {0} \"Product*\"-pattern keys. Deleted them in {1} millisecs", ProductKeys.Count(), (end - start).TotalMilliseconds);
                #endregion

                #region add
                Console.Write("How many Products do you want to prepare in Redis? ");
                int NumberOfProducts = int.Parse(Console.ReadLine());
                Console.WriteLine($"Preparing {NumberOfProducts} products...");
                start = DateTime.Now;
                for (int i = 1; i <= NumberOfProducts; i++)
                {
                    cache.StringSet($"Product{i}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                }
                end = DateTime.Now;
                Console.WriteLine($"Prepared {NumberOfProducts} products in {(end - start).TotalMilliseconds} millisecs");
                #endregion

                Console.Write($"How many heartbeats do you want to play on {ProductKeys.Count()} products? ");
                int NumberOfHeartbeats = int.Parse(Console.ReadLine());
                Console.WriteLine($"Preparing to spam {NumberOfHeartbeats} heatbeats on {ProductKeys.Count()} products...");
                start = DateTime.Now;
                for (int i = 0; i < NumberOfHeartbeats; i++)
                {
                    string key = $"Product{rnd.Next(1, ProductKeys.Count() + 1)}";
                    cache.StringSet(key, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                }
                end = DateTime.Now;
                Console.WriteLine($"Updating {NumberOfHeartbeats} heatbeats in {(end - start).TotalMilliseconds} millisecs");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}

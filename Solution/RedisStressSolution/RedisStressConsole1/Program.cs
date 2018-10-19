using RedisUtil;
using System;
using System.Configuration;
using System.Linq;

namespace RedisStressConsole1
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start, end;
            var rnd = new Random();
            try
            {
                var ProductKeys = RedisConnectorHelper.Server.Keys(pattern: "Product*").ToList();

                #region delete
                Console.WriteLine("DELETE all \"Product*\"-pattern keys in Redis...");
                start = DateTime.Now;
                foreach (var key in ProductKeys)
                {
                    RedisConnectorHelper.Cache.KeyDelete(key);
                }
                end = DateTime.Now;
                Console.WriteLine($"There are {ProductKeys.Count()} \"Product*\"-pattern keys. Deleted them in {(end - start).TotalMilliseconds} millisecs");
                #endregion

                #region add
                Console.Write("How many Products do you want to prepare in Redis? ");
                int NumberOfProducts = int.Parse(Console.ReadLine());
                Console.WriteLine($"Preparing {NumberOfProducts} products...");
                start = DateTime.Now;
                for (int i = 1; i <= NumberOfProducts; i++)
                {
                    RedisConnectorHelper.Cache.StringSet($"Product{i}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                }
                end = DateTime.Now;
                Console.WriteLine($"Prepared {NumberOfProducts} products in {(end - start).TotalMilliseconds} millisecs");
                #endregion

                Console.Write($"How many heartbeats do you want to play on {NumberOfProducts} products? ");
                int NumberOfHeartbeats = int.Parse(Console.ReadLine());
                Console.WriteLine($"Preparing to spam {NumberOfHeartbeats} heatbeats on {NumberOfProducts} products...");
                start = DateTime.Now;
                for (int i = 0; i < NumberOfHeartbeats; i++)
                {
                    string key = $"Product{rnd.Next(1, NumberOfProducts + 1)}";
                    string value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                    RedisConnectorHelper.Cache.StringSet(key, value);
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

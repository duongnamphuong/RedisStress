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
                RedisConnector connector = new RedisConnector(ConfigurationManager.AppSettings["redisserver"], false);

                #region delete
                Console.WriteLine("DELETE all \"Product*\"-pattern keys in Redis...");
                start = DateTime.Now;
                int countDeleted = connector.KeyDelete("Product*");
                end = DateTime.Now;
                Console.WriteLine($"There are {countDeleted} \"Product*\"-pattern keys. Deleted them in {(end - start).TotalMilliseconds} millisecs");
                #endregion

                #region add
                Console.Write("How many Products do you want to prepare in Redis? ");
                int NumberOfProducts = int.Parse(Console.ReadLine());
                Console.WriteLine($"Preparing {NumberOfProducts} products...");
                start = DateTime.Now;
                for (int i = 1; i <= NumberOfProducts; i++)
                {
                    connector.StringSet($"Product{i}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
                    string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    connector.StringSet(key, value);
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

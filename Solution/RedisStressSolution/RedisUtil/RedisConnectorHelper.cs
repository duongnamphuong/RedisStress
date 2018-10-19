using StackExchange.Redis;
using System;
using System.Configuration;

namespace RedisUtil
{
    public class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redisserver"]);
            });
            Server = lazyConnection.Value.GetServer(ConfigurationManager.AppSettings["redisserver"]);
            Cache = lazyConnection.Value.GetDatabase();
            Console.WriteLine($"Connect to {ConfigurationManager.AppSettings["redisserver"]} successfully.");
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static IServer server;
        private static IDatabase cache;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static IServer Server
        {
            get
            {
                return server;
            }
            private set
            {
                server = value;
            }
        }

        public static IDatabase Cache
        {
            get
            {
                return cache;
            }
            private set
            {
                cache = value;
            }
        }
    }
}

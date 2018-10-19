using StackExchange.Redis;
using System;

namespace RedisUtil
{
    public class RedisConnector
    {
        private Lazy<ConnectionMultiplexer> _lazyConnection;
        private IServer _server;
        private IDatabase _database;

        /// <summary>
        /// Construct a new connection to a Redis server.
        /// </summary>
        /// <param name="serverSocket">For example: localhost:6379, or 192.168.x.y:port</param>
        public RedisConnector(string serverSocket)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(serverSocket);
            });
            _server = _lazyConnection.Value.GetServer(serverSocket);
            _database = _lazyConnection.Value.GetDatabase();
        }

        public bool StringSet(string key, string value)
        {
            return _database.StringSet(key, value);
        }

        /// <summary>
        /// Delete keys with certain pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>Number of deleted keys</returns>
        public int KeyDelete(string pattern)
        {
            int count = 0;
            //var ProductKeys = _server.Keys(pattern: pattern).ToList();
            var ProductKeys = _server.Keys(pattern: pattern);
            foreach (var key in ProductKeys)
            {
                if (_database.KeyDelete(key))
                    count++;
            }
            return count;
        }
    }
}

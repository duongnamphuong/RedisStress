using StackExchange.Redis;
using System;
using System.Net;

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
        public RedisConnector(string serverSocket, bool AbortOnConnectFail)
        {
            string[] ep = serverSocket.Split(':');
            if (ep.Length != 2)
                throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException($"Invalid IP address: {ep[0]}");
            }
            int port;
            if (!int.TryParse(ep[1], out port))
            {
                throw new FormatException("Invalid port");
            }
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            ConfigurationOptions option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { endpoint }
            };
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(option);
            });
            _server = _lazyConnection.Value.GetServer(serverSocket);
            _database = _lazyConnection.Value.GetDatabase();
        }

        public bool StringSet(string key, string value)
        {
            return _database.StringSet(key, value);
        }

        public string StringGet(string key)
        {
            return _database.StringGet(key);
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

        ~RedisConnector()
        {
            _lazyConnection = null;
            _server = null;
            _database = null;
        }
    }
}

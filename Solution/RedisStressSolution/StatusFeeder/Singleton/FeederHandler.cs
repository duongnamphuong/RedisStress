using RedisUtil;
using System;

namespace StatusFeeder.Singleton
{
    internal class FeederHandler
    {
        #region essential parts of Singleton Pattern

        private static object syncRoot = new Object();
        private static volatile FeederHandler instance;

        public static FeederHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new FeederHandler();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion
        private RedisConnector _connector = null;
        public RedisConnector Connector
        {
            get { return _connector; }
            set { _connector = value; }
        }
    }
}

using RedisUtil;
using System;
using System.Collections.Generic;

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
        private List<string> _imeiList = new List<string>();

        public RedisConnector Connector
        {
            get { return _connector; }
            set { _connector = value; }
        }

        public List<string> ImeiList
        {
            get { return _imeiList; }
            set { _imeiList = value; }
        }
    }
}

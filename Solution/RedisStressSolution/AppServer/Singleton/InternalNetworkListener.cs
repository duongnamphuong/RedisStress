using ProtocolUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServer.Singleton
{
    internal class InternalNetworkListener
    {
        #region essential parts of Singleton Pattern

        private static object syncRoot = new Object();
        private static volatile InternalNetworkListener instance;

        public static InternalNetworkListener Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new InternalNetworkListener();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        internal TcpConnection TcpConnection { get; set; }
    }
}

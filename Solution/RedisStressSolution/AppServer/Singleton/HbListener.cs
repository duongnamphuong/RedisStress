using ProtocolUtil;
using ProtocolUtil.Event;
using ProtocolUtil.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace AppServer.Singleton
{
    internal class HbListener
    {
        #region essential parts of Singleton Pattern

        private static object syncRoot = new Object();
        private static volatile HbListener instance;

        public static HbListener Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new HbListener();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        public UdpConnection PacketConnection { get; set; }

        public void DataReceived(object sender, PacketDataReceivedEventArgs e)
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Get {e.TotalBytesRead} byte(s): 0x{e.DataRead}");
            PacketConnection.Send(new byte[] { 0xAB, 0xFF, 0x00 }, e.DestinationTuple);
        }

        public void DataSent(object sender, PacketDataSentEventArgs e)
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Send {e.Data.Count()} byte(s) 0x{ByteStreamUtil.ByteToHexBit(e.Data)}");
        }
    }
}

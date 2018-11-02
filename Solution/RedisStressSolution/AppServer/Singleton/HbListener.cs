using LogUtil;
using ProtocolUtil;
using ProtocolUtil.Event;
using ProtocolUtil.Utils;
using RedisUtil;
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
        private RedisConnector _connector = null;
        public RedisConnector Connector
        {
            get { return _connector; }
            set { _connector = value; }
        }

        public void DataReceived(object sender, PacketDataReceivedEventArgs e)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Get {e.TotalBytesRead} byte(s): 0x{e.DataRead} from {e.DestinationTuple.RemoteEndPoint}");
            if (e.TotalBytesRead == 8 && e.DataRead.Substring(0, 6) == "AB0106" && e.DataRead.Substring(14, 2) == "00")
            {
                try
                {
                    byte[] ImeiStream = e.BytesRead.Skip(3).Take(4).ToArray();
                    int ImeiInt = BitConverter.ToInt32(ImeiStream.Reverse().ToArray(), 0);
                    string ImeiWithLeadingZerosLength20 = ImeiInt.ToString("00000000000000000000");
                    Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Imei: {ImeiInt}. With leading zeros: {ImeiWithLeadingZerosLength20}");
                    string key = $"Product_{ImeiWithLeadingZerosLength20}_Heartbeat";
                    string value = _connector.StringGet(key);

                    //if found the key (value == null)
                    if (value != null)
                    {
                        _connector.StringSet(key, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        PacketConnection.Send(new byte[] { 0xAB, 0xFF, 0x06, e.BytesRead[3], e.BytesRead[4], e.BytesRead[5], e.BytesRead[6], 0x01, 0x01 }, e.DestinationTuple);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, $"Error during DataReceived (UDP)", ex);
                }
            }
        }

        public void DataSent(object sender, PacketDataSentEventArgs e)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Send {e.Data.Count()} byte(s) 0x{ByteStreamUtil.ByteToHexBit(e.Data)}");
        }
    }
}

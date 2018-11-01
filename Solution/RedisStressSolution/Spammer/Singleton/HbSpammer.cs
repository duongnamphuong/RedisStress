using LogUtil;
using ProtocolUtil;
using ProtocolUtil.Event;
using ProtocolUtil.Utils;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spammer.Singleton
{
    class HbSpammer
    {
        #region essential parts of Singleton Pattern

        private static object syncRoot = new Object();
        private static volatile HbSpammer instance;

        public static HbSpammer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new HbSpammer();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion
        public UdpConnection PacketConnection { get; set; }
        public ISchedulerFactory _schedulerFactory1 = new StdSchedulerFactory();
        public IScheduler _scheduler1;
        public int frequency;
        public List<byte[]> HbByteStreamList = new List<byte[]>();
        public void DataReceived(object sender, PacketDataReceivedEventArgs e)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Get {e.TotalBytesRead} byte(s): 0x{e.DataRead}");
        }
        public void DataSent(object sender, PacketDataSentEventArgs e)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Send {e.Data.Count()} byte(s) 0x{ByteStreamUtil.ByteToHexBit(e.Data)} from {e.Destination.LocalEndPoint} to {e.Destination.RemoteEndPoint}");
        }
    }
}

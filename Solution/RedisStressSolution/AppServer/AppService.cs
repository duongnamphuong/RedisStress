using AppServer.Singleton;
using Dal;
using LogUtil;
using ProtocolUtil;
using ProtocolUtil.Event;
using RedisUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AppServer
{
    public partial class AppService : ServiceBase
    {
        public AppService()
        {
            InitializeComponent();
        }

        private readonly EventHandler<PacketDataReceivedEventArgs> evtHandlerReceived = new EventHandler<PacketDataReceivedEventArgs>(HbListener.Instance.DataReceived);
        private readonly EventHandler<PacketDataSentEventArgs> evtHandlerSent = new EventHandler<PacketDataSentEventArgs>(HbListener.Instance.DataSent);

        public void RunStartActions()
        {
            HbListener.Instance.Connector = new RedisConnector(ConfigurationManager.AppSettings["redisserver"]);
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Connected to Redis server.");
            List<Product> products = null;
            using (RedisStressContext ctx = new RedisStressContext())
            {
                products = ctx.Products.ToList();
                Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Get {products.Count} products.");
            }
            var start = DateTime.UtcNow;
            int count = 0;
            foreach (Product prod in products)
            {
                try
                {
                    HbListener.Instance.Connector.StringSet($"Product_{prod.Imei}_Heartbeat", (prod.LastHbUtc != null ? prod.LastHbUtc.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""));
                    count++;
                }
                catch (Exception ex)
                {
                    Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
                }
            }
            var end = DateTime.UtcNow;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Set {count} Redis keys in {(end - start).TotalMilliseconds} millisecs.");
            HbListener.Instance.PacketConnection = new UdpConnection();
            HbListener.Instance.PacketConnection.DataReceived += evtHandlerReceived;
            HbListener.Instance.PacketConnection.DataSent += evtHandlerSent;
            HbListener.Instance.PacketConnection.StartUdpListening(int.Parse(ConfigurationManager.AppSettings["listenport"]));
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Heartbeat listener is running...");
        }

        public void RunStopActions()
        {
            HbListener.Instance.PacketConnection.DataReceived -= evtHandlerReceived;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "DataReceived event Unsubscribed.");
            HbListener.Instance.PacketConnection.DataSent -= evtHandlerSent;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "DataSent event Unsubscribed.");
            HbListener.Instance.PacketConnection.Dispose();
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Connection disposed.");
            HbListener.Instance.Connector = null;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Redis connection set to null.");
        }

        protected override void OnStart(string[] args)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStart");
            try
            {
                RunStartActions();
            }
            catch (Exception e)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, "Error when starting Windows service", e);
            }
        }

        protected override void OnStop()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStop");
            RunStopActions();
        }
    }
}

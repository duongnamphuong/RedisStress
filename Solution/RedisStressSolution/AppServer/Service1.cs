using AppServer.Singleton;
using ProtocolUtil;
using ProtocolUtil.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AppServer
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private readonly EventHandler<PacketDataReceivedEventArgs> evtHandlerReceived = new EventHandler<PacketDataReceivedEventArgs>(HbListener.Instance.DataReceived);
        private readonly EventHandler<PacketDataSentEventArgs> evtHandlerSent = new EventHandler<PacketDataSentEventArgs>(HbListener.Instance.DataSent);

        public void RunStartActions()
        {
            HbListener.Instance.PacketConnection = new UdpConnection();
            HbListener.Instance.PacketConnection.DataReceived += evtHandlerReceived;
            HbListener.Instance.PacketConnection.DataSent += evtHandlerSent;
            HbListener.Instance.PacketConnection.StartUdpListening(4060);
        }

        public void RunStopActions()
        {
            HbListener.Instance.PacketConnection.DataReceived -= evtHandlerReceived;
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "DataReceived event Unsubscribed.");
            HbListener.Instance.PacketConnection.DataSent -= evtHandlerSent;
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "DataSent event Unsubscribed.");
            HbListener.Instance.PacketConnection.Dispose();
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Connection disposed.");
        }

        protected override void OnStart(string[] args)
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStart");
            try
            {
                RunStartActions();
            }
            catch (Exception e)
            {
                LogUtil.Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, "Error when starting Windows service", e);
            }
        }

        protected override void OnStop()
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStop");
            RunStopActions();
        }
    }
}

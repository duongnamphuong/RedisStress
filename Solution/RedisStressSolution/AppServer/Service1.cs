using AppServer.Singleton;
using ProtocolUtil;
using ProtocolUtil.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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

        public void RunStartActions()
        {
            HbListener.Instance.PacketConnection = new UdpConnection();
            HbListener.Instance.PacketConnection.DataReceived += new EventHandler<PacketDataReceivedEventArgs>(HbListener.Instance.DataReceived);
            HbListener.Instance.PacketConnection.DataSent += new EventHandler<PacketDataSentEventArgs>(HbListener.Instance.DataSent);
            HbListener.Instance.PacketConnection.StartUdpListening(4060);
        }

        protected override void OnStart(string[] args)
        {
            LogUtil.Log4netLogger.Info("Windows service OnStart");
            try
            {
                RunStartActions();
            }
            catch (Exception e)
            {
                LogUtil.Log4netLogger.Error("Error when starting Windows service", e);
            }
        }

        protected override void OnStop()
        {
            LogUtil.Log4netLogger.Info("Windows service OnStop");
        }
    }
}

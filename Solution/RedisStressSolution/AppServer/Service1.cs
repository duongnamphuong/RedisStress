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

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

        protected override void OnStart(string[] args)
        {
            LogUtil.Log4netLogger.Info("OnStart");
        }

        protected override void OnStop()
        {
            LogUtil.Log4netLogger.Info("OnStop");
        }
    }
}

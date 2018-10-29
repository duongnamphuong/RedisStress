using LogUtil;
using RedisUtil;
using System;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;

namespace StatusFeeder
{
    public partial class Service1 : ServiceBase
    {
        private RedisConnector _connector = null;

        public Service1()
        {
            InitializeComponent();
        }

        public void RunStartActions()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStart");
            try
            {
                _connector = new RedisConnector(ConfigurationManager.AppSettings["redisserver"]);
            }
            catch (Exception ex)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
            }
        }

        public void RunStopActions()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStop");
            _connector = null;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Redis connection set to null.");
        }

        protected override void OnStart(string[] args)
        {
            RunStartActions();
        }

        protected override void OnStop()
        {
            RunStopActions();
        }
    }
}

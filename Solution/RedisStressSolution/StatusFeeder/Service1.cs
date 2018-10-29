using System.Reflection;
using System.ServiceProcess;

namespace StatusFeeder
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void RunStartActions()
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStart");
        }

        public void RunStopActions()
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStop");
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

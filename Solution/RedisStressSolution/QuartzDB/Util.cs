using System.Collections.Specialized;
using System.Configuration;

namespace QuartzDB
{
    internal class Util
    {
        private static NameValueCollection quartzProperties;
        public static NameValueCollection QuartzProperties
        {
            get
            {
                quartzProperties = new NameValueCollection();

                // Configure Scheduler
                quartzProperties.Add("quartz.scheduler.instanceName", "myScheduler1");

                // Configure Thread Pool
                quartzProperties.Add("quartz.threadPool.type", "Quartz.Simpl.SimpleThreadPool, Quartz");
                quartzProperties.Add("quartz.threadPool.threadCount", "10");
                quartzProperties.Add("quartz.threadPool.threadPriority", "Normal");

                // Configure Job Store
                quartzProperties.Add("quartz.jobStore.misfireThreshold", "60000");
                quartzProperties.Add("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
                quartzProperties.Add("quartz.jobStore.useProperties", "false");
                quartzProperties.Add("quartz.jobStore.dataSource", "default");
                quartzProperties.Add("quartz.jobStore.tablePrefix", "QRTZ_");
                quartzProperties.Add("quartz.jobStore.lockHandler.type", "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz");

                // Configure Data Source
                quartzProperties.Add("quartz.dataSource.default.connectionString", QuartzDbConnectionString);
                quartzProperties.Add("quartz.dataSource.default.provider", "SqlServer-20");

                return quartzProperties;
            }
        }

        private static string QuartzDbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["QuartzStore"].ConnectionString;
            }
        }
    }
}

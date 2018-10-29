using Quartz;
using System;
using System.Reflection;

namespace StatusFeeder.QuartzJobs
{
    [DisallowConcurrentExecution]
    internal class HeartbeatFeeder : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogUtil.Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{nameof(HeartbeatFeeder)}: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} UTC");
        }
    }
}

using Dal;
using LogUtil;
using Quartz;
using StatusFeeder.Singleton;
using System;
using System.Linq;
using System.Reflection;

namespace StatusFeeder.QuartzJobs
{
    [DisallowConcurrentExecution]
    internal class HeartbeatFeeder : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{nameof(HeartbeatFeeder)}: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} UTC");
            for (int i = 0; i < FeederHandler.Instance.ImeiList.Count; i++)
            {
                try
                {
                    string currentImei = FeederHandler.Instance.ImeiList[i];
                    string LastHbUtcStr = FeederHandler.Instance.Connector.StringGet($"Product_{currentImei}_Heartbeat");
                    DateTime LastHbUtc;
                    if (DateTime.TryParse(LastHbUtcStr, out LastHbUtc))
                    {
                        using (RedisStressContext ctx = new RedisStressContext())
                        {
                            var prod = ctx.Products.Where(p => p.Imei == currentImei).FirstOrDefault();
                            if (prod != null)
                            {
                                prod.LastHbUtc = LastHbUtc;
                                ctx.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, $"Exception in loop #{i}");
                    Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
                }
            }
        }
    }
}

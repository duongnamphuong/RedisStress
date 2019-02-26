using Dal;
using LogUtil;
using Quartz;
using Quartz.Impl;
using RedisUtil;
using StatusFeeder.QuartzJobs;
using StatusFeeder.Singleton;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace StatusFeeder
{
    public partial class FeederService : ServiceBase
    {
        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public FeederService()
        {
            InitializeComponent();
        }

        public void RunStartActions()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStart");
            try
            {
                FeederHandler.Instance.Connector = new RedisConnector(ConfigurationManager.AppSettings["redisserver"], false);
                using (RedisStressContext ctx = new RedisStressContext())
                {
                    FeederHandler.Instance.ImeiList= ctx.Products.Select(p => p.Imei).ToList();
                }
                Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"Loaded {FeederHandler.Instance.ImeiList.Count} IMEIs from SQL Server database into memory.");

                #region Quartz

                _schedulerFactory = new StdSchedulerFactory();
                _scheduler = _schedulerFactory.GetScheduler();
                IJobDetail job = JobBuilder.Create<HeartbeatFeeder>().WithIdentity("StatusFeederJob", "StatusFeederGroup").Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("StatusFeederTrigger", "StatusFeederGroup").WithCronSchedule(ConfigurationManager.AppSettings["StatusFeederCron"], x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("UTC"))).Build();
                _scheduler.ScheduleJob(job, trigger);
                _scheduler.Start();

                #endregion
            }
            catch (Exception ex)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
            }
        }

        public void RunStopActions()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Windows service OnStop");
            try
            {
                #region Quartz

                _scheduler.Shutdown(true);
                Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, "Quartz Shutdown.");

                #endregion
            }
            catch (Exception ex)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
            }
            FeederHandler.Instance.Connector = null;
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

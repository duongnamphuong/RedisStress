using Quartz;
using QuartzDB.Jobs;
using System;

namespace QuartzDB
{
    internal class Test
    {
        private static void Main(string[] args)
        {
            try
            {
                QuartzManagement.InitializeScheduler();

                //clear
                QuartzManagement.Clear();

                //Add a cron job
                IJobDetail job1 = JobBuilder.Create<TestJob1>().WithIdentity("myJob1", "myGroup1").Build();
                string cronString = "0,15,30,45 * * ? * * *";
                ITrigger trigger1 = TriggerBuilder.Create().WithIdentity("myTrigger1", "myGroup1").WithCronSchedule(cronString, x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("UTC"))).Build();
                IScheduler sch = QuartzManagement.GetScheduler();
                sch.ScheduleJob(job1, trigger1);

                //start jobs
                QuartzManagement.StartJobs();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

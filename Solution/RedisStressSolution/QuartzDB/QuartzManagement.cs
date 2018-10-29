using Quartz;
using Quartz.Impl;

namespace QuartzDB
{
    public static class QuartzManagement
    {
        private static ISchedulerFactory sf;
        private static IScheduler sched;

        public static ISchedulerFactory GetSchedulerFactory()
        {
            if (sf == null)
            {
                InitializeScheduler();
            }
            return sf;
        }

        public static IScheduler GetScheduler()
        {
            if (sched == null)
            {
                InitializeScheduler();
            }
            return sched;
        }

        /// <summary>
        ///  Start scheduler
        /// </summary>
        public static void InitializeScheduler()
        {
            if (sf == null)
            {
                sf = new StdSchedulerFactory(Util.QuartzProperties);
            }
            if (sched == null)
            {
                sched = sf.GetScheduler();
            }
        }

        /// <summary>
        ///  shutdown scheduler
        /// </summary>
        public static void ShutdownSchedule()
        {
            if (sched != null)
            {
                sched.Shutdown(true);
            }
        }

        public static void PauseSchedule()
        {
            if (sched != null)
            {
                sched.PauseAll();
            }
        }

        public static void ResumSchedule()
        {
            if (sched != null)
            {
                sched.ResumeAll();
            }
        }

        /// <summary>
        /// Start all job
        /// </summary>
        public static void StartJobs()
        {
            if (sched != null)
            {
                sched.Start();
            }
        }

        public static void Clear()
        {
            if (sched != null)
            {
                sched.Clear();
            }
        }
    }
}

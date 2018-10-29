using Quartz;
using System;

namespace QuartzDB.Jobs
{
    internal class TestJob1 : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{nameof(TestJob1)}: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} UTC");
        }
    }
}

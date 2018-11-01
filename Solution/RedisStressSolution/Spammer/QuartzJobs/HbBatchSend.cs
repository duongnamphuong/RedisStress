using Quartz;
using Spammer.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spammer.QuartzJobs
{
    [DisallowConcurrentExecution]
    internal class HbBatchSend : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Random rnd = new Random();
            for (int i = 0; i < HbSpammer.Instance.frequency; i++)
            {
                byte[] RandomHeartbeat = HbSpammer.Instance.HbByteStreamList[rnd.Next(0, HbSpammer.Instance.HbByteStreamList.Count)];
                HbSpammer.Instance.PacketConnection.Send(RandomHeartbeat, HbSpammer.Instance.PacketConnection.destinationTuple);
            }
            
        }
    }
}

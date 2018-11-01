using Dal;
using LogUtil;
using ProtocolUtil;
using ProtocolUtil.Event;
using Quartz;
using Quartz.Impl;
using Spammer.QuartzJobs;
using Spammer.Singleton;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spammer
{
    class Program
    {
        static void Main(string[] args)
        {
            EventHandler<PacketDataReceivedEventArgs> evtHandlerReceived = new EventHandler<PacketDataReceivedEventArgs>(HbSpammer.Instance.DataReceived);
            EventHandler<PacketDataSentEventArgs> evtHandlerSent = new EventHandler<PacketDataSentEventArgs>(HbSpammer.Instance.DataSent);
            HbSpammer.Instance.PacketConnection = new UdpConnection();
            HbSpammer.Instance.PacketConnection.DataReceived += evtHandlerReceived;
            HbSpammer.Instance.PacketConnection.DataSent += evtHandlerSent;
            HbSpammer.Instance.PacketConnection.StartUdpListening(int.Parse(ConfigurationManager.AppSettings["localPort"]));
            IPEndPoint localEndPoint, remoteEndPoint;
            localEndPoint = HbSpammer.Instance.PacketConnection.ConnectionSocket.LocalEndPoint as IPEndPoint;
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["remoteIP"]), int.Parse(ConfigurationManager.AppSettings["remotePort"]));
            HbSpammer.Instance.PacketConnection.destinationTuple = new DestinationTuple(localEndPoint, remoteEndPoint);
            Console.Write("How many Heartbeats do you want to send very second? ");
            bool success;
            do
            {
                success = int.TryParse(Console.ReadLine(), out HbSpammer.Instance.frequency);
            }
            while (!success);
            try
            {
                List<string> ImeiList = null;
                using (RedisStressContext ctx = new RedisStressContext())
                {
                    ImeiList = ctx.Products.Select(pr => pr.Imei).ToList();
                }
                for (int i = 0; i < ImeiList.Count; i++)
                {
                    int ImeiInt = int.Parse(ImeiList[i]);
                    byte[] ImeiByteArray = BitConverter.GetBytes(ImeiInt).Reverse().ToArray();
                    byte[] HbByteArray = new byte[] { 0xAB, 0x01, 0x06 }.Concat(ImeiByteArray).Concat(new byte[] { 0x00 }).ToArray();
                    HbSpammer.Instance.HbByteStreamList.Add(HbByteArray);
                }
                HbSpammer.Instance._scheduler1 = HbSpammer.Instance._schedulerFactory1.GetScheduler();
                IJobDetail job = JobBuilder.Create<HbBatchSend>().WithIdentity("HbBatchSend", "HbBatchSendGroup").Build();
                string cronString = "* * * * * ? *";
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("EverySecond", "HbBatchSendGroup").WithCronSchedule(cronString, x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("UTC"))).Build();
                HbSpammer.Instance._scheduler1.ScheduleJob(job, trigger);
                HbSpammer.Instance._scheduler1.Start();
            }
            catch (Exception ex)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, ex);
            }
        }
    }
}

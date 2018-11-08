using System;

namespace StatusFeeder
{
    internal class Test
    {
        private static void Main(string[] args)
        {
            var service1 = new FeederService();
            service1.RunStartActions();
            Console.Write("Press Enter to terminate this Console...");
            Console.ReadLine();
            service1.RunStopActions();
        }
    }
}

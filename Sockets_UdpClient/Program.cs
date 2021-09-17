using Helpers.Sockets.Udp;
using System;
using System.Timers;

namespace Sockets_UdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var udpClient = new UdpSender("127.0.0.1", 8000);

            Timer timer = new Timer(8000);
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                var recievedMessage = udpClient.Send(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (DateTime.TryParse(recievedMessage, out DateTime dateTimeServer))
                {
                    var dateTimeClient = DateTime.Now;
                    Console.WriteLine($" Client[{dateTimeClient}] <-> Server[{dateTimeServer}] : {dateTimeClient.CompareTo(dateTimeServer)}");
                }
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            Console.WriteLine("Press any key to go to exit the program.");
            Console.ReadKey();
        }
    }
}

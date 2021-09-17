using Helpers.Sockets.Udp;
using System;

namespace Sockets_UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var udpServer = new UdpServer(8000);
            udpServer.Start();

            Console.WriteLine("Press any key to go to exit the program.");
            Console.ReadKey();
        }
    }
}

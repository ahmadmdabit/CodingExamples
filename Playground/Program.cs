using Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int num;
            do
            {
                num = SelectOption();
                switch (num)
                {
                    case 1: SecondLSBTest(); break;
                    case 2: CircularBufferTest(); break;
                    case 3: SoftwareVersionTest(); break;
                    case 4: SocketsTest(); break;
                    default: break;
                }
            } while (num != 0);
            //Console.ReadKey();
        }

        private static int SelectOption()
        {
            Console.Clear();
            Console.WriteLine("Select any of these options:");
            Console.WriteLine("1: Second LSB.");
            Console.WriteLine("2: Circular Buffer.");
            Console.WriteLine("3: Software Version.");
            Console.WriteLine("4: Sockets (Udp Server / Client)");
            Console.WriteLine("... ... ...");
            Console.WriteLine("0: Exit");

            // if the number is successfully parsed return number else run this method again
            return int.TryParse(Console.ReadLine(), out int num) ? num : SelectOption();
        }

        #region Second LSB
        private static void SecondLSBTest()
        {
            Console.WriteLine("Second LSB ...");
            RecursionLSB(start: 0, last: 200, endieanType: EndieanType.BigEndian);
            Console.WriteLine("Press any key to go to options menu.");
            Console.ReadKey();
        }

        private static void RecursionLSB(int start, int last, EndieanType endieanType = EndieanType.LittleEndian)
        {
            Console.WriteLine(LSBHelper.GetSecondLSB(start, endieanType));
            if (start < last) RecursionLSB(++start, last, endieanType);
        }
        #endregion

        #region Circular Buffer
        private static void CircularBufferTest()
        {
            Console.WriteLine("Circular Buffer ...");
            // <ref:https://en.wikipedia.org/wiki/Circular_buffer>
            CircularBuffer circularBuffer = new CircularBuffer(size: 7, startIndex: 2, allowOverwriting: true);
            circularBuffer.Put(1);
            circularBuffer.Put(2);
            circularBuffer.Put(3);
            circularBuffer.Remove();
            circularBuffer.Remove();
            circularBuffer.Put(4);
            circularBuffer.Put(5);
            circularBuffer.Put(6);
            circularBuffer.Put(7);
            circularBuffer.Put(8);
            circularBuffer.Put(9);
            circularBuffer.Put('A');
            circularBuffer.Put('B');
            circularBuffer.Remove();
            circularBuffer.Remove();
            Console.WriteLine("Press any key to go to options menu.");
            Console.ReadKey();
        }

        #endregion

        #region Software Version

        private static void SoftwareVersionTest()
        {
            Console.WriteLine("Software Version ...");
            Console.WriteLine($" 212.21.12.13 <-> 212.21.12.13 : {SoftwareVersionHelper.Compare("212.21.12.13", "212.21.12.13")}");
            Console.WriteLine($" 212.23.13.13 <-> 212.21.12.13 : {SoftwareVersionHelper.Compare("212.23.13.13", "212.21.12.13")}");
            Console.WriteLine($" 212.21.12.13 <-> 212.23.13.13 : {SoftwareVersionHelper.Compare("212.21.12.13", "212.23.13.13")}");
            Console.WriteLine("Press any key to go to options menu.");
            Console.ReadKey();
        }

        #endregion

        #region Sockets (Udp Server / Client)

        private static void SocketsTest()
        {
            Console.WriteLine("Sockets (Udp Server / Client) ...");

            Process.Start(Directory.GetCurrentDirectory() + @"\..\..\..\..\Sockets_UdpServer\bin\Debug\netcoreapp3.1\Sockets_UdpServer.exe");
            Process.Start(Directory.GetCurrentDirectory() + @"\..\..\..\..\Sockets_UdpClient\bin\Debug\netcoreapp3.1\Sockets_UdpClient.exe"); // Client

            Console.WriteLine("Press any key to go to options menu.");
            Console.ReadKey();
        }

        #endregion
    }
}
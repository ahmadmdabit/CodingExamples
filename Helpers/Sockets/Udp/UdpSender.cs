using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers.Sockets.Udp
{
    public class UdpSender
    {
        protected bool _Connected;
        protected UdpClient _UdpClient;
        protected readonly int _port;
        protected readonly IPAddress _IPAddress;
        protected IPEndPoint _IPEndPoint;

        public UdpSender(string iPAddress, int port)
        {
            this._IPAddress = IPAddress.Parse(iPAddress);
            this._port = port;
            this._IPEndPoint = new IPEndPoint(this._IPAddress, this._port);

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            this._UdpClient = new UdpClient();

            Console.WriteLine("The UdpSender configured...");

            //Task.Factory.StartNew(() => ListenToIsYourTimeSet()); // Not Completed: It has some issues
        }

        public string Send(string message)
        {
            //WaitUntilDisconnected().Wait();
            if (!this._Connected)
            {
                this._UdpClient = new UdpClient();
                this._UdpClient.Connect(this._IPEndPoint);
                this._Connected = true;
            }
            byte[] bytesToSend = Encoding.Unicode.GetBytes(message);
            this._UdpClient.Send(bytesToSend, bytesToSend.Length);
            Console.WriteLine($"Sent to {this._IPEndPoint}");

            byte[] bytes = this._UdpClient.Receive(ref this._IPEndPoint);
            if (this._Connected)
            {
                this._UdpClient.Close();
                this._Connected = false;
            }

            var recievedMsg = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
            Console.WriteLine($"{recievedMsg} Received broadcast from {this._IPEndPoint}");
            return recievedMsg;
        }

        protected void ListenToIsYourTimeSet()
        {
            var udpClient = new UdpClient(this._port - 1);
            var iPEndPoint = new IPEndPoint(this._IPAddress, this._port - 1);
            while (true)
            {
                //WaitUntilDisconnected().Wait();
                Console.WriteLine("Waiting for ISYOURTIMESET");
                //IPEndPoint iPEndPoint = new IPEndPoint(this._IPAddress, this._port - 1);
                //if (this._Connected) this._UdpClient.Close();
                //this._UdpClient.Connect(iPEndPoint);
                //this._Connected = true;
                byte[] bytes = udpClient.Receive(ref iPEndPoint);
                //this._UdpClient.Close();
                //this._Connected = false;
                var recievedMsg = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                Console.WriteLine($"{recievedMsg} Received ISYOURTIMESET from {iPEndPoint}");

                byte[] bytesToSend = Encoding.Unicode.GetBytes("OK");
                udpClient.Send(bytesToSend, bytesToSend.Length, iPEndPoint);
                Console.WriteLine($"Sent to {iPEndPoint}");
            }
        }

        protected async Task<bool> WaitUntilDisconnected()
        {
            bool succeeded = false;
            while (!succeeded)
            {
                // do work
                succeeded = !this._Connected; // if it worked, make as succeeded, else retry
                await Task.Delay(1000); // arbitrary delay
            }
            return succeeded;
        }
    }
}
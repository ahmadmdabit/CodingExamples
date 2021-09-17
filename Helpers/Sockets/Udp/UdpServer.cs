using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Helpers.Sockets.Udp
{
    public class UdpServer
    {
        protected readonly UdpClient _UdpClient;
        protected readonly UdpClient _UdpClientTime;
        protected readonly int Port;
        protected IPEndPoint _IPEndPoint;
        protected readonly List<IPEndPointCustom> _IPEndPointClients = new List<IPEndPointCustom>();
        protected readonly List<IPEndPointCustom> _IPEndPointClientsTemp = new List<IPEndPointCustom>();
        protected bool _IsLocked;

        public UdpServer(int port)
        {
            this.Port = port;
            this._UdpClient = new UdpClient(Port);
            this._UdpClientTime = new UdpClient();
            this._IPEndPoint = new IPEndPoint(IPAddress.Any, Port);

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("The UdpServer configured...");
        }

        public void Start()
        {
            Console.WriteLine("Starting the UdpServer...");

            try
            {
                //IsYourTimeSet(); // Not Completed: It has some issues
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = this._UdpClient.Receive(ref this._IPEndPoint);
                    var recievedMsg = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"{recievedMsg} Received broadcast from {this._IPEndPoint}");

                    byte[] bytesToSend = Encoding.Unicode.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    this._UdpClient.Send(bytesToSend, bytesToSend.Length, this._IPEndPoint);
                    Console.WriteLine($"Sent to {this._IPEndPoint}");

                    if (!this._IPEndPointClientsTemp.Exists(x => x.IPEndPoint.Address == this._IPEndPoint.Address && x.IPEndPoint.Port == this._IPEndPoint.Port))
                    {
                        _IPEndPointClientsTemp.Add(new IPEndPointCustom { IPEndPoint = this._IPEndPoint, });
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                this._UdpClient.Close();
            }
        }

        protected void IsYourTimeSet()
        {
            Timer timer = new Timer(4000);
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                _IPEndPointClients.Clear();
                _IPEndPointClients.AddRange(_IPEndPointClientsTemp);
                _IPEndPointClients.ForEach(x =>
                {
                    if (!x.Connected)
                    {
                        this._UdpClientTime.Connect(x.IPEndPoint);
                        x.Connected = true;
                    }
                    byte[] bytesToSend = Encoding.Unicode.GetBytes("ISYOURTIMESET");
                    this._UdpClientTime.Send(bytesToSend, bytesToSend.Length);
                    Console.WriteLine($"[ISYOURTIMESET] Sent to {x.IPEndPoint} :");

                    var iPEndPoint = x.IPEndPoint;
                    byte[] bytes = this._UdpClientTime.Receive(ref iPEndPoint);
                    var recievedMsg = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine($"[ISYOURTIMESET:{recievedMsg}] Received broadcast from {iPEndPoint} :");
                });
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        public class IPEndPointCustom
        {
            public IPEndPoint IPEndPoint { get; set; }
            public bool Connected { get; set; }
        }
    }
}
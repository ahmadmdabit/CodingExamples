using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Helpers.Sockets.Tcp
{
    public class SocketServer : IDisposable
    {
        protected readonly TcpListener _TcpListener;
        protected readonly int Port;
        protected IPEndPoint _IPEndPoint;
        private bool disposedValue;
        protected readonly List<IPEndPoint> _IPEndPointClients = new List<IPEndPoint>();
        protected readonly List<IPEndPoint> _IPEndPointClientsTemp = new List<IPEndPoint>();

        public SocketServer(int port)
        {
            this.Port = port;
            this._IPEndPoint = new IPEndPoint(IPAddress.Any, Port);
            this._TcpListener = new TcpListener(this._IPEndPoint);

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("The TcpServer configured...");
        }

        public void Start()
        {
            Console.WriteLine("Starting the TcpServer...");

            TcpClient incomingTcpClient = null;
            try
            {
                //IsYourTimeSet();
                while (true)
                {
                    this._TcpListener.Start();

                    incomingTcpClient = this._TcpListener.AcceptTcpClient();
                    //if (incomingTcpClient == null || !incomingTcpClient.Connected) incomingTcpClient.Connect((IPEndPoint)incomingTcpClient.Client.RemoteEndPoint);

                    NetworkStream networkStream = incomingTcpClient.GetStream();

                    // Receive -------------------------------------------------
                    byte[] bufferReceived = new byte[incomingTcpClient.ReceiveBufferSize];
                    int bytesReceived = networkStream.Read(bufferReceived, 0, incomingTcpClient.ReceiveBufferSize);
                    string messageReceived = Encoding.Unicode.GetString(bufferReceived, 0, bytesReceived);
                    Console.WriteLine($"{messageReceived} Received broadcast from {incomingTcpClient.Client.RemoteEndPoint}");

                    // Send ----------------------------------------------------
                    byte[] bytesToSend = Encoding.Unicode.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    networkStream.Write(bytesToSend, 0, bytesToSend.Length);
                    Console.WriteLine($"Sent to {incomingTcpClient.Client.RemoteEndPoint}");

                    var ipEndPoint = (IPEndPoint)incomingTcpClient.Client.RemoteEndPoint;
                    if (!this._IPEndPointClientsTemp.Exists(x => x.Address == ipEndPoint.Address && x.Port == ipEndPoint.Port))
                    {
                        _IPEndPointClientsTemp.Add(ipEndPoint);
                    }
                    incomingTcpClient.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (incomingTcpClient != null && incomingTcpClient.Connected) incomingTcpClient.Close();
                this._TcpListener.Stop();
            }
        }

        protected void IsYourTimeSet()
        {
            Timer timer = new Timer(4000);
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                _IPEndPointClients.Clear();
                _IPEndPointClients.AddRange(_IPEndPointClientsTemp);
                _IPEndPointClients.ForEach(x => SendToClient(x, "ISYOURTIMESET"));
            };
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        protected void SendToClient(IPEndPoint iPEndPoint, string message)
        {
            var client = new TcpClient();
            try
            {
                if (!client.Connected) client.Connect(iPEndPoint);

                NetworkStream networkStream = client.GetStream();

                // Send ----------------------------------------------------
                byte[] bytesToSend = Encoding.Unicode.GetBytes(message);
                networkStream.Write(bytesToSend, 0, bytesToSend.Length);
                Console.WriteLine($"[{message}] Sent to {client.Client.RemoteEndPoint}");

                // Receive -------------------------------------------------
                byte[] bufferReceived = new byte[client.ReceiveBufferSize];
                int bytesReceived = networkStream.Read(bufferReceived, 0, client.ReceiveBufferSize);
                string messageReceived = Encoding.Unicode.GetString(bufferReceived, 0, bytesReceived);
                Console.WriteLine($"[{message}:{messageReceived}] Received broadcast from {iPEndPoint} :");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (client != null && client.Connected) client.Close();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this._TcpListener.Stop();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SocketServer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
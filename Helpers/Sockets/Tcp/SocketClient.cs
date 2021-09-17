using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Helpers.Sockets.Tcp
{
    public class SocketClient : IDisposable
    {
        protected readonly TcpClient _TcpClient;
        protected readonly string _RemoteHost;
        protected readonly int _Port;
        private bool disposedValue;

        public SocketClient(string remoteHost, int port)
        {
            this._RemoteHost = remoteHost;
            this._Port = port;

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            this._TcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(this._RemoteHost), this._Port + 1));

            Console.WriteLine("The TcpClient configured...");

            //Task.Factory.StartNew(() => ListenToIsYourTimeSet());
        }

        public string Send(string message)
        {
            try
            {
                if (this._TcpClient == null || !this._TcpClient.Connected) this._TcpClient.Connect(this._RemoteHost, this._Port);

                NetworkStream networkStream = this._TcpClient.GetStream();

                // Send ----------------------------------------------------
                byte[] bytesToSend = Encoding.Unicode.GetBytes(message);
                networkStream.Write(bytesToSend, 0, bytesToSend.Length);
                Console.WriteLine($"[{message}] Sent to {this._TcpClient.Client.RemoteEndPoint}");

                // Receive -------------------------------------------------
                byte[] bufferReceived = new byte[this._TcpClient.ReceiveBufferSize];
                int bytesReceived = networkStream.Read(bufferReceived, 0, this._TcpClient.ReceiveBufferSize);
                string messageReceived = Encoding.Unicode.GetString(bufferReceived, 0, bytesReceived);
                Console.WriteLine($"[{message}:{messageReceived}] Received broadcast from {this._RemoteHost}:{this._Port} :");
                return messageReceived;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (this._TcpClient != null && this._TcpClient.Connected) this._TcpClient.Close();
                return null;
            }
            finally
            {
                //if (this._TcpClient != null && this._TcpClient.Connected) this._TcpClient.Close();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (this._TcpClient != null && this._TcpClient.Connected) this._TcpClient.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SocketClient()
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

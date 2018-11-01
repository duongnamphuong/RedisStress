using ProtocolUtil.Event;
using System;
using System.Net;
using System.Net.Sockets;

namespace ProtocolUtil
{
    public class UdpConnection : IDisposable
    {
        public event EventHandler<PacketDataSentEventArgs> DataSent;

        public event EventHandler<PacketDataReceivedEventArgs> DataReceived;

        private DestinationTuple dt;

        public Socket ConnectionSocket { get; set; }

        public bool Disposed
        {
            get
            {
                return disposed;
            }

            private set
            {
                disposed = value;
            }
        }

        // Constructor
        public UdpConnection()
        {
            try
            {
                dt = new DestinationTuple();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DestinationTuple destinationTuple
        {
            get { return dt; }
            set { dt = value; }
        }

        public void StartUdpListening(int port)
        {
            this.ConnectionSocket = SocketUtil.BindUDPConnection(new IPEndPoint(IPAddress.Any, port));
            this.StartReading();
        }

        // Send
        public void Send(byte[] byteStream, DestinationTuple destination)
        {
            PacketDataSentEventArgs e = new PacketDataSentEventArgs(byteStream, destination);
            try
            {
                this.ConnectionSocket.SendTo(byteStream, SocketFlags.None, (EndPoint)destination.RemoteEndPoint);
                this.OnDataSent(e);
            }
            catch (ObjectDisposedException exception)
            {
                throw exception;
            }
            catch (SocketException exception)
            {
                if (exception.SocketErrorCode == SocketError.WouldBlock)
                {
                    this.ConnectionSocket.BeginSendTo(byteStream, 0, byteStream.Length, SocketFlags.None, (EndPoint)destination.RemoteEndPoint, new AsyncCallback(this.SentCallback), e);
                }
                else
                {
                    throw exception;
                }
            }
        }

        private void OnDataSent(PacketDataSentEventArgs e)
        {
            if (this.DataSent != null)
            {
                this.DataSent(this, e);
            }
        }

        private void SentCallback(IAsyncResult result)
        {
            try
            {
                PacketDataSentEventArgs e = (PacketDataSentEventArgs)result.AsyncState;
                this.ConnectionSocket.EndSend(result);
                this.OnDataSent(e);
            }
            catch (Exception exception)
            {
            }
        }

        // Received
        private void OnDataReceived(PacketDataReceivedEventArgs e)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(this, e);
            }
        }

        private void ReceivedCallback(IAsyncResult result)
        {
            SocketData s = (SocketData)result.AsyncState;

            try
            {
                if (s.Socket == null)
                {
                    return;
                }
                int count = s.Socket.EndReceiveFrom(result, ref s.RemoteEndPoint);
                SocketData sd = s;
                sd.TotalReadSize = count;
                sd.Buffer = new byte[s.TotalReadSize];
                Array.Copy((Array)s.Buffer, (Array)sd.Buffer, sd.TotalReadSize);
                if (sd.TotalReadSize > 0)
                {
                    DestinationTuple d = new DestinationTuple((IPEndPoint)sd.Socket.LocalEndPoint, (IPEndPoint)sd.RemoteEndPoint);
                    PacketDataReceivedEventArgs e = new PacketDataReceivedEventArgs(sd.TotalReadSize, sd.Buffer, d);
                    this.OnDataReceived(e);
                }
            }
            catch (Exception exception)
            {
            }
            StartReading();
        }

        private void StartReading()
        {
            try
            {
                if (this.ConnectionSocket != null)
                {
                    SocketData s = new SocketData(this.ConnectionSocket, 4096);
                    this.ConnectionSocket.BeginReceiveFrom(s.Buffer, 0, s.Buffer.Length, SocketFlags.None, ref s.RemoteEndPoint, new AsyncCallback(ReceivedCallback), s);
                }
            }
            catch (Exception exception)
            {
            }
        }

        private void Disconnect()
        {
            try
            {
                if (this.ConnectionSocket == null)
                    return;
                this.ConnectionSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception exception)
            {
            }
        }

        ~UdpConnection()
        {
            this.Dispose(false);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                }
                Disposed = true;
                this.Disconnect();
                if (this.ConnectionSocket == null)
                {
                    return;
                }
                this.ConnectionSocket.Dispose();
                this.ConnectionSocket = (Socket)null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}

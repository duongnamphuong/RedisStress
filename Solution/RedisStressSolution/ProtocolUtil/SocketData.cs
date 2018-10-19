using System.Net;
using System.Net.Sockets;

namespace ProtocolUtil
{
    public struct SocketData
    {
        public byte[] Buffer;
        public int TotalReadSize;
        public EndPoint RemoteEndPoint;
        public Socket Socket;

        public SocketData(Socket socket, int maximumTransmissionUnit)
        {
            this.Buffer = new byte[maximumTransmissionUnit];
            this.TotalReadSize = maximumTransmissionUnit;
            this.Socket = socket;
            this.RemoteEndPoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
        }

        public SocketData(int bufferSize)
        {
            this.Buffer = new byte[bufferSize];
            this.TotalReadSize = bufferSize;
            this.Socket = (Socket)null;
            this.RemoteEndPoint = (EndPoint)null;
        }
    }
}

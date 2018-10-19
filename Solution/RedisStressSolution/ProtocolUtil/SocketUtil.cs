using System.Net;
using System.Net.Sockets;

namespace ProtocolUtil
{
    public class SocketUtil
    {
        public SocketUtil()
        {
        }

        public static Socket BindUDPConnection(IPEndPoint listenEndPoint)
        {
            try
            {
                Socket socket = new Socket(listenEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind((EndPoint)listenEndPoint);
                return socket;
            }
            catch (SocketException exception)
            {
                throw exception;
            }
        }

        public static Socket BindTCPConnection(IPEndPoint listenEndPoint)
        {
            try
            {
                Socket socket = new Socket(listenEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind((EndPoint)listenEndPoint);
                return socket;
            }
            catch (SocketException exception)
            {
                throw exception;
            }
        }
    }
}

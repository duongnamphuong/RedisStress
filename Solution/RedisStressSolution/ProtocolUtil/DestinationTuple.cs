using System;
using System.Net;

namespace ProtocolUtil
{
    public class DestinationTuple : EventArgs, IDisposable
    {
        public DestinationTuple()
        {
        }

        public DestinationTuple(IPEndPoint remoteEndPoint)
        {
            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException();
            }
            RemoteEndPoint = remoteEndPoint;
            RemoteIpAddress = RemoteEndPoint.Address.ToString();
            RemotePort = remoteEndPoint.Port;
        }

        public DestinationTuple(IPAddress remoteIpAddress, int remotePort)
        {
            if (remoteIpAddress == null || remotePort < 0)
            {
                throw new ArgumentNullException();
            }
            RemoteIpAddress = remoteIpAddress.ToString();
            RemotePort = remotePort;
            RemoteEndPoint = new IPEndPoint(remoteIpAddress, remotePort);
        }

        public DestinationTuple(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            if (localEndPoint == null || remoteEndPoint == null)
            {
                throw new ArgumentNullException();
            }
            RemoteEndPoint = remoteEndPoint;
            RemoteIpAddress = RemoteEndPoint.Address.ToString();
            RemotePort = remoteEndPoint.Port;
            LocalEndPoint = localEndPoint;
            LocalIpAddress = localEndPoint.Address.ToString();
            LocalPort = localEndPoint.Port;
        }

        public DestinationTuple(string remoteIpAddress, int remotePort)
        {
            if (string.IsNullOrEmpty(remoteIpAddress) || remotePort < 0)
            {
                throw new ArgumentNullException();
            }
            RemoteIpAddress = remoteIpAddress;
            RemotePort = remotePort;
            RemoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIpAddress), remotePort);
        }

        public IPEndPoint RemoteEndPoint { get; set; }
        public string RemoteIpAddress { get; private set; }
        public int RemotePort { get; private set; }
        public IPEndPoint LocalEndPoint { get; set; }
        public string LocalIpAddress { get; private set; }
        public int LocalPort { get; private set; }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

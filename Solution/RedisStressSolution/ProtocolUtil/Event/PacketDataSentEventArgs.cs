using System;

namespace ProtocolUtil.Event
{
    public class PacketDataSentEventArgs : EventArgs, IDisposable
    {
        public byte[] Data { get; private set; }
        public DestinationTuple Destination { get; private set; }

        internal PacketDataSentEventArgs(byte[] data, DestinationTuple recipient)
        {
            this.Destination = recipient;
            this.Data = data;
        }

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

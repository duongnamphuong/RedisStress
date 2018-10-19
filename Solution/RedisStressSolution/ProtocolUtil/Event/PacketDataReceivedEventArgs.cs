using ProtocolUtil.Utils;
using System;

namespace ProtocolUtil.Event
{
    public class PacketDataReceivedEventArgs : EventArgs, IDisposable
    {
        public int TotalBytesRead { get; set; }
        public byte[] BytesRead { get; set; }

        /// <summary>
        /// Byte stream displayed as string
        /// </summary>
        public string DataRead { get; set; }

        public DestinationTuple DestinationTuple { get; set; }

        //private static UTF8Encoding encoding = new UTF8Encoding();

        public PacketDataReceivedEventArgs(int bufferSize, byte[] dataBuffer, DestinationTuple d)
        {
            TotalBytesRead = bufferSize;
            BytesRead = new byte[bufferSize];
            Array.Copy(dataBuffer, 0, this.BytesRead, 0, bufferSize);
            DataRead = ByteStreamUtil.ByteToHexBit(BytesRead);
            //DataRead = encoding.GetString(BytesRead, 0, bufferSize);
            DestinationTuple = d;
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

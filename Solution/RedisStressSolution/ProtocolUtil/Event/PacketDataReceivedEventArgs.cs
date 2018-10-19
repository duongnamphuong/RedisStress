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
            DataRead = ByteToHexBit(BytesRead);
            //DataRead = encoding.GetString(BytesRead, 0, bufferSize);
            DestinationTuple = d;
        }

        // convert Byte array to string (Hex format)
        private string ByteToHexBit(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
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

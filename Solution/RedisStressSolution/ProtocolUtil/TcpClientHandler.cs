using LogUtil;
using ProtocolUtil.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ProtocolUtil
{
    internal class TcpClientHandler
    {
        private Socket _clientSocket = null;
        private Thread _clientListenerThread = null;
        private DateTime _lastReceiveDateTime;
        private DateTime _currentReceiveDateTime;
        private bool _stopClient = false;
        private bool _markedForDeletion = false;
        private string _clientSocketAddress;

        public TcpClientHandler(Socket clientSocket)
        {
            this._clientSocket = clientSocket;
            this._clientSocket.ReceiveBufferSize = 524288;
            this._clientSocketAddress = "Connection-" + IPAddress.Parse(((IPEndPoint)_clientSocket.RemoteEndPoint).Address.ToString()) + ":" + ((IPEndPoint)_clientSocket.RemoteEndPoint).Port.ToString() + ": ";
        }

        /// <summary>
        /// Handle incomming client connection
        /// </summary>
        public void StartSocketListener()
        {
            if (this._clientSocket != null)
            {
                _clientListenerThread = new Thread(new ThreadStart(StartConversation));
                _clientListenerThread.Start();
            }
        }

        private void StartConversation()
        {
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{_clientSocketAddress} Start");
            _lastReceiveDateTime = DateTime.UtcNow;
            _currentReceiveDateTime = DateTime.UtcNow;
            Timer t = new Timer(new TimerCallback(CheckClientCommInterval), null, 15000, 15000);
            int bytesRec = 0;

            byte[] dataBytes = new byte[4096];

            // An incoming connection needs to be processed.
            while (!_stopClient)
            {
                try
                {
                    if (!IsConnected())
                    {
                        break;
                    }
                    bytesRec = _clientSocket.Receive(dataBytes);
                    _currentReceiveDateTime = DateTime.UtcNow;
                    if (bytesRec > 0)
                    {
                        byte[] shrinkDataBytes = new byte[bytesRec];
                        Array.Copy(dataBytes, shrinkDataBytes, bytesRec);
                        Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{_clientSocketAddress} received {bytesRec} bytes: {ByteStreamUtil.ByteToHexBit(shrinkDataBytes)}");
                        byte[] answer = Encoding.ASCII.GetBytes("<Ack>").Concat(shrinkDataBytes).Concat(Encoding.ASCII.GetBytes("<\\Ack>")).ToArray();
                        SendMessage(answer);
                        Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{_clientSocketAddress} sent {answer.Count()} bytes: {ByteStreamUtil.ByteToHexBit(answer)}");
                    }
                }
                catch (Exception exception)
                {
                    Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, $"{_clientSocketAddress} exception {exception}");
                    _stopClient = true;
                    _markedForDeletion = true;
                }
            }
            _stopClient = true;
            _markedForDeletion = true;
            t.Change(Timeout.Infinite, Timeout.Infinite);
            t = null;
            Log4netLogger.Info(MethodBase.GetCurrentMethod().DeclaringType, $"{_clientSocketAddress} End");
        }

        private void CheckClientCommInterval(object o)
        {
            if (_lastReceiveDateTime.Equals(_currentReceiveDateTime))
            {
                this.StopSocketListener();
            }
            else
            {
                _lastReceiveDateTime = _currentReceiveDateTime;
            }
        }

        public void StopSocketListener()
        {
            if (_clientSocket != null)
            {
                _stopClient = true;
                _clientSocket.Close();

                if (_clientListenerThread != null)
                {
                    // Wait for one second for the the thread to stop.
                    _clientListenerThread.Join(1000);

                    // If still alive; Get rid of the thread.
                    if (_clientListenerThread.IsAlive)
                    {
                        _clientListenerThread.Abort();
                    }
                    _clientListenerThread = null;
                }
                _clientSocket = null;
                _markedForDeletion = true;
            }
        }

        public bool IsMarkedForDeletion()
        {
            return _markedForDeletion;
        }

        private bool IsConnected()
        {
            try
            {
                return !(this._clientSocket.Poll(1, SelectMode.SelectRead) && this._clientSocket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        private void SendMessage(byte[] messageBytes)
        {
            if (this._clientSocket != null)
            {
                byte[] messageLenBytes = System.BitConverter.GetBytes(messageBytes.Length);
                _clientSocket.Send(messageBytes);
            }
        }
    }
}

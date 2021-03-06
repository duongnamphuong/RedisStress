﻿using LogUtil;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace ProtocolUtil
{
    public class TcpConnection : IDisposable
    {
        public TcpListener ConnectionSocket { get; set; }
        private bool _stopServer = false;
        private bool _stopPurging = false;
        private Thread _serverThread = null;
        private Thread _purgingThread = null;
        private ArrayList _socketListenersList = null;

        public void StartListening(int port)
        {
            this.ConnectionSocket = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            this.ConnectionSocket.Start(1000);
            this.StartServer();
        }

        private void StartServer()
        {
            if (this.ConnectionSocket != null)
            {
                // Create a ArrayList for storing SocketListeners before
                // starting the server.
                _socketListenersList = new ArrayList();

                // Start the Server and start the thread to listen client
                // requests.
                _serverThread = new Thread(new ThreadStart(ServerThreadStart));
                _serverThread.Start();

                // Create a low priority thread that checks and deletes client
                // SocktConnection objects that are marked for deletion.
                _purgingThread = new Thread(new ThreadStart(PurgingThreadStart));
                _purgingThread.Priority = ThreadPriority.Lowest;
                _purgingThread.Start();
            }
        }

        private void ServerThreadStart()
        {
            while (!_stopServer)
            {
                try
                {
                    // Client Socket variable;
                    Socket clientSocket = null;
                    TcpClientHandler socketListener = null;

                    // Wait for any client requests and if there is any
                    // request from any client accept it (Wait indefinitely).
                    clientSocket = this.ConnectionSocket.AcceptSocket();

                    // Create a SocketListener object for the client.
                    socketListener = new TcpClientHandler(clientSocket);

                    // Add the socket listener to an array list in a thread
                    // safe fashon.
                    //Monitor.Enter(m_socketListenersList);
                    lock (_socketListenersList)
                    {
                        _socketListenersList.Add(socketListener);
                    }
                    //Monitor.Exit(m_socketListenersList);

                    // Start a communicating with the client in a different
                    // thread.
                    socketListener.StartSocketListener();
                }
                catch (Exception)
                {
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void PurgingThreadStart()
        {
            while (!_stopPurging)
            {
                ArrayList deleteList = new ArrayList();

                // Check for any clients SocketListeners that are to be
                // deleted and put them in a separate list in a thread sage
                // fashon.
                //Monitor.Enter(m_socketListenersList);
                lock (_socketListenersList)
                {
                    foreach (TcpClientHandler socketListener
                                 in _socketListenersList)
                    {
                        if (socketListener.IsMarkedForDeletion())
                        {
                            deleteList.Add(socketListener);
                            try
                            {
                                socketListener.StopSocketListener();
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }

                    // Delete all the client SocketConnection ojects which are
                    // in marked for deletion and are in the delete list.
                    for (int i = 0; i < deleteList.Count; ++i)
                    {
                        _socketListenersList.Remove(deleteList[i]);
                    }
                }

                deleteList = null;
                Thread.Sleep(10000);
            }
        }

        ~TcpConnection()
        {
            this.Dispose(false);
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
                this.Disconnect();
                if (this.ConnectionSocket != null)
                {
                    return;
                }
                this.Dispose();
                this.ConnectionSocket = (TcpListener)null;
            }
        }

        private void Disconnect()
        {
            try
            {
                if (this.ConnectionSocket == null)
                    return;
                this.StopServer();
            }
            catch (Exception exception)
            {
                Log4netLogger.Error(MethodBase.GetCurrentMethod().DeclaringType, exception);
            }
        }

        public void StopServer()
        {
            if (this.ConnectionSocket != null)
            {
                // It is important to Stop the server first before doing
                // any cleanup. If not so, clients might being added as
                // server is running, but supporting data structures
                // (such as m_socketListenersList) are cleared. This might
                // cause exceptions.

                // Stop the TCP/IP Server.
                _stopServer = true;
                this.ConnectionSocket.Stop();

                // Wait for one second for the the thread to stop.
                _serverThread.Join(1000);

                // If still alive; Get rid of the thread.
                if (_serverThread.IsAlive)
                {
                    _serverThread.Abort();
                }
                _serverThread = null;

                _stopPurging = true;
                _purgingThread.Join(1000);
                if (_purgingThread.IsAlive)
                {
                    _purgingThread.Abort();
                }
                _purgingThread = null;

                // Free Server Object.
                this.ConnectionSocket = null;

                // Stop All clients.
                //StopAllSocketListers();
            }
        }
    }
}

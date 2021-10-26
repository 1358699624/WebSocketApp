using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocketApp
{
    public class WebSocketService : IDisposable
    {
        public static NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

        #region 向外传递数据事件
        public event Action<WebSocketSession, string> MessageReceived;
        public event Action<WebSocketSession> NewConnected;
        public event Action<WebSocketSession> Closed;
        #endregion

        public WebSocketServer WebSocket;

        Thread _thread;
        bool _isRunning = true;


        public WebSocketService()
        {
        }

        #region WebSockertServer
        /// <summary>
        /// 开启服务端
        /// </summary>
        /// <param name="port"></param>
        /// <param name="serverName"></param>
        /// <param name="isUseCertificate"></param>
        /// <param name="serverStoreName"></param>
        /// <param name="serverSecurity"></param>
        /// <param name="serverThumbprint"></param>
        /// <returns></returns>
        public bool Open(int port, string serverName, bool isUseCertificate = false, string serverStoreName = "", string serverSecurity = "", string serverThumbprint = "")
        {
            bool isSetuped = false;
            try
            {
                this.WebSocket = new WebSocketServer();
                var serverConfig = new ServerConfig
                {
                    Name = serverName,
                    MaxConnectionNumber = 10000, //最大允许的客户端连接数目，默认为100。
                    Mode = SocketMode.Tcp,
                    Port = port, //服务器监听的端口。
                    ClearIdleSession = false,   //true或者false， 是否清除空闲会话，默认为false。
                    ClearIdleSessionInterval = 120,//清除空闲会话的时间间隔，默认为120，单位为秒。
                    ListenBacklog = 10,
                    ReceiveBufferSize = 64 * 1024, //用于接收数据的缓冲区大小，默认为2048。
                    SendBufferSize = 64 * 1024,   //用户发送数据的缓冲区大小，默认为2048。
                    KeepAliveInterval = 1,     //keep alive消息发送时间间隔。单位为秒。
                    KeepAliveTime = 60,    //keep alive失败重试的时间间隔。单位为秒。
                    SyncSend = false
                };
                SocketServerFactory socketServerFactory = null;
                //开启wss 使用证书
                if (isUseCertificate)
                {
                    serverConfig.Security = serverSecurity;
                    serverConfig.Certificate = new SuperSocket.SocketBase.Config.CertificateConfig
                    {
                        StoreName = serverStoreName,
                        StoreLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine,
                        Thumbprint = serverThumbprint
                    };
                    socketServerFactory = new SocketServerFactory();
                }
                isSetuped = this.WebSocket.Setup(new RootConfig(), serverConfig, socketServerFactory);
                if (isSetuped)
                {
                    _Logger.Info("Setup Success...");
                }
                else
                {
                    _Logger.Error("Failed to setup!");
                }
                this.WebSocket.NewSessionConnected += NewSessionConnected;
                this.WebSocket.NewMessageReceived += NewMessageReceived;
                this.WebSocket.SessionClosed += SessionClosed;
                isSetuped = this.WebSocket.Start();
                if (isSetuped)
                {
                    _Logger.Info("Start Success...");
                    _Logger.Info("Server Listen at " + this.WebSocket.Listeners[0].EndPoint.Port.ToString());
                    this._isRunning = true;
                    this._thread = new Thread(new ThreadStart(ProcessMaintainance));
                    this._thread.Start();
                }
                else
                {
                    _Logger.Error("Failed to start!");
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.ToString());
            }
            return isSetuped;
        }
        /// <summary>
        /// 消息触发事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        void NewMessageReceived(WebSocketSession session, string value)
        {
            try
            {
                _Logger.Info("Receive:" + value.ToString() + " ClientIP:" + session.RemoteEndPoint);
                if (value.ToString().Equals("IsHere**"))//客户端定时发送心跳，维持链接
                {
                    return;
                }
                else
                {
                    MessageReceived?.Invoke(session, value.ToString());
                }
            }
            catch (Exception e)
            {
                _Logger.Error(e.ToString());
            }
        }
        /// <summary>
        /// 新链接触发事件
        /// </summary>
        /// <param name="session"></param>
        void NewSessionConnected(WebSocketSession session)
        {
            try
            {
                string message = string.Format("New Session Connected:{0}, Path:{1}, Host:{2}, IP:{3}",
                    session.SessionID.ToString(), session.Path, session.Host, session.RemoteEndPoint);
                _Logger.Info(message);
                NewConnected?.Invoke(session);

            }
            catch (Exception e)
            {
                _Logger.Error(e.ToString());
            }
        }
        /// <summary>
        /// 客户端链接关闭触发事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        void SessionClosed(WebSocketSession session, CloseReason value)
        {
            string message = string.Format("Session Close:{0}, Path:{1}, IP:{2}", value.ToString(), session.Path, session.RemoteEndPoint);
            _Logger.Info(message);
            Closed?.Invoke(session);
        }

        #endregion
        /// <summary>
        /// 关闭服务端触发事件
        /// </summary>
        public void Dispose()
        {
            this._isRunning = false;
            foreach (WebSocketSession session in this.WebSocket.GetAllSessions())
            {
                session.Close();
            }
            try
            {
                this.WebSocket.Stop();
            }
            catch { }
        }
        /// <summary>
        /// 输出实时连接线程
        /// </summary>
        void ProcessMaintainance()
        {
            do
            {
                try
                {
                    _Logger.Debug("Display Session Info:" + this.WebSocket.SessionCount);
                    foreach (WebSocketSession session in this.WebSocket.GetAllSessions())
                    {
                        string message = string.Format("ID:{0}, Remote:{1}, Path:{2}, LastActiveTime:{3}, StartTime:{4}",
                            session.SessionID, session.RemoteEndPoint, session.Path
                          , session.LastActiveTime, session.StartTime);
                        _Logger.Debug(message);
                    }
                }
                catch (Exception e)
                {
                    _Logger.Error(e.ToString());
                }
                System.Threading.Thread.Sleep(5 * 60000);
            } while (this._isRunning);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="session">客户端连接</param>
        /// <param name="message">消息内容</param>

        public void SendMessage(WebSocketSession session, string message)
        {
            Task.Factory.StartNew(() => { if (session != null && session.Connected) session.Send(message); });
        }


    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Fleck;
using NLog;

namespace ProtocolApp
{
    class MessageCall
    {
        public WebSocketMessageType type;
        public byte[] binary;
        public object message;
        public IWebSocketConnection client;
    }
    internal class WebSocketService
    {
        static WebSocketServer server = null;
        static public void Start(Action<MessageCall> action, string Port = "12345")
        {
            try
            {
                server = new WebSocketServer("ws://127.0.0.1:" + Port);
                server.Start(socket =>
                {
                    socket.OnOpen = () => Logger.Log.Info("WebSocket Open!");
                    socket.OnClose = () => Logger.Log.Info("WebSocket Close!");
                    socket.OnError = (err) => Logger.Log.Info("WebSocket Error: " + err.Message);
                    socket.OnMessage = (message => {
                        object msg;
                        try
                        {
                            msg = new JavaScriptSerializer().DeserializeObject(message);
                        }
                        catch (Exception)
                        {
                            msg = message;
                        }
                        action(new MessageCall()
                        {
                            type = WebSocketMessageType.Text,
                            message = msg,
                            client = socket,
                        });
                    });
                    socket.OnBinary = (message => {
                        action(new MessageCall()
                        {
                            type = WebSocketMessageType.Binary,
                            binary = message,
                            client = socket,
                        });
                    });
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Create Websocket Server Failed, {0}", ex.Message);

            }
        }


        static public void Stop()
        {
            server.Dispose();
        }
    }
}

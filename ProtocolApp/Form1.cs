using Fleck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace ProtocolApp
{
    public partial class Form1 : Form
    {
        IWebSocketConnection wsClient;
        Action<byte[]> binaryExecute = null;

        public Form1()
        {
            InitializeComponent();
            ServiceStart(12345);
            SetTitle();
            ViewRoute();
        }

        void SetTitle()
        {
            string routeName = "Untitled";
            if (!string.IsNullOrEmpty(Program.route.name)) 
            { 
                routeName = Program.route.name;
            }
            Text = routeName;
        }

        void ViewRoute()
        {
            if (Program.route.param.Keys.Contains("id")) 
                text_id.Text = Program.route.param["id"];

            if (Program.route.param.Keys.Contains("key"))
                text_key.Text = Program.route.param["key"];

            text_query.Text = new JavaScriptSerializer().Serialize(Program.route.query);
        }

        private void ServiceStart(int Port)
        {
            Logger.Log.Info("Start WebSocket in Port " + Port);
            WebSocketService.Start(call =>
            {
                try
                {
                    wsClient = call.client;
                    if (call.type == WebSocketMessageType.Text)
                    {
                        MessageExecute((Dictionary<string, object>)call.message);
                    }

                    if (call.type == WebSocketMessageType.Binary && binaryExecute != null)
                    {
                        binaryExecute(call.binary);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Fatal("Websocket Message Error: " + ex.Message);
                    throw ex;
                }
            });
        }

        private void SendData(string command, object data)
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("command", command); 
            msg.Add("data", data);
            wsClient.Send(new JavaScriptSerializer().Serialize(msg));
        }

        private void MessageExecute(Dictionary<string, object> message)
        {
            string command = message["command"] as string;
            switch(command)
            {
                case "Ping":
                    SendData("Ping", "Pong");
                    break;
                case "Save":
                    savePath = message["data"] as string;
                    binaryExecute = SaveFile;
                    break;
                case "Close":
                    Application.Exit();
                    break;
            }
        }

        string savePath = "";
        void SaveFile(byte[] data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            using (FileStream file = new FileStream(savePath, FileMode.Create))
            {
                file.Write(data, 0, data.Length);
            }
        }
    }
}

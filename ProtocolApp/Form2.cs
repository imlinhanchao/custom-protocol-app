using Fleck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ProtocolApp
{
    public partial class Form2 : Form
    {
        IWebSocketConnection wsClient;
        public Form2()
        {
            InitializeComponent();
            this.Visible = false;
            var query = Program.route.query;
            var port = query.Keys.Contains("port") ? Convert.ToInt32(query["port"]) : 12345;
            ServiceStart(port);            
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
                }
                catch (Exception ex)
                {
                    Logger.Log.Fatal("Websocket Message Error: " + ex.Message);
                    throw ex;
                }
            }, Port.ToString());
        }

        private void SendData(string command, object data)
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("command", command);
            msg.Add("data", data);
            wsClient?.Send(new JavaScriptSerializer().Serialize(msg));
        }

        private void MessageExecute(Dictionary<string, object> message)
        {
            string command = message["command"] as string;
            object data = message.Keys.Contains("data") ? message["data"] : null;
            switch (command)
            {
                case "Ping":
                    SendData("Ping", "Pong");
                    break;
                case "Download":
                    Download((data as Dictionary<string, object>)["url"] as string, (data as Dictionary<string, object>)["local"] as string);
                    break;
                case "Close":
                    Application.Exit();
                    break;
            }
        }

        async void Download(string url, string localDir)
        {
            string local = Path.Combine(localDir, Path.GetFileName(url));
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        // 检查是否支持获取文件大小
                        long? totalBytes = response.Content.Headers.ContentLength;

                        // 使用 FileStream 进行文件写入，注意使用 FileOptions.Asynchronous 以便异步写入
                        using (FileStream fileStream = new FileStream(local, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            byte[] buffer = new byte[8192];
                            int bytesRead;
                            long bytesDownloaded = 0;

                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);

                                bytesDownloaded += bytesRead;

                                // 如果支持获取文件大小，则输出下载进度
                                if (totalBytes.HasValue)
                                {
                                    int percentage = (int)((bytesDownloaded * 100) / totalBytes.Value);
                                    SendData("Progress", percentage);
                                }
                            }

                            Console.WriteLine("File downloaded successfully!");
                            SendData("Progress", 100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendData("Error", ex.Message);
                    Console.WriteLine("Error downloading file: " + ex.Message);
                }
            }
        }
    }
}

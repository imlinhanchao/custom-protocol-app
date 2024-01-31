using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtocolApp
{
    internal static class Program
    {
        public static Route route { get => _route; }
        static Route _route = new Route();
        static RouteMeta[] meta = new RouteMeta[] {
            new RouteMeta("Form1", "/form1/:id/:key", new Dictionary<string, object> {
                { "title", "Custom Title" },
            }),
            new RouteMeta("Form2", "/form2", new Dictionary<string, object>())
        };
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] args = Environment.GetCommandLineArgs();
            Logger.Init(AppDomain.CurrentDomain.BaseDirectory + "application.log");

            if (args.Length > 1) {
                _route = Route.Format(args[1], meta);
            }

            switch (_route.name)
            {
                case "Form2":
                    Application.Run(new Form2());
                    break;
                case "Form1":
                default:
                    Application.Run(new Form1());
                    break;
            }
        }
    }
}

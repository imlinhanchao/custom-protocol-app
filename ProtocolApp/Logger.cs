using NLog.Config;
using NLog.Targets;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtocolApp
{
    internal class Logger
    {
        static NLog.Logger _logger;
        static public void Init(string LogPath)
        {
            LoggingConfiguration config = new LoggingConfiguration();

            FileTarget fileTarget = new FileTarget(Application.ProductName);
            fileTarget.FileName = LogPath;
            fileTarget.Layout = "[${level}] <${longdate}> ${message}";

            config.AddTarget(fileTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }

        static public NLog.Logger Log { get => _logger; }
    }
}

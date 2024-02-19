using System.Configuration;
using WebSocketSharp.Server;

namespace SSARMiddlware
{
    internal static class SoketHelper
    {
        public static WebSocketServer WebSocketServer;

        public static void Initialize()
        {
            string ip = ConfigurationManager.AppSettings["ip"];
            string port = ConfigurationManager.AppSettings["port"];
            string url = ip + ":" + port;
            WebSocketServer = new WebSocketServer(url);
        }

        public static void Start()
        {
            WebSocketServer.Start();
        }

        public static void Stop()
        {
            WebSocketServer.Stop();
        }
    }
}

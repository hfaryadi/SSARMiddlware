using System.Web.Configuration;
using WebSocketSharp.Server;

namespace SSARMiddlware
{
    internal static class SoketHelper
    {
        public static WebSocketServer WebSocketServer;

        public static void Initialize()
        {
            string ip = WebConfigurationManager.AppSettings["ip"];
            string port = WebConfigurationManager.AppSettings["port"];
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

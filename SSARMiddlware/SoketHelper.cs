using WebSocketSharp.Server;

namespace SSARMiddlware
{
    internal static class SoketHelper
    {
        public static WebSocketServer ws;
        private static string _url = "ws://127.0.0.1:8080";

        public static void Initialize(string url = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                _url = url;
            }
            ws = new WebSocketServer(_url);
        }

        public static void Start()
        {
            ws.Start();
        }

        public static void Stop()
        {
            ws.Stop();
        }
    }
}

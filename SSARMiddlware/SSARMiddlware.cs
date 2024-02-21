using SSARMiddlware.Services;
using System.ServiceProcess;

namespace SSARMiddlware
{
    partial class SSARMiddlware : ServiceBase
    {
        public SSARMiddlware()
        {
            InitializeComponent();
        }

        public void onDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            SoketHelper.Initialize();
            AddServices();
            SoketHelper.Start();
        }

        protected override void OnStop()
        {
            SoketHelper.Stop();
        }

        private void AddServices()
        {
            SoketHelper.WebSocketServer.AddWebSocketService<PozService>("/Poz");
            SoketHelper.WebSocketServer.AddWebSocketService<ScannerService>("/Scanner");
        }
    }
}

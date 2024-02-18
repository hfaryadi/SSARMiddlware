using SSARMiddlware.Services;
using System.ServiceProcess;

namespace SSARMiddlware
{
    partial class SSARMiddlware : ServiceBase
    {
        public SSARMiddlware()
        {
            InitializeComponent();
            SoketHelper.Initialize("ws://127.0.0.1:7998");
            AddServices();
        }

        public void onDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            SoketHelper.Start();
        }

        protected override void OnStop()
        {
            SoketHelper.Stop();
        }

        private void AddServices()
        {
            SoketHelper.ws.AddWebSocketService<PaymentService>("/Payment");
        }
    }
}

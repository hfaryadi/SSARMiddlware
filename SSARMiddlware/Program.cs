using System.ServiceProcess;

namespace SSARMiddlware
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            #if DEBUG
                SSARMiddlware ssarMiddlware = new SSARMiddlware();
                ssarMiddlware.onDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new SSARMiddlware()
                };
                ServiceBase.Run(ServicesToRun);
            #endif
        }
    }
}

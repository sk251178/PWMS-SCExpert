using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace PicklistBreak
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
          
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new PicklistBreak() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}
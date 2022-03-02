using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System;

namespace SCExpertConnect
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new SCExpertConnect() };

            ServiceBase.Run(ServicesToRun);


            // To start the connect framework as a window application unComment the following lines
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SCExpertConnectWinApp());
        }
    }
}
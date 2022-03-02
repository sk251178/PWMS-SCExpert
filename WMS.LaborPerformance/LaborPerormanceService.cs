using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace WMS.LaborPerformance
{
    public partial class LaborPerformanceService : ServiceBase
    {

        private LaborPerformanceProc LPO;
        private System.ComponentModel.Container components = null;

        public LaborPerformanceService()
        {
            InitializeComponent();
        }


        // The main entry point for the process
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new LaborPerformanceService(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new LaborPerformanceService() };

            ServiceBase.Run(ServicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            //System.Diagnostics.Debugger.Break();
            Connect();
            LPO = new LaborPerformanceProc();
            LPO.StartQueue();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            DisConnect();
            LPO.StopQueue();
        }


        /// <summary> 
        /// Required designer variable.
        /// </summary>

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "LaborPerformance";
        }

        #endregion

        #region "License Management"

        private bool Connect()
        {
            bool ret = false;
            string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
            ret = Made4Net.DataAccess.DataInterface.Connect(licUser);
            return ret;
        }
        private bool DisConnect()
        {
            bool ret = false;
            string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
            ret = Made4Net.DataAccess.DataInterface.Disconnect(licUser);
            return ret;
        }
        #endregion
    }
}

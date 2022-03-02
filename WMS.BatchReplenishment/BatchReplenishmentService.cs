using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using Made4Net.Shared;
using Made4Net.General;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End

namespace WMS.BatchReplenishment
{
	public class BatchReplenishmentService : System.ServiceProcess.ServiceBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected BatchReplenishment rl = null;

		public BatchReplenishmentService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

        // The main entry point for the process
        private static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;

			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new BatchReplenishmentService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);


		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

			components = new System.ComponentModel.Container();
			this.ServiceName = "Expert Replenishment Service";
            string logPath  = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.ApplicationLogDirectory);
            logPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(logPath);

            string configPath = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.XmlConfigPath);
            configPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(configPath);

            LogConfiguration.InitializeDiagnostics(configPath);
            ExceptionConfiguration.BuildExceptionConfiguration(logPath, string.Empty, string.Empty, null);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			bool Connected = Connect();
			if(!Connected)
				throw new ApplicationException("License Not Found!");
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.Replenishment);
            //Commented for RWMS-1185 End

			rl = new BatchReplenishment();
			rl.StartQueue();
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
            //Commented for RWMS-1185 Start
		    //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 Start

			DisConnect();
			rl.StopQueue();
		}

		#region "License Management"

		private bool Connect()
		{
			bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.BatchReplanishmnetServiceSection,
                    Made4Net.Shared.ConfigurationSettingsConsts.BatchReplanishmnetServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Connect(licUser);
			return ret;
		}
		private bool DisConnect()
		{
			bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.BatchReplanishmnetServiceSection,
                     Made4Net.Shared.ConfigurationSettingsConsts.BatchReplanishmnetServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Disconnect(licUser);
			return ret;
		}
		#endregion


	}
}
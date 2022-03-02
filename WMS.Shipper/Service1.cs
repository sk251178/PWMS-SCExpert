using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End

namespace WMS.Shipper
{
    public partial class Service1 : ServiceBase
    {
        protected Shipper oShipper = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Threading.Thread.Sleep(20000);
            bool Connected = Connect();
            if (!Connected)
                throw new ApplicationException("License Not Found!");

            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.Shipper);
            //Commented for RWMS-1185 Start

            oShipper = new Shipper();
            oShipper.StartQueue();
        }

        protected override void OnStop()
        {
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 End

            DisConnect();
            oShipper.StopQueue();
        }

        #region "License Management"

        private bool Connect()
        {
            bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceSection,
                         Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Connect(licUser);
            return ret;
        }
        private bool DisConnect()
        {
            bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceSection,
                         Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Disconnect(licUser);
            return ret;
        }
        #endregion
    }
}
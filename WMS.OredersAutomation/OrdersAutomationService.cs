using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using WMS.Lib;
using WMS.Logic;
using Made4Net.DataAccess;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End

namespace WMS.OredersAutomation
{
    public partial class OrdersAutomationService : ServiceBase
    {
        private OrdersAutomation OA;

        public OrdersAutomationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            bool Connected = Connect();
            if (!Connected)
                throw new ApplicationException("License Not Found!");

            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.OrdersAutomation);
            //Commented for RWMS-1185 End

            OA = new OrdersAutomation();
            OA.StartQueue();
        }

        protected override void OnStop()
        {
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 End

            DisConnect();
            OA.StopQueue();
        }

        #region "License Management"

        private bool Connect()
        {
            bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceSection,
                    Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Connect(licUser);
            return ret;
        }
        private bool DisConnect()
        {
            bool ret = false;
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            string licUser = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceSection,
                         Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceLicenseUserId); // PWMS-817
            ret = true; // Made4Net.DataAccess.DataInterface.Disconnect(licUser);
            return ret;
        }
        #endregion
    }
}
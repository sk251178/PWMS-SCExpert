using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End


namespace WMS.InventoryTransaction
{
    public partial class InventoryTransactionService : ServiceBase
    {
        #region Fields

        private string LicenseUserId = string.Empty;
        private Handler aq = null;

        #endregion

        #region Constructors

        public InventoryTransactionService()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            if (!Connect()) throw new System.ApplicationException("No License found, please check Made4Net License Service and start the service again.");

            try
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.InventoryTransaction);
                //Commented for RWMS-1185 End

                aq = new Handler(); //PWMS-817
                aq.StartQueue();
            }
            catch (Exception ex)
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.StopMonitor();
                //Commented for RWMS-1185 End

                // If there was Error in Q creating then release licence and throw Exception
                Disconnect();
                throw ex;
            }
        }

        protected override void OnStop()
        {
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 Start

            // TODO: Add code here to perform any tear-down necessary to stop your service.
            aq.StopQueue();
            Disconnect();
        }

        #endregion

        #region License Methods

        protected bool Connect()
        {
            bool res = false;
            //LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceSection,
                                        Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceLicenseUserId); // PWMS-817

            try
            {
                res = true; // Made4Net.DataAccess.DataInterface.Connect(LicenseUserId);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return res;
        }

        protected bool Disconnect()
        {
            try
            {
                //LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
                LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceSection,
                             Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceLicenseUserId); // PWMS-817
                //Made4Net.DataAccess.DataInterface.Disconnect(LicenseUserId);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        #endregion
    }
}
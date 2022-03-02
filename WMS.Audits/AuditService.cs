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

namespace WMS.Audits
{
    public partial class AuditService : ServiceBase
    {
        #region Fields

        private string LicenseUserId = string.Empty;
        private Handler aq = null;

        #endregion

        #region Constructors

        public AuditService()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        protected override void OnStart(string[] args)
        {
            if (!Connect()) throw new System.ApplicationException("No License found, please check Made4Net License Service and start the service again.");

            try
            {
                aq = new Handler(); //PWMS-817
                aq.StartQueue();
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        protected override void OnStop()
        {
            aq.StopQueue();
            Disconnect();
        }

        #endregion

        #region License Methods

        protected bool Connect()
        {
            bool res = false;
            //LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null); //PWMS-817
            LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.AuditServiceServiceConfigSection,
                                         Made4Net.Shared.ConfigurationSettingsConsts.AuditServiceServiceLicenseUserId); // PWMS-817
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
                LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.AuditServiceServiceConfigSection,
                                         Made4Net.Shared.ConfigurationSettingsConsts.AuditServiceServiceLicenseUserId); // PWMS-817
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
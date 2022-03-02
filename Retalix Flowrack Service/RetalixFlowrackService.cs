using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Messaging;

namespace RetalixFlowrackService
{
    public partial class RetalixFlowrackService : ServiceBase
    {
        #region Fields

        private string LicenseUserId = string.Empty;
        private Handler aq = null;

        #endregion

        #region Constructors

        public RetalixFlowrackService()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        protected override void OnStart(string[] args)
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);

            // TODO: Add code here to start your service.
            //if (!Connect()) throw new System.ApplicationException("No License found, please check Made4Net License Service and start the service again.");

            try
            {
               // string MessageQ = Made4Net.Shared.AppConfig.Get("MessageQ", null);
                //System.Messaging.MessageQueue oQ = new System.Messaging.MessageQueue();
                //if (!System.Messaging.MessageQueue.Exists(@".\Private$\" + MessageQ))
                //{

                //    oQ = System.Messaging.MessageQueue.Create(@".\Private$\" + MessageQ);
                //    oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                //    oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                //}
                //{
                //    oQ = new System.Messaging.MessageQueue(@".\Private$\" + MessageQ);
                //    oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl);
                //    oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                //}
                //aq = new Handler( ConfigurationSettings.AppSettings.Get("MessageQ") ); --> what a douchebag!!!
                aq = new Handler(); //PWMS-817
                aq.StartQueue();
            }
            catch (Exception ex)
            {
                // If there was Error in Q creating then release licence and throw Exception
                //Disconnect();
                throw ex;
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            aq.StopQueue();
            //Disconnect();
        }

        #endregion

        #region License Methods

        protected bool Connect()
        {
            bool res = false;
        //    LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
            LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackSection,
                                                     Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackLicenseUserId); // PWMS-817
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
                //LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
                LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackSection,
                                                     Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackLicenseUserId); // PWMS-817
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
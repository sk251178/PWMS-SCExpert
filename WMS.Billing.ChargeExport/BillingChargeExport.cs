using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Messaging;

namespace WMS.Billing.ChargeExport
{
    public partial class BillingChargeExport : ServiceBase
    {
        #region Fields

        //private string LicenseUserId = string.Empty;
        private Handler aq = null;
 
        #endregion

        #region Constructors

        public BillingChargeExport()
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
                //string MessageQ = Made4Net.Shared.AppConfig.Get("MessageQ", null); //PWMS-817
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
                //aq = new Handler(MessageQ); 
                aq = new Handler();  //PWMS-817
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

        //protected bool Connect()
        //{
        //    bool res = false;
        //LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
        //try
        //{
        //    res = Made4Net.DataAccess.DataInterface.Connect(LicenseUserId);
        //}
        //catch (Exception ex)
        //{
        //    ex.ToString();
        //}
        //    return res;
        //}

        //protected bool Disconnect()
        //{
        //    try
        //    {
        //        LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
        //        Made4Net.DataAccess.DataInterface.Disconnect(LicenseUserId);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        return false;
        //    }
        //}

        #endregion
    }
}

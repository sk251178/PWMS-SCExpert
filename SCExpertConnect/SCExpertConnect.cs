using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using SCExpertConnectPlugins.BO;
using System.Xml;
using Made4Net.Shared;
using Made4Net.DataAccess;
using ExpertObjectMapper;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End

namespace SCExpertConnect
{
    public partial class SCExpertConnect : ServiceBase
    {
        #region Members

        protected string mLicensUser = "SCExpert Connect Service";
        protected Thread ImporterThread;
        protected SCExpertConnectExporter mExporter = null;

        #endregion

        #region Service Handlers

        public SCExpertConnect()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //RWMS-1435 Start
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
            DirPath = DirPath + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectAttachmentPath;
            bool exists = System.IO.Directory.Exists(DirPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(DirPath);
            //RWMS-1435 End
            //System.Threading.Thread.Sleep(20000);
            if (!Connect())
                throw new ApplicationException("License Not Found - Service will be terminated!");
            try
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.SCExpertConnect);
                //Commented for RWMS-1185 End

                ImporterThread = new Thread(InitPlugins);
                ImporterThread.Start();
                mExporter = new SCExpertConnectExporter();
                mExporter.StartQueue();
            }
            catch (Exception ex)
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.StopMonitor();
                //Commented for RWMS-1185 End

                if (mExporter != null)
                    mExporter.StopQueue();
                DisConnect();
                throw ex;
            }
        }

        protected override void OnStop()
        {
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 End

            DisConnect();
            mExporter.StopQueue();
        }

        #endregion

        #region ImportPlugins

        private void InitPlugins()
        {
            SCExpertConnectImporter oImporter = new SCExpertConnectImporter();
            oImporter.InitPlugins();
        }

        #endregion

        #region License Methods

        protected bool Connect()
        {
            return true;// Made4Net.DataAccess.DataInterface.Connect(mLicensUser);
        }

        protected bool DisConnect()
        {
            return true;// Made4Net.DataAccess.DataInterface.Disconnect(mLicensUser);
        }

        #endregion
    }
}
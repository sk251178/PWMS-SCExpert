using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Made4Net.Shared.Web;
using System.Reflection;
using System.Threading;

namespace PWMSService
{
    public partial class PWMSService : ServiceBase
    {
        private Made4Net.Shared.Logging.LogFile mPWMSService = null;
        private string LicenseUserId = string.Empty;
        public PWMSService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            string mAssemblyName, mClassName,MethodName;
            System.Diagnostics.Debugger.Launch();
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath(true);
            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.PWMSServiceLogDirectory;

            mPWMSService = new Made4Net.Shared.Logging.LogFile(DirPath, "PWMSService", true);
            mPWMSService.WriteLine("Service Started", true);

            DataTable dt = GetAssemplyinformation();
            if (dt.Rows.Count > 0)
            {
                //based on type spawn thread or process.
                ThreadSpawn(dt); 
                
            }
            else
            {
                throw new ApplicationException("No assembly information found"); 
            }
            
        }
        protected void ProcessSpawn()
        {
            Process myProcess = Process.GetCurrentProcess();
        }
        protected void ThreadSpawn(DataTable objDatatable)
        {
            //TODO: Create a main process and span threads from it
            ThreadProcessing obj = new ThreadProcessing(objDatatable);
            ThreadStart ts = new ThreadStart(obj.ExecuteMetod);
            Thread thread = new Thread(ts);
            thread.Start();
            Thread.Sleep(100);
            obj.requestStop();

           

        }

        public static void ExecuteMethod(DataTable objDatatable)
        {

        }

        protected override void OnStop()
        {
        }

        #region License Methods

        protected bool Connect()
        {
            bool res = false;
            LicenseUserId = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackSection,
                                                     Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackLicenseUserId, true); 
            try
            {
                res = Made4Net.DataAccess.DataInterface.Connect(LicenseUserId);
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
                                                     Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackLicenseUserId, true); // PWMS-817
                Made4Net.DataAccess.DataInterface.Disconnect(LicenseUserId);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        #endregion

        protected DataTable GetAssemplyinformation()
        {
            DataTable dt = new DataTable();;
            try
            {
                string sql = "select * from PROCESS_CONTROL WHERE Enable = 1";               
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, false, "default");                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString() );
            }
            return dt;
        }

    }
}

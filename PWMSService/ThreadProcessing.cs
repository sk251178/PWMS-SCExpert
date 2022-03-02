using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace PWMSService
{
    public class ThreadProcessing
    {
        DataTable _mobjDataTable = null;
        private volatile bool _shouldStop;
        private Made4Net.Shared.Logging.LogFile mPWMSServiceThread = null;

        public ThreadProcessing(DataTable objDataTable)
        {
            _mobjDataTable = objDataTable;
        }

        public void ExecuteMetod()
        {
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath(true);
            DirPath += "\\Logs\\AsyncQueueWorker"; // +Made4Net.Shared.ConfigurationSettingsConsts.PWMSServiceLogDirectory;
            while (!_shouldStop)
            {
                string mAssemblyName, mClassName, MethodName;

                mAssemblyName = _mobjDataTable.Rows[0]["AssemblyName"].ToString();
                mClassName = _mobjDataTable.Rows[0]["ClassName"].ToString();
                MethodName = _mobjDataTable.Rows[0]["MethodName"].ToString();

                string assemblyName = System.IO.Path.GetFileName(mAssemblyName);
                //string assemblyPath = System.IO.Path.GetDirectoryName(mAssemblyName);
                string assemblyPath = Made4Net.DataAccess.Util.GetInstancePath(true);
                //string FolderName = Made4Net.DataAccess.Util.GetAppConfigNameValue("ServiceFolderName",true);
                assemblyPath = assemblyPath + "\\Services";
                //the arguments should be the constractor parameters 

                Type objectType = Made4Net.Shared.Reflection.Common.GetTypeFromAssembly(assemblyName, mClassName, assemblyPath);
                MethodInfo methodInfo = objectType.GetMethod(MethodName);
                methodInfo.Invoke(Activator.CreateInstance(objectType), null);

                mPWMSServiceThread = new Made4Net.Shared.Logging.LogFile(DirPath, "AsyncQueueWorker", true);
                mPWMSServiceThread.WriteLine("Worker thread Started", true);
            }
            mPWMSServiceThread = new Made4Net.Shared.Logging.LogFile(DirPath, "AsyncQueueWorker", true);
            mPWMSServiceThread.WriteLine("Worker thread terminating gracefully", true);
        }

        public void requestStop()
        {
            _shouldStop = true;
        }
    }
}

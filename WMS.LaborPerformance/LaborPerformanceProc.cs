using System.Text;
using System;
using System.Collections;
using Made4Net.Shared;
using Made4Net.DataAccess;
using WMS.Logic;
using WMS.Lib;
using System.Messaging;

namespace WMS.LaborPerformance
{
    /// <summary>
    /// Summary description for LaborPerformanceProc.
    /// </summary>
    ///



    public class LaborPerformanceProc : Made4Net.Shared.QHandler
    {
        #region "Constructors"

        public LaborPerformanceProc()
            : base("LaborPerformanceProc", false)
        {

        }

        #endregion

        #region "Override Methods"

        /// <summary>
        /// This funtion processing the message that arrived to the service.
        /// Must override function from Made4Net.Shared namespace
        /// </summary>
        /// <param name="qMsg"></param>
        /// <param name="qSender"></param>
        /// <param name="e"></param>
        protected override void ProcessQueue(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            //string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null), appId = Made4Net.Shared.AppConfig.Get("LicenseAppId", null);
            bool isLicensed = true;//GetLicense(licUser);

            System.Reflection.Assembly oAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo oFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(oAssembly.Location);
            string fileversion = oFileVersionInfo.FileVersion;

            LogHandler lg = null;

            try
            {
                // Create logger instance
                string LoggingEnabled;
              //  LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                LoggingEnabled =Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceUseLogs);
                if (LoggingEnabled == "1")
                {
                    string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;

                 //   lg = new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(10000).Next().ToString() + ".txt");
                    lg = new LogHandler(DirPath, DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(10000).Next().ToString() + ".txt");
                    lg.StartWrite();

                    lg.Write("Start Labor Perormance Calculation v." + fileversion +"...");
                    lg.writeSeperator('*', 100);

                    lg.Write("License status : " + isLicensed.ToString());
                    lg.Write("Processing Message : " + GetMessageType(qSender));
                    lg.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                }
            }
            catch { }

            //try
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Made4Net.Shared.Util.GetSystemParameterValue("ServiceCultureSetting"));
            //    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = Made4Net.Shared.Util.GetSystemParameterValue("ServiceDecimalSeparator");
            //    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = Made4Net.Shared.Util.GetSystemParameterValue("ServiceDecimalSeparator");
            //}
            //catch { }

            // Base on message type deside which procedure to run
            ProcessEvenetMsg(qSender, qMsg, lg);

            //ReleaseLicense(licUser);

            try
            {
                // Create logger instance
                if (lg != null) lg.EndWrite();
            }
            catch { }
        }

        #endregion

        #region "Process Labor Performance Event Methods"

        protected void ProcessEvenetMsg(Made4Net.Shared.QMsgSender qSender, Message qmsg, LogHandler lg)
        {
            try
            {
                if (lg != null)
                {
                    lg.Write("Processing Message : " + qSender.Values["EVENT"].ToString());
                    lg.writeSeperator ('*',60);
                }
            }
            catch { }

            string TaskId ="";
            try
            {
                TaskId = qSender.Values["TASK"].ToString();
                string TaskUser = qSender.Values["USERID"].ToString();
                if (lg != null)
                {
                    lg.Write("Task: " + TaskId);
                    lg.Write("User: " + TaskUser);
                }

                string isUseLogLaborPefTaskCreated = Made4Net.Shared.Util.GetSystemParameterValue("UseLogLaborPefTaskCreated");
                string isUseLogLaborPefTaskCompleted = Made4Net.Shared.Util.GetSystemParameterValue("UseLogLaborPefTaskCompleted");

                LogHandler tasklg = null;
                TaskStdCalculation oTaskStdCalculation;
                switch ((WMSEvents.EventType)Convert.ToInt32(qSender.Values["EVENT"].ToString()))
                {
                    case WMSEvents.EventType.TaskAssigned:
                        if (lg != null) lg.Write("Task Assigned Calculation run... ");
                        if (isUseLogLaborPefTaskCreated == "1")
                        {
                            Random oRnd = new Random(1000);
                            string srnd = oRnd.Next().ToString();
                            string dtm = DateTime.Now.ToString("yyyyMMddhhmmss");
                            string filename;
                            if (TaskUser == String.Empty)
                                filename = "UndefUser" + "_" + TaskId + "_" + dtm + "_" + srnd + ".txt";
                            else
                                filename = TaskUser + "_" + TaskId + "_" + dtm + "_" + srnd + ".txt";

                            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;
                           // tasklg = new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), filename);
                            tasklg = new LogHandler(DirPath, filename);
                            if (lg != null) lg.Write("Log created: " + filename);

                            tasklg.StartWrite();
                        }
                        oTaskStdCalculation = new TaskStdCalculation(TaskId, TaskUser, tasklg);
                        //calculate  stdtime
                        oTaskStdCalculation.EvalEquation();
                        if (lg != null) lg.Write("Task Assigned Calculation evaluated.");

                        WriteStdTime(oTaskStdCalculation.STDTIME, TaskId, tasklg);
                        if (lg != null)
                        {
                            lg.Write("Task Assigned STD time updated completed.");
                            lg.Write("Task Assigned complete.");

                        }
                        ResponseOnSuccess(TaskId, qmsg);
                        break;
                    case WMSEvents.EventType.TaskCreated:
                        if (lg != null) lg.Write("Task Created Calculation run... ");
                        if (isUseLogLaborPefTaskCreated == "1")
                        {
                            Random oRnd = new Random(1000);
                            string srnd = oRnd.Next().ToString();
                            string dtm = DateTime.Now.ToString("yyyyMMddhhmmss");
                            string filename;
                            if (TaskUser==String.Empty)
                                filename = "UndefUser" + "_" + TaskId + "_" + dtm + "_" + srnd + ".txt";
                            else
                                filename = TaskUser + "_" + TaskId + "_" + dtm + "_" + srnd + ".txt";

                            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;
                          //  tasklg = new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), filename);
                            tasklg = new LogHandler(DirPath, filename);
                            if (lg != null) lg.Write("Log created: " + filename);

                            tasklg.StartWrite();
                        }
                         oTaskStdCalculation = new TaskStdCalculation(TaskId, TaskUser, tasklg);
                        //calculate  stdtime
                             oTaskStdCalculation.EvalEquation();
                        if (lg != null) lg.Write("Task Created Calculation evaluated.");

                        WriteStdTime(oTaskStdCalculation.STDTIME, TaskId, tasklg);
                        if (lg != null)
                        {
                            lg.Write("Task Created STD time updated completed.");
                            lg.Write("Task Created complete.");

                        }
                        ResponseOnSuccess(TaskId, qmsg);
                        break;


                    case WMS.Logic.WMSEvents.EventType.TaskCompleted:
                        if (lg != null) lg.Write("Task " + TaskId + " Completed Calculation run... ");
                        if (isUseLogLaborPefTaskCompleted == "1")
                        {
                            Random oRnd = new Random(10000);
                            string srnd = oRnd.Next().ToString();
                            string dtm = DateTime.Now.ToString("yyyyMMddhhmmss");

                            string filename;
                            filename = TaskUser + "_" + TaskId + "_" + dtm + "_" + srnd + ".txt";

                            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;
                           // tasklg = new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), filename);
                            tasklg = new LogHandler(DirPath, filename);
                            if (lg != null) lg.Write("Log created: " + filename);
                            tasklg.StartWrite();
                        }

                         oTaskStdCalculation = new TaskStdCalculation(TaskId, TaskUser, tasklg);
                    //recalculate  stdtime
                        oTaskStdCalculation.EvalEquation();
                        if (lg != null) lg.Write("Task Completed Calculation evaluated.");
                        WriteStdTime(oTaskStdCalculation.STDTIME, TaskId, tasklg);
                        if (lg != null) lg.Write("Task Completed STD Time updated.");

                    //calculate counters
#pragma warning disable 0618
                        oTaskStdCalculation.CalculateCounters(WMS.Lib.LaborPerFormance.LaborCounterScope.Task);
#pragma warning restore 0618
                        if (lg != null) lg.Write("Task Completed Counters  Calculated.");


                    //write to LABORPERFORMANCEAUDIT
                        oTaskStdCalculation.WriteAudit();
                        if (lg != null)
                        {
                            lg.Write("Task Completed Audit writed.");
                            lg.Write("Task Completed complete.");
                        }
                        ResponseOnSuccess(TaskId,qmsg);
                        break;
                }
                if (tasklg!=null)
                    tasklg.EndWrite();
            }
            catch (Exception ex)
            {
                if (lg != null) lg.Write("Error : " + ex.Message.ToString());
                ResponseOnError(TaskId, qmsg);
            }
        }

        protected void WriteStdTime(Double stdTaskTime, String TaskId, LogHandler lg)
        {
            if (stdTaskTime != 0D)
            {
                Task oTask = new Task(TaskId, false);
                oTask.UpdateStdTime(stdTaskTime);
                if (lg != null)
                    lg.Write("Std Time updated: " + stdTaskTime.ToString());
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// This function will get the license for the service
        /// </summary>
        /// <returns>Result : true = license granted, false = no license granted</returns>
        private bool GetLicense(string licUser)
        {
            bool ret = false;
            try
            {
                ret = true; // Made4Net.DataAccess.DataInterface.Connect(licUser);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// This function releases the license that the service used.
        /// </summary>
        private void ReleaseLicense(string licUser)
        {
            try
            {
                //Made4Net.DataAccess.DataInterface.Disconnect(licUser);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        /// <summary>
        /// What is the message type
        /// </summary>
        /// <param name="qSender"></param>
        /// <returns></returns>
        private string GetMessageType(Made4Net.Shared.QMsgSender qSender)
        {
            return MessageTypes.TRANSACTION;
        }

        public void ResponseOnError(string pTaskID, Message qMsg)
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("ERROR", "");
                qs.Send(newmq, pTaskID, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }

        public void ResponseOnSuccess(string pTaskID, Message qMsg)
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Send(newmq, pTaskID, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }


        #endregion

        #region "Variables, Enums, Constants"

        public class MessageTypes
        {
            public const string TRANSACTION = "TRANSACTION";
        }

        #endregion
    }



}
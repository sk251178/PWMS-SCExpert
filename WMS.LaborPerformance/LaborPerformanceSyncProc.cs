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

    public class LaborPerformanceSyncProc : Made4Net.Shared.QHandler
    {
        #region "Constructors"

        public LaborPerformanceSyncProc()
            : base("LaborPerformanceSyncProc", false)
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
            //System.Reflection.Assembly oAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //System.Diagnostics.FileVersionInfo oFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(oAssembly.Location);
            //string fileversion = oFileVersionInfo.FileVersion;

            LogHandler lg = null;

            try
            {
                //  string LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                string LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceUseLogs);
                if (LoggingEnabled == "1")
                {
                    string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;
                    // lg = new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(10000).Next().ToString() + "_sync.txt");
                    lg = new LogHandler(DirPath, DateTime.Now.ToString("yyyyMMddhhmmss") + new Random(10000).Next().ToString() + "_sync.txt");
                    lg.StartWrite();

                    //lg.Write("Start Sync Labor Perormance Calculation v." + fileversion + "...");
                    lg.writeSeperator('*', 100);

                    //lg.Write("License status : " + isLicensed.ToString());
                    //lg.Write("Processing Message : " + GetMessageType(qSender));
                    lg.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                }
            }
            catch { }

            ProcessEvenetMsg(qSender, qMsg, lg);


            try
            {
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
                    lg.writeSeperator('*', 60);
                }
            }
            catch { }

            string TaskId = "";
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
                            if (TaskUser == String.Empty)
                                tasklg = initTaskLog("UndefUser", "OnAssign", TaskId);
                            else
                                tasklg = initTaskLog(TaskUser, "OnAssign", TaskId);

                            if (lg != null) lg.Write("Log created: " + tasklg.FileName);
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
                            if (TaskUser == String.Empty)
                                tasklg = initTaskLog("UndefUser", "OnCreate", TaskId);
                            else
                                tasklg = initTaskLog(TaskUser, "OnCreate", TaskId);

                            if (lg != null) lg.Write("Log created: " + tasklg.FileName);
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
                            tasklg = initTaskLog(TaskUser, "OnComplete", TaskId);
                            if (lg != null) lg.Write("Log created: " + tasklg.FileName);
                            tasklg.StartWrite();
                        }

                        oTaskStdCalculation = new TaskStdCalculation(TaskId, TaskUser, tasklg);
                        //recalculate  stdtime
                        oTaskStdCalculation.EvalEquation();
                        if (lg != null) lg.Write("Task Completed Calculation evaluated.");
                        WriteStdTime(oTaskStdCalculation.STDTIME, TaskId, tasklg);
                        if (lg != null) lg.Write("Task Completed STD Time updated.");

                        //calculate counters
                        //oTaskStdCalculation.CalculateCounters(WMS.Lib.LaborPerFormance.LaborCounterScope.Task);
                        //if (lg != null) lg.Write("Task Completed Counters  Calculated.");


                        //write to LABORPERFORMANCEAUDIT
                        oTaskStdCalculation.WriteAudit();
                        oTaskStdCalculation.UpdateWHactivityParamsOnComplete();
                        if (lg != null)
                        {
                            lg.Write("Task Completed Audit writed.");
                            lg.Write("Task Completed complete.");
                        }
                        ResponseOnSuccess(TaskId, qmsg);
                        break;

                    case WMS.Logic.WMSEvents.EventType.LaborTaskUpdated:

                        if (lg != null) lg.Write("Task " + TaskId + " End time updated... ");

                        if (isUseLogLaborPefTaskCompleted == "1")
                        {
                            tasklg = initTaskLog(TaskUser, "OnUpdate", TaskId);
                            if (lg != null) lg.Write("Log created: " + tasklg.FileName);
                            tasklg.StartWrite();
                        }

                        oTaskStdCalculation = new TaskStdCalculation(TaskId, TaskUser, tasklg);

                        string TaskEndDate = qSender.Values["UPDATEDENDDATE"].ToString();
                        string actualTime = qSender.Values["EXECUTIONTIME"].ToString();

                        oTaskStdCalculation.UpdateAudit(TaskEndDate, actualTime);

                        if (lg != null)
                        {
                            lg.Write("Task End time updted to Audit.");
                        }
                        ResponseOnSuccess(TaskId, qmsg);
                        break;

                }
                if (tasklg != null)
                    tasklg.EndWrite();
            }
            catch (Exception ex)
            {
                if (lg != null) lg.Write("Error : " + ex.Message.ToString());
                ResponseOnError(TaskId, qmsg);
            }
        }

        private LogHandler initTaskLog(string pTaskUser, String qualifyingString, string pTaskID)
        {
            string filename = pTaskUser + "_" + pTaskID + "_" + qualifyingString + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + new Random(10000).Next().ToString() + ".txt";
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LaborPerformanceServiceLogDirectory;
            //  return new LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), filename);
            return new LogHandler(DirPath, filename);
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

    }



}
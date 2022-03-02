using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using WMS.Lib;
using System.Data;

namespace RetalixReplTaskManager
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected int LoggingEnabled = 0;

        #endregion

        #region Constructors

        public Handler()
            : base("ReplTaskManager", true)
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
          //  LoggingEnabled = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("UseLogs"));
            LoggingEnabled = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertReplTaskManagerSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ExpertReplTaskManagerUseLogs));
        }

        #endregion

        #region Methods

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                if (LoggingEnabled == 1)
                {
                    string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertReplTaskManagerLogPath;
                    //_qhlogger = new Made4Net.Shared.Logging.LogFile(Made4Net.Shared.AppConfig.Get("LogPath", null), "ReplTaskManager_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
                    _qhlogger = new Made4Net.Shared.Logging.LogFile(DirPath, "ReplTaskManager_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
                    _qhlogger.WriteLine("Starting to proccess message", true);
                    _qhlogger.WriteLine("Current warehouse set in dataaccss: " + Made4Net.DataAccess.DataInterface.ConnectionName,true );
                    _qhlogger.WriteLine("Current warehouse set in Logic: " + WMS.Logic.Warehouse.CurrentWarehouse + " (Connection = " + WMS.Logic.Warehouse.WarehouseConnection + ")",true );
                    _qhlogger.WriteLine("Current warehouse set Message: " + qSender.Values["WAREHOUSE"],true );

                }
                // Proccess message
                ProccessMessage(qSender);
                // Send response if needed
                ResponseOnSuccess(qMsg);
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == 1)
                {
                    _qhlogger.Close();
                }
                // Throw exception so the message will not be received
                throw new M4NQHandleException(ex.Message, false);
            }
        }

        private void ProccessMessage(Made4Net.Shared.QMsgSender qSender)
        {
            // Create audit record
            //WMS.Logic.Audit auditrec = new WMS.Logic.Audit();
            try
            {
              //  auditrec.Post(qSender, _qhlogger);
                //qSender.Values.i

                foreach (string el in qSender.Values)
                {
                    if (LoggingEnabled == 1)
                        WriteLog(el + "= " + qSender.Values.Get(el));
                }

                int caseSwitch = Convert.ToInt16(qSender.Values.Get("EVENT"));
                switch (caseSwitch)
                {
                    case 210:
                        string caseTASK = Convert.ToString(qSender.Values.Get("TASK"));
                        WMS.Logic.Task tm = new Task(caseTASK, true);

                        switch (tm.TASKTYPE)
                        {
                            case WMS.Lib.TASKTYPE.FULLREPL:
                            case WMS.Lib.TASKTYPE.PARTREPL:
                                UpdateReplSubType(qSender);
                                break;
                            default:
                                WriteLog("event id is out of conditioning treatment");
                                break;
                        }
                        break;
                    default:
                        //WriteLog("event id is out of conditioning treatment");
                        break;
                }


                //if (LoggingEnabled == 1)
                //    WriteLog("Saved");
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == 1)
                    WriteLog(ex.Message);

                LoggingEnabled = 0;
                throw ex;
            }
        }

        private void WriteLog(string msg) {

            if (LoggingEnabled == 1)
                _qhlogger.WriteLine(msg, true);
        }

        private void UpdateReplSubType(Made4Net.Shared.QMsgSender qSender)
        {
            string task="";
            string USERID = "";
            string sWhere = string.Empty;
            string sql = "SELECT TASK, IsNull(REPLTYPE,'') as REPLTYPE, IsNull(REPLMETHOD,'') as REPLMETHOD, IsNull(FROMWAREHOUSEAREA,'') as FROMWAREHOUSEAREA, IsNull(fromPICKREGION,'') as fromPICKREGION, IsNull(toPICKREGION,'') as toPICKREGION, IsNull(SKUGROUP,'') as SKUGROUP, IsNull(CLASSNAME,'') as CLASSNAME  FROM ReplTaskDetails WHERE TASK = '{0}'";
            try {
                task = qSender.Values.Get("TASK");
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";
            }catch{}
            sql = string.Format(sql, task);

            DataTable dt = new DataTable();
            DataTable dtRak = new DataTable();

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt,false,"");

            foreach(DataRow dr in dt.Rows)
            {

                sWhere += string.Format(" '{0}' like isnull(REPLTYPE,'%') ", dr["REPLTYPE"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(REPLMETHOD,'%') ", dr["REPLMETHOD"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(FROMWAREHOUSEAREA,'%') ", dr["FROMWAREHOUSEAREA"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(FROMPICKREGION,'%') ", dr["FROMPICKREGION"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(TOPICKREGION,'%') ", dr["TOPICKREGION"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(SKUGROUP,'%') ", dr["SKUGROUP"].ToString());
                sWhere += string.Format(" and '{0}' like isnull(CLASSNAME,'%') ", dr["CLASSNAME"].ToString());
                bool b = false;

                sql = "select top 1 PRIORITY, TASKSUBTYPE from REPLENISHMENTSUBTYPE where {0} order by PRIORITY";
                sql = string.Format(sql, sWhere);
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dtRak,false, "");
                foreach (DataRow drRak in dtRak.Rows)
                {
                    if (!string.IsNullOrEmpty(drRak["TASKSUBTYPE"].ToString()))
                    {
                        sql = "update tasks set TASKSUBTYPE = '{0}',editdate=getdate(),edituser='{2}' where task='{1}'";
                        sql = string.Format(sql, drRak["TASKSUBTYPE"].ToString(), task, USERID);
                        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql);
                        WriteLog("tasksubtype of task update successfully");
                        b = true;
                    }

                }
                if (!b)
                    WriteLog("tasksubtype of task was NOT update");
            }

        }



        private void ResponseOnSuccess(System.Messaging.Message qMsg)
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("MESSAGEPROCESSED", "1");
                qs.Send(newmq, qMsg.Label, qMsg.Id, System.Messaging.MessagePriority.Normal);
                if (LoggingEnabled == 1)
                    _qhlogger.WriteLine("Message sent to responce queue", true);
            }
        }

        #endregion
    }
}
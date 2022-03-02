using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using System.Data;

namespace RetalixFlowrackService
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected int LoggingEnabled = 0;

        #endregion

        #region Constructors

        public Handler(): base("Flowrack", true)
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
          //  LoggingEnabled = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("UseLogs"));
                LoggingEnabled = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackUseLogs));
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
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertFlowrackLogPath;
                    _qhlogger = new Made4Net.Shared.Logging.LogFile(DirPath, "Flowrack_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false); // PWMS-817
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

                try {
                    foreach (string el in qSender.Values)
                    {
                        if (LoggingEnabled == 1)
                            WriteLog(el + "= " + qSender.Values.Get(el));
                    }
                }catch{}

                int caseSwitch = Convert.ToInt16(qSender.Values.Get("EVENT"));
                switch (caseSwitch)
                {
                    case 210:
                        UpdateRackLocation(qSender);
                        break;
                    default:
                        WriteLog("event id is out of conditioning treatment");
                        break;
                }


                if (LoggingEnabled == 1)
                    WriteLog("Saved");
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

        private void UpdateRackLocation(Made4Net.Shared.QMsgSender qSender)
        {
            string task="";
            string tolocation = "";
            string TOWAREHOUSEAREA = "";
            string REPLENISHMENT = "";
            string FROMLOAD = "";
            string USERID = "";
            string sql = "SELECT top 1  TASK, TASKTYPE, LOCATION, WAREHOUSEAREA, FLOWRACKTYPE, REPLENISHMENT,FROMLOAD FROM  vLocationFlowRackTask where task='{0}'";
            try {
                task = qSender.Values.Get("TASK");
                //tolocation = qSender.Values.Get("TOLOC");
                //TOWAREHOUSEAREA = qSender.Values.Get("TOWAREHOUSEAREA");
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";
            }catch{}
            sql = string.Format(sql, task);

            DataTable dt = new DataTable();
            DataTable dtRak = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt,false,"");

            foreach(DataRow dr in dt.Rows)
            {
                tolocation = "";
                 TOWAREHOUSEAREA = "";
                 REPLENISHMENT = "";
                 FROMLOAD = "";
                if (!string.IsNullOrEmpty(dr["REPLENISHMENT"].ToString()))
                {
                    REPLENISHMENT = dr["REPLENISHMENT"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["LOCATION"].ToString()))
                {
                    tolocation = dr["LOCATION"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["WAREHOUSEAREA"].ToString()))
                {
                    TOWAREHOUSEAREA = dr["WAREHOUSEAREA"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["FROMLOAD"].ToString()))
                {
                    FROMLOAD = dr["FROMLOAD"].ToString();
                }

                sql = "select top 1 LOCATION,isnull(WAREHOUSEAREA,'') WAREHOUSEAREA from vLocationFlowRack where FRONTRACKLOCATION = '{0}' and WAREHOUSEAREA='{1}' and FLOWRACKTYPE= 'BACKRACK'";
                sql = string.Format(sql, tolocation, TOWAREHOUSEAREA);
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dtRak,false, "");
                foreach (DataRow drRak in dtRak.Rows)
                {
                    if (!string.IsNullOrEmpty(drRak["LOCATION"].ToString()))
                    {
                        sql = "update tasks set TOLOCATION = '{0}',editdate=getdate(),edituser='{2}',TOWAREHOUSEAREA='{3}' where task='{1}'";
                        sql = string.Format(sql, drRak["LOCATION"].ToString(), task, USERID, drRak["WAREHOUSEAREA"].ToString());
                        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql);

                        if (!string.IsNullOrEmpty(REPLENISHMENT))
                        {
                            sql = "update REPLENISHMENT set TOLOCATION = '{0}',editdate=getdate(),edituser='{2}',TOWAREHOUSEAREA='{3}' where REPLID='{1}'";
                            sql = string.Format(sql, drRak["LOCATION"].ToString(), REPLENISHMENT, USERID, drRak["WAREHOUSEAREA"].ToString());
                            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql);
                        }
                        if (!string.IsNullOrEmpty(FROMLOAD))
                        {
                            //Commented for RWMS-2484
                            //sql = "update LOADS set DESTINATIONLOCATION = '{0}',editdate=getdate(),edituser='{2}',DESTINATIONWAREHOUSEAREA='{3}' where loadid='{1}'";
                            //sql = string.Format(sql, drRak["LOCATION"].ToString(), FROMLOAD, USERID, drRak["WAREHOUSEAREA"].ToString());
                            //Made4Net.DataAccess.DataInterface.ExecuteScalar(sql);
                            //End Commented for RWMS-2484
                            //Added for RWMS-2484 - if REPLENISHMENT is CANCELED, then donot update the load.
                            string sqlReplenStatus = string.Format("Select isnull(STATUS,'') from REPLENISHMENT where REPLID='{0}'", REPLENISHMENT);
                            sqlReplenStatus = Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlReplenStatus));
                            if (sqlReplenStatus != "CANCELED")
                            {
                                sql = "update LOADS set DESTINATIONLOCATION = '{0}',editdate=getdate(),edituser='{2}',DESTINATIONWAREHOUSEAREA='{3}' where loadid='{1}'";
                                sql = string.Format(sql, drRak["LOCATION"].ToString(), FROMLOAD, USERID, drRak["WAREHOUSEAREA"].ToString());
                                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql);
                                //End Added for RWMS-2484
                            }
                        }
                    }
                    //select DESTINATIONLOCATION,DESTINATIONWAREHOUSEAREA from LOADS
                }
            }

        }

        //private void ProccessMessage(Made4Net.Shared.QMsgSender qSender)
        //{
        //    // Create audit record
        //    WMS.Logic.Audit auditrec = new WMS.Logic.Audit();
        //    try
        //    {
        //        auditrec.ACTIVITYDATE = Made4Net.Shared.Util.WMSStringToDate(qSender.Values["ACTIVITYDATE"]);
        //        _qhlogger.WriteLine("ACTIVITYDATE : " + auditrec.ACTIVITYDATE, true);
        //        auditrec.ACTIVITYTIME = Convert.ToInt32(qSender.Values["ACTIVITYTIME"]);
        //        _qhlogger.WriteLine("ACTIVITYTIME : " + auditrec.ACTIVITYTIME, true);
        //        auditrec.ACTIVITYTYPE = Convert.ToString(qSender.Values["ACTIVITYTYPE"]);
        //        _qhlogger.WriteLine("ACTIVITYTYPE : " + auditrec.ACTIVITYTYPE, true);
        //        auditrec.CONSIGNEE = Convert.ToString(qSender.Values["CONSIGNEE"]);
        //        _qhlogger.WriteLine("CONSIGNEE : " + auditrec.CONSIGNEE, true);
        //        auditrec.DOCUMENT = Convert.ToString(qSender.Values["DOCUMENT"]);
        //        _qhlogger.WriteLine("DOCUMENT : " + auditrec.DOCUMENT, true);
        //        try
        //        {
        //            auditrec.DOCUMENTLINE = Convert.ToInt32(qSender.Values["DOCUMENTLINE"]);
        //            _qhlogger.WriteLine("DOCUMENTLINE : " + auditrec.DOCUMENTLINE, true);
        //        }
        //        catch
        //        {
        //            _qhlogger.WriteLine("DOCUMENTLINE : null ", true);
        //        }
        //        auditrec.FROMLOAD = Convert.ToString(qSender.Values["FROMLOAD"]);
        //        _qhlogger.WriteLine("FROMLOAD : " + auditrec.FROMLOAD, true);
        //        auditrec.TOLOAD = Convert.ToString(qSender.Values["TOLOAD"]);
        //        _qhlogger.WriteLine("TOLOAD : " + auditrec.TOLOAD, true);
        //        auditrec.FROMLOC = Convert.ToString(qSender.Values["FROMLOC"]);
        //        _qhlogger.WriteLine("FROMLOC : " + auditrec.FROMLOC, true);
        //        try
        //        {
        //            auditrec.FROMQTY = Convert.ToDecimal(qSender.Values["FROMQTY"]);
        //            _qhlogger.WriteLine("FROMQTY : " + auditrec.FROMQTY, true);
        //        }
        //        catch
        //        {
        //            _qhlogger.WriteLine("FROMQTY : null", true);
        //        }
        //        auditrec.FROMSTATUS = Convert.ToString(qSender.Values["FROMSTATUS"]);
        //        _qhlogger.WriteLine("FROMSTATUS : " + auditrec.FROMSTATUS, true);
        //        auditrec.NOTES = Convert.ToString(qSender.Values["NOTES"]);
        //        _qhlogger.WriteLine("NOTES : " + auditrec.NOTES, true);
        //        auditrec.SKU = Convert.ToString(qSender.Values["SKU"]);
        //        _qhlogger.WriteLine("SKU : " + auditrec.SKU, true);
        //        auditrec.TOLOAD = Convert.ToString(qSender.Values["TOLOAD"]);
        //        _qhlogger.WriteLine("TOLOAD : " + auditrec.TOLOAD, true);
        //        auditrec.TOLOC = Convert.ToString(qSender.Values["TOLOC"]);
        //        _qhlogger.WriteLine("TOLOC : " + auditrec.TOLOC, true);
        //        try
        //        {
        //            auditrec.TOQTY = Convert.ToDecimal(qSender.Values["TOQTY"]);
        //            _qhlogger.WriteLine("TOQTY : " + auditrec.TOQTY, true);
        //        }
        //        catch
        //        {
        //            _qhlogger.WriteLine("TOQTY : null", true);
        //        }
        //        auditrec.TOSTATUS = Convert.ToString(qSender.Values["TOSTATUS"]);
        //        _qhlogger.WriteLine("TOSTATUS : " + auditrec.TOSTATUS, true);
        //        auditrec.USERID = Convert.ToString(qSender.Values["USERID"]);
        //        _qhlogger.WriteLine("USERID : " + auditrec.USERID, true);
        //        auditrec.ADDDATE = DateTime.Now;
        //        _qhlogger.WriteLine("ADDDATE : " + DateTime.Now, true);
        //        auditrec.ADDUSER = Convert.ToString(qSender.Values["ADDUSER"]);
        //        _qhlogger.WriteLine("ADDUSER : " + auditrec.ADDUSER, true);
        //        auditrec.EDITDATE = DateTime.Now;
        //        _qhlogger.WriteLine("EDITDATE : " + DateTime.Now, true);
        //        auditrec.EDITUSER = Convert.ToString(qSender.Values["EDITUSER"]);
        //        _qhlogger.WriteLine("EDITUSER : " + auditrec.EDITUSER, true);

        //        auditrec.Post();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (LoggingEnabled == 1)
        //            _qhlogger.WriteLine(ex.Message, true);

        //        LoggingEnabled = 0;
        //        throw ex;
        //    }


        //    _qhlogger.WriteLine("Saved", true);
        //}

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
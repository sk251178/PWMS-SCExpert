using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using System.Data;
using System.IO;
using System.Xml;

namespace BillingChargePostedService
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        //protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected LogHandler _qhlogger=null;
        protected int LoggingEnabled = 0;
        protected int MessageDelayTime = 0;

        #endregion

        #region Constructors

        public Handler()
            : base("BillingChargeDetailPosted", true)//PWMS-817
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
            LoggingEnabled = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeDetailPostedServiceSection ,
                                                   Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeDetailPostedServiceUseLogs)); //PWMS-817
            MessageDelayTime = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeDetailPostedServiceSection,
                                                   Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeDetailPostedServiceMessageDelayTime)); //PWMS-817
        }

        #endregion

        #region Methods

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                string DirPath = Made4Net.DataAccess.Util.GetInstancePath();// PWMS-817
                DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeDetailPostedServiceLogPath ;  // PWMS-817
                _qhlogger = new LogHandler(DirPath, "BillingChargeDetailPosted" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt"); // PWMS-817
                _qhlogger.Write("Starting to proccess message");

                System.Threading.Thread.Sleep(MessageDelayTime);
                // Proccess message
                ProccessMessage(qSender);
                // Send response if needed
                ResponseOnSuccess(qMsg);
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == 1)
                {
                 //   _qhlogger.Stream.Close();

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
                    case 91:
                        BillingChargeDetailPosted(qSender);
                        break;
                    default:
                        WriteLog("event id is out of conditioning treatment, " + caseSwitch);
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
                _qhlogger.Write(msg);
//                _qhlogger.WriteLine(msg, true);
        }

        //  Dim aq As New EventManagerQ
        //Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BillingChargeDetailPosted
        //aq.Add("EVENT", EventType)
        //aq.Add("USERID", puser)
        //aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        //aq.Add("CHARGEID", _CHARGEID)
        //aq.Add("CHARGELINE", _chargeline)
        //'aq.Add("AGREEMENT", _agreement.Name)
        //aq.Add("BILLFROMDATE", _billfromdate)
        //aq.Add("BILLTODATE", _billtodate)
        //aq.Add("BILLINGRUNID", _chargeheader.BILLINGRUNID)
        //aq.Add("BILLTOTAL", _billtotal)
        //aq.Add("CONSIGNEE", _chargeheader.CONSIGNEE)
        //aq.Send("BillingChargeDetailPosted")
        private void BillingChargeDetailPosted(Made4Net.Shared.QMsgSender qSender)
        {
            //vBillingChargeExport
            string CHARGEID = "",  USERID = "", AGREEMENT = "", caseSwitch = "";
            int CHARGELINE;
            string  CONSIGNEE = "";
            int AGREEMENTLINE = 0;
          //  DateTime BILLFROMDATE, BILLTODATE;
            //string filename;
            string sql;
            //string LogPath = Made4Net.Shared.AppConfig.Get("LogPath", null);// WMS.Logic.Common.GetSysParam("ApplicationLogDirectory").ToString();

          //  LogHandler oLogger;

                CHARGEID = qSender.Values.Get("CHARGEID");
                CHARGELINE=Convert.ToInt32( qSender.Values.Get("CHARGELINE"));
                AGREEMENT= qSender.Values.Get("AGREEMENT");
                CONSIGNEE= qSender.Values.Get("CONSIGNEE");
                //BILLFROMDATE =Convert.ToDateTime( qSender.Values.Get("BILLFROMDATE"));
                //BILLTODATE = Convert.ToDateTime(qSender.Values.Get("BILLTODATE"));
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";

                WMS.Logic.Billing.ChargeDetail cd;

                WMS.Logic.Billing.Agreement ah;
                cd = new WMS.Logic.Billing.ChargeDetail(CHARGEID, CHARGELINE);

                AGREEMENTLINE = cd.AGREEMENTLINE;
                AGREEMENT = cd.AGREEMENTNAME;

                sql = "select TRANTYPE FROM BILLINGAGREEMENTDETAIL where AGREEMENTNAME = '{0}' and CONSIGNEE='{1}' and LINE='{2}'";
                sql = string.Format(sql, AGREEMENT, CONSIGNEE, AGREEMENTLINE);
                caseSwitch = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql).ToString();

                WriteLog("caseSwitch = " + caseSwitch);
 // WMS.Logic.Billing.ChargeHeader ch;
            Int32 ret = 0;
            switch (caseSwitch.ToUpper())
            {
                case "INBOUND": case "OUTBOUND":

                    //filename = caseSwitch + "_BillingChargeDetailPosted_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + new Random().Next() + ".txt";

                    //WriteLog("create file " + LogPath + filename);

                    //oLogger = new LogHandler(LogPath, filename);
                    ah = new WMS.Logic.Billing.Agreement(AGREEMENT, CONSIGNEE,true);

                    if (caseSwitch.ToUpper() == "INBOUND")
                    {
                        WMS.Logic.Billing.InboundAgreementDetail iad = new WMS.Logic.Billing.InboundAgreementDetail(ref ah, AGREEMENTLINE);
                        cd = new WMS.Logic.Billing.ChargeDetail(CHARGEID, CHARGELINE);
                        ret = iad.ProcessBillingChargeDetailPosted(cd.BILLFROMDATE, cd.BILLTODATE, _qhlogger, USERID, cd);
                        //ret = iad.ProcessBillingChargeDetailPosted(BILLFROMDATE, BILLTODATE, _qhlogger, USERID, cd);
                    }
                    else//OUTBOUND
                    {

                        WMS.Logic.Billing.OutboundAgreementDetail oad = new WMS.Logic.Billing.OutboundAgreementDetail(ref ah, AGREEMENTLINE);
                        cd = new WMS.Logic.Billing.ChargeDetail(CHARGEID, CHARGELINE);
                        ret = oad.ProcessBillingChargeDetailPosted(cd.BILLFROMDATE, cd.BILLTODATE, _qhlogger, USERID, cd);
//                        ret = oad.ProcessBillingChargeDetailPosted(BILLFROMDATE, BILLTODATE, _qhlogger, USERID, cd);
                    }

                    sql = "delete BILLINGCHARGESDETAIL where CHARGEID='{0}' and CHARGELINE='{1}'";
                    sql = string.Format(sql, CHARGEID, CHARGELINE);
                    Made4Net.DataAccess.DataInterface.RunSQL(sql);
                    WriteLog("delete source charge " + sql);

                    sql = "update BILLINGCHARGESHEADER set BILLTOTAL = (select SUM(BILLTOTAL) from BILLINGCHARGESDETAIL where CHARGEID='{0}')  where CHARGEID='{0}'";
                    sql = string.Format(sql, CHARGEID);
                    WriteLog(sql);
                    Made4Net.DataAccess.DataInterface.RunSQL(sql);
                    //Added for RWMS-2015
                    sql = "SELECT * FROM BILLINGCHARGESDETAIL WHERE " + " CHARGEID = '" + CHARGEID + "' ";
                    DataTable dtCD = new DataTable();
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, dtCD);
                    if (dtCD.Rows.Count == 0)
                     {
                        //Commented for RWMS-2386(RWMS-2239) Start
                       //sql = "update BILLINGCHARGESHEADER set BILLTOTAL = 0.0 from BILLINGCHARGESDETAIL where CHARGEID='{0}'";
                       //Commented for RWMS-2386(RWMS-2239) End

                       //Added for RWMS-2386(RWMS-2239) Start
                        sql = "update BILLINGCHARGESHEADER set BILLTOTAL = 0.0 where CHARGEID='{0}'";
                       //Added for RWMS-2386(RWMS-2239) End
                      sql = string.Format(sql, CHARGEID);
                      WriteLog(sql);
                      Made4Net.DataAccess.DataInterface.RunSQL(sql);
                     }
                    //Ended for RWMS-2015
                    break;
                default:
                    WriteLog("illegal caseSwitch = " + caseSwitch);
                    break;
            }
            WriteLog("Num of created rows = " + ret);


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
                    _qhlogger.Write("Message sent to responce queue");
                //_qhlogger.WriteLine("Message sent to responce queue", true);
            }
        }

        #endregion
    }
}
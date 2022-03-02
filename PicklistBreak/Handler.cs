using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using System.Data;

namespace PicklistBreak
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected int LoggingEnabled = 0;

        #endregion

        #region Constructors

        public Handler()
            : base("picklistbreak", true)
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
            LoggingEnabled = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("UseLogs"));
        }

        #endregion

        #region Methods

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                Made4Net.Shared.Warehouse.setCurrentWarehouse(qSender.Values["WAREHOUSE"]);

              Made4Net.DataAccess.DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;

                if (LoggingEnabled == 1)
                {
                    _qhlogger = new Made4Net.Shared.Logging.LogFile(Made4Net.Shared.AppConfig.Get("LogPath", null), "PicklistBreak_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
                    //qSender.Values["WAREHOUSE"]
                    _qhlogger.WriteLine("Starting to proccess message", true);
                    _qhlogger.WriteLine("qSender.Values('WAREHOUSE')=" + qSender.Values["WAREHOUSE"], true);
                    _qhlogger.WriteLine("Current warehouse set in dataaccss: " + Made4Net.DataAccess.DataInterface.ConnectionName, true);
                    _qhlogger.WriteLine("Current warehouse set in Logic: " + WMS.Logic.Warehouse.CurrentWarehouse + " (Connection = " + WMS.Logic.Warehouse.WarehouseConnection + ")",true );
                   // _qhlogger.WriteLine("Current warehouse set Message: " + qSender.Values["WAREHOUSE"],true );

                }
                // Proccess message
                ProccessMessage(qSender);
                // Send response if needed
              //  ResponseOnSuccess(qMsg);
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == 1)
                {
                    _qhlogger.WriteLine(ex.Message,true);
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
                    case 227:
                        PicklistBreak(qSender);
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
        private void PicklistBreak(Made4Net.Shared.QMsgSender qSender)
        {
            string wave = "", USERID = "";
            try
            {
                wave = qSender.Values.Get("DOCUMENT");
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";
            }
            catch { }
            PicklistBreakHandler ph = new PicklistBreakHandler();
            ph.PicklistBreak(wave, USERID);
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;

namespace WMS.InventoryTransaction
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected int LoggingEnabled = 0;
        protected string LogDirectory = "";
        #endregion

        #region Constructors

        public Handler()
            : base("InventoryTransaction", true)
        {
            //LoggingEnabled = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("UseLogs"));
            LoggingEnabled = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceUseLogs));
           // LogDirectory = System.Configuration.ConfigurationSettings.AppSettings.Get("LogPath");
             LogDirectory =  Made4Net.DataAccess.Util.GetInstancePath();
             LogDirectory += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.InventoryTransServiceLogPath;

        }

        #endregion

        #region Methods

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                if (LoggingEnabled == 1)
                {
                    _qhlogger = new Made4Net.Shared.Logging.LogFile(LogDirectory, "InventoryTransaction_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
                    _qhlogger.WriteLine("Starting to proccess message: \\ScExpert\\WMS.InventoryTransaction\\Handler.cs", true);
                }
                // Proccess message
                ProccessMessage(qSender);
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
            WMS.Logic.InventoryTransaction it = new WMS.Logic.InventoryTransaction();
            try
            {

                it.TRANDATE = Made4Net.Shared.Util.WMSStringToDate(qSender.Values["TRANDATE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("TRANDATE : " + it.TRANDATE, true);
                it.INVTRNTYPE = Convert.ToString(qSender.Values["INVTRNTYPE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("INVTRNTYPE : " + it.INVTRNTYPE, true);
                it.CONSIGNEE = Convert.ToString(qSender.Values["CONSIGNEE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("CONSIGNEE : " + it.CONSIGNEE, true);
                it.DOCUMENT = Convert.ToString(qSender.Values["DOCUMENT"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("DOCUMENT : " + it.DOCUMENT, true);
                it.LINE = Convert.ToInt32(qSender.Values["LINE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("LINE : " + it.LINE, true);
                it.LOADID = Convert.ToString(qSender.Values["LOADID"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("LOADID : " + it.LOADID, true);
                it.UOM = Convert.ToString(qSender.Values["UOM"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("UOM : " + it.UOM, true);
                it.QTY = Convert.ToDecimal(qSender.Values["QTY"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("QTY : " + it.QTY, true);
                try
                {
                    it.CUBE = Convert.ToDecimal(qSender.Values["CUBE"]);
                    if (LoggingEnabled == 1) _qhlogger.WriteLine("CUBE : " + it.CUBE, true);
                }
                catch { }
                try
                {
                    it.WEIGHT = Convert.ToDecimal(qSender.Values["LOADWEIGHT"]);
                    if (LoggingEnabled == 1) _qhlogger.WriteLine("WEIGHT : " + it.WEIGHT, true);
                }
                catch { }
                try
                {
                    it.AMOUNT = Convert.ToDecimal(qSender.Values["AMOUNT"]);
                    if (LoggingEnabled == 1) _qhlogger.WriteLine("AMOUNT : " + it.AMOUNT, true);
                }
                catch { }
                it.SKU = Convert.ToString(qSender.Values["SKU"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("SKU : " + it.SKU, true);
                it.STATUS = Convert.ToString(qSender.Values["STATUS"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("STATUS : " + it.STATUS, true);
                try
                {
                    it.REASONCODE = Convert.ToString(qSender.Values["REASONCODE"]);
                    if (LoggingEnabled == 1) _qhlogger.WriteLine("REASONCODE : " + it.REASONCODE, true);
                }
                catch { }
                it.ADDDATE = Made4Net.Shared.Util.WMSStringToDate(qSender.Values["ADDDATE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("ADDDATE : " + it.ADDDATE, true);
                it.ADDUSER = Convert.ToString(qSender.Values["ADDUSER"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("ADDUSER : " + it.ADDUSER, true);
                it.EDITDATE = Made4Net.Shared.Util.WMSStringToDate(qSender.Values["EDITDATE"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("EDITDATE : " + it.EDITDATE, true);
                it.EDITUSER = Convert.ToString(qSender.Values["EDITUSER"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("EDITUSER : " + it.EDITUSER, true);
                //RWMS-344:Begin
                it.HOSTREFERENCEID = Convert.ToString(qSender.Values["HOSTREFERENCEID"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("HOSTREFERENCEID : " + it.HOSTREFERENCEID, true);

                it.NOTES = Convert.ToString(qSender.Values["NOTES"]);
                if (LoggingEnabled == 1) _qhlogger.WriteLine("NOTES : " + it.NOTES, true);
                //RWMS-344:End
                // Extract inventory attributes
                try
                {
                    it.ExtractAttributesFromQSender(qSender, _qhlogger);
                }
                catch (Exception ex)
                {
                    if (LoggingEnabled == 1)
                        _qhlogger.WriteLine("Error extracting SKU attributes [ " + ex.Message + "]", true);
                    LoggingEnabled = 0;
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == 1)
                    _qhlogger.WriteLine(ex.Message, true);

                LoggingEnabled = 0;
                throw ex;
            }

            if (LoggingEnabled == 1) _qhlogger.WriteLine("before it.Post() : INVTRANS : " + it.INVTRANS, true);

            it.Post();
           //it.INVTRANS =  it.Post1();
           //it.INVTRANS = qSender.Values["INVTRANS"].ToString();
            if (LoggingEnabled == 1) _qhlogger.WriteLine("after it.Post() : INVTRANS : " + it.INVTRANS, true);

            if (LoggingEnabled == 1) _qhlogger.WriteLine("Saved", true);
        }

        #endregion
    }
}
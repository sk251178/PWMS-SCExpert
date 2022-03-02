using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using System.Data;
using System.IO;
using System.Xml;

namespace WMS.Billing.ChargeExport
{
    public class Handler : Made4Net.Shared.QHandler
    {
        #region Fields

        protected Made4Net.Shared.Logging.LogFile _qhlogger = null;
        protected int LoggingEnabled = 0;

        #endregion

        #region Constructors

        public Handler()
            : base("BillingChargeExport", true)// PWMS-817
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);
            LoggingEnabled = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceSection,
                                                   Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceUseLogs));  // PWMS-817;
        }

        #endregion

        #region Methods

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                if (LoggingEnabled == 1)
                {
                    string DirPath = Made4Net.DataAccess.Util.GetInstancePath(); // PWMS-817
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceLogPath;  // PWMS-817
                    _qhlogger = new Made4Net.Shared.Logging.LogFile(DirPath, "BillingChargeExport_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false); //PWMS-817
                    _qhlogger.WriteLine("Starting to proccess message", true);

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
                    case 2011:
                       // BillingChargeExport(qSender);
                        BillingChargeExportXML(qSender);
                        break;
                    default:
                        WriteLog("event id is out of conditioning treatment, " + caseSwitch);
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

        private void BillingChargeExportXML(Made4Net.Shared.QMsgSender qSender)
        {

            string CHARGEID, USERID;
            string ExportOutputFolder, ExportBackUpFolder, fileName;
            CHARGEID = "";
//            BillDateFormat = System.Configuration.ConfigurationSettings.AppSettings.Get("BillDateFormat");
            //Start added for PWMS-817
            ExportOutputFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath( Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceSection,
                                                   Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceExportOutputFolder));  // PWMS-817;
            ExportBackUpFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath( Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceSection,
                                                   Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceExportBackUpFolder));  // PWMS-817;

            string sql = "SELECT * FROM vBillingChargeExport where CHARGEID='{0}'";
            try
            {
                CHARGEID = qSender.Values.Get("CHARGEID");
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";
            }
            catch { }
            sql = string.Format(sql, CHARGEID);

            DataTable dt = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, false, "");
            if (dt.Rows.Count > 0)
            {
                fileName = "BillingChargeExport_" + DateTime.Now.ToString("yyyy_MM_dd") + "_" + Made4Net.Shared.Util.getNextCounter("BillingChargeExport") + ".xml";
                string filePath = System.IO.Path.Combine(ExportOutputFolder, fileName);
                string fileBackUp = System.IO.Path.Combine(ExportBackUpFolder, fileName);

                XmlDocument xmlData = new XmlDocument();
                string XmlSchema = "<?xml version=\"1.0\" encoding=\"utf-8\"?><DATACOLLECTION></DATACOLLECTION>";
                xmlData.LoadXml(XmlSchema);


                BuildHeaderStaic(xmlData, dt);

                //writeToLog("temp xml is: " + xmlData.InnerXml);
                //if (File.Exists(exportCustomTranslationFile))
                //    xmlReceipt = transXSLT(xmlReceipt);
                //else
                //    writeToLog("exportCustomTranslationFile is not exists!");
                //writeToLog("transformed xml is: " + xmlReceipt.InnerXml);
                WriteLog(xmlData.InnerXml);
                WriteLog("save to: " + filePath);
                xmlData.Save(filePath);

                WriteLog("save to: " + fileBackUp);
                xmlData.Save(fileBackUp);

            }//if (dt.Rows.Count > 0)
            else
            {
                WriteLog(sql + " is empty!");

            }

        }
//        <?xml version="1.0" encoding="utf-8"?>
//<DATACOLLECTION>
// <DATA>
//   <CONSIGNEE></CONSIGNEE>
//    <BILLDATE> </ BILLDATE >
//    <CHARGEID>< /CHARGEID >
//    <BILLINGACCOUNT> < /BILLINGACCOUNT >
//    <BILLTOTAL></ BILLTOTAL >
//</DATA>
//</DATACOLLECTION>

        private void BuildHeaderStaic(XmlDocument xmlData, DataTable dt)
        {

            foreach (DataRow dr in dt.Rows)
            {
                XmlElement oElem = CreateElement(ref xmlData, "DATACOLLECTION", "DATA");

                AppendNodeToElementStatic(ref xmlData, ref oElem, dr, "CONSIGNEE");
                AppendNodeToElementStatic(ref xmlData, ref oElem, dr, "BILLDATE");
                AppendNodeToElementStatic(ref xmlData, ref oElem, dr, "CHARGEID");
                AppendNodeToElementStatic(ref xmlData, ref oElem, dr, "BILLINGACCOUNT");
                AppendNodeToElementStatic(ref xmlData, ref oElem, dr, "BILLTOTAL");
            }
        }

        private void BuildHeaderDynamic(XmlDocument xmlData, DataTable dt)
        {
            string ColumnVal;
            string BillDateFormat = System.Configuration.ConfigurationManager.AppSettings.Get("BillDateFormat");

            foreach (DataRow dr in dt.Rows)
            {
                XmlElement oElem = CreateElement(ref xmlData, "DATACOLLECTION", "DATA");

                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        ColumnVal = dr[dc.ColumnName].ToString();//.ToString(BillDateFormat);

                        ColumnVal = Convert.ToDateTime(ColumnVal).ToString(BillDateFormat);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                        {
                            ColumnVal = dr[dc.ColumnName].ToString();
                        }
                        else ColumnVal = "";
                    }
                    AppendNodeToElement(ref xmlData, ref oElem, ColumnVal, dc.ColumnName);
                }
            }
        }


        private static XmlElement CreateElement(ref XmlDocument oXmlDoc, string ParentElement, string ElementName)
        {
            XmlElement oXmlTempNode;
            oXmlTempNode = oXmlDoc.CreateElement(ElementName);
            oXmlDoc.SelectSingleNode(ParentElement).AppendChild(oXmlTempNode);
            return oXmlTempNode;
        }

        private static void AppendNodeToElementStatic(ref XmlDocument oXmlDoc, ref XmlElement oXmlElemnt, DataRow dr, string NodeName)
        {
            string Value;
            string BillDateFormat = System.Configuration.ConfigurationManager.AppSettings.Get("BillDateFormat");

            if (!string.IsNullOrEmpty(dr[NodeName].ToString()))
            {
                Value = dr[NodeName].ToString();
                if (NodeName.ToUpper() == "BILLDATE")
                {
                    Value = Convert.ToDateTime(Value).ToString(BillDateFormat);
                }
            }
            else Value = "";

            XmlElement oXmlTempNode;
            oXmlTempNode = oXmlDoc.CreateElement(NodeName);
            oXmlTempNode.InnerText = Value;
            //oXmlTempNode.AppendChild(oXmlDoc.CreateCDataSection(Value));
            oXmlElemnt.AppendChild(oXmlTempNode);
        }

        private static void AppendNodeToElement(ref XmlDocument oXmlDoc, ref XmlElement oXmlElemnt, string Value, string NodeName)
        {
            XmlElement oXmlTempNode;
            oXmlTempNode = oXmlDoc.CreateElement(NodeName);
            oXmlTempNode.InnerText = Value;
            //oXmlTempNode.AppendChild(oXmlDoc.CreateCDataSection(Value));
            oXmlElemnt.AppendChild(oXmlTempNode);
        }
        private void BillingChargeExport(Made4Net.Shared.QMsgSender qSender)
        {
            //vBillingChargeExport
            string CHARGEID, USERID, ColumnVal, BillDateFormat, line;
            string ExportOutputFolder, ExportBackUpFolder, fileName;
            CHARGEID = "";
            BillDateFormat = System.Configuration.ConfigurationManager.AppSettings.Get("BillDateFormat");
            ExportOutputFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath(System.Configuration.ConfigurationManager.AppSettings.Get("ExportOutputFolder"));
            ExportBackUpFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath(System.Configuration.ConfigurationManager.AppSettings.Get("ExportBackUpFolder"));

            string sql = "SELECT * FROM vBillingChargeExport where CHARGEID='{0}'";
            try {
                CHARGEID = qSender.Values.Get("CHARGEID");
                USERID = qSender.Values.Get("USERID");
                if (string.IsNullOrEmpty(USERID)) USERID = "System";
            }catch{}
            sql = string.Format(sql, CHARGEID);

            DataTable dt = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt,false,"");
            if (dt.Rows.Count > 0)
            {
                fileName ="BillingChargeExport_" + DateTime.Now.ToString("yyyy_MM_dd") + "_" + Made4Net.Shared.Util.getNextCounter("BillingChargeExport") + ".txt";
                string filePath =System.IO.Path.Combine(ExportOutputFolder, fileName);
                string fileBackUp = System.IO.Path.Combine(ExportBackUpFolder, fileName);


                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        line = string.Empty;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.DataType == typeof(DateTime))
                            {
                                ColumnVal = dr[dc.ColumnName].ToString();//.ToString(BillDateFormat);

                                ColumnVal =Convert.ToDateTime(ColumnVal).ToString(BillDateFormat);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                                {
                                    ColumnVal = dr[dc.ColumnName].ToString();
                                }
                                else ColumnVal = "";
                            }
                            if (line == string.Empty)
                                line = ColumnVal;
                            else
                                line = line + ";" + ColumnVal;
                        }
                        sw.WriteLine(line);
                    }
                }//using (StreamWriter sw = new StreamWriter(fs))

            }//if (dt.Rows.Count > 0)
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
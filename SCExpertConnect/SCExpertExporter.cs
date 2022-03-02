using System;
using System.Xml;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Messaging;
using System.Runtime.InteropServices;
using Made4Net.Shared;
using Made4Net.DataAccess;
using ExpertObjectMapper;
using System.Reflection;
using SCExpertConnectPlugins.BO;

namespace SCExpertConnect
{
    #region SCExpertConnectExporter

    public class SCExpertConnectExporter : Made4Net.Shared.QHandler
    {
        #region Ctor

        public SCExpertConnectExporter() : base("SCExpertConnect", false) { }

        #endregion

        protected override void ProcessQueue(System.Messaging.Message qMsg, QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            OutgoingEventProcessor oEvtProc = new OutgoingEventProcessor();
            string sMultithreading = "0";
            Int32 iMaxNumberOfThreads = 0;

            try{

                sMultithreading = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectUseEnableMultiThreading);
            }
            catch{}

            try {
               //iMaxNumberOfThreads =  Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("ThreadPoolMaxThreads"));
                iMaxNumberOfThreads  = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceUseLogs));
            }
            catch { iMaxNumberOfThreads = 1; }

            if (sMultithreading == "1")
            {
                //Thread oRequestProccesor = new Thread(new ParameterizedThreadStart(oEvtProc.ProcessRequest));
                //oRequestProccesor.Start(qSender);
                try
                {
                    ThreadPool.SetMaxThreads(iMaxNumberOfThreads, iMaxNumberOfThreads);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(oEvtProc.ProcessRequest), qSender);
                    Thread.Sleep(1000);
                }
                catch{}
            }
            else
            {
                oEvtProc.ProcessRequest(qSender);
            }
        }
    }

    #endregion

    #region OutgoingEventProcessor

    public class OutgoingEventProcessor
    {
        int stransactionID = 0;

        #region Ctor

        public OutgoingEventProcessor()
        {
        }

        #endregion

        #region Process Message

        public void ProcessRequest(object qSend)
        {
            QMsgSender qSender = (QMsgSender)qSend;
            Made4Net.Shared.Logging.LogFile oLogger = null;
            try
            {
                string whId = qSender.Values["WAREHOUSE"];
                SetConnection(whId);

                oLogger = InitLogger("");
                if (oLogger != null) { oLogger.WriteLine(string.Format("Export phase started for Expert event... Message Content:"), true); }
                PrintMessageContent(qSender, oLogger);

                string sAction = qSender.Values["Action"];
                int evt = Convert.ToInt32(qSender.Values["EVENT"]);
                if (sAction == null) sAction = "";

                if (sAction.Equals("TransmitTransaction", StringComparison.OrdinalIgnoreCase))
                    TransmitTransaction(qSender, oLogger);
                else
                    ProcessExpertEvent(evt, qSender, oLogger);

                if (oLogger != null)
                {
                    oLogger.Dispose();
                    oLogger = null;
                }
            }
            catch (Made4Net.Shared.M4NException ex)
            {
                if (oLogger != null)
                {
                    oLogger.WriteSeperator();
                    oLogger.WriteLine("Exception while exporting Event from expert: " + ex.ToString(), true);
                    oLogger.WriteSeperator();
                }
            }
            catch (Exception ex)
            {
                if (oLogger != null)
                {
                    oLogger.WriteSeperator();
                    oLogger.WriteLine("Exception while exporting Event from expert: " + ex.ToString(), true);
                    oLogger.WriteSeperator();
                    oLogger.WriteLine("Keeping the message for reprocessing... ", true);
                    qSender.Send("SCExpertConnect", "", "", MessagePriority.Normal);
                }
            }
        }

        #endregion

        #region Export Files

        private void ProcessExpertEvent(int pEventId, QMsgSender qSender, Made4Net.Shared.Logging.LogFile oLogger)
        {
            string whId = qSender.Values["WAREHOUSE"];
            string sConsignee = qSender.Values["CONSIGNEE"];
            XmlDocument oXmlDoc = null;
            string sSql = string.Format("select SCEXPERTCONNECTPLUGINS.PLUGINID,ASSEMBLYDLL,CLASSNAME,CONSIGNEE from SCEXPERTPLUGINEVENTREGISTRATION inner join SCEXPERTCONNECTPLUGINS on SCEXPERTCONNECTPLUGINS.PLUGINID = SCEXPERTPLUGINEVENTREGISTRATION.PLUGINID inner join SCEXPERTCONNECTPLUGINTYPES on SCEXPERTCONNECTPLUGINTYPES.PLUGINTYPEID = SCEXPERTCONNECTPLUGINS.PLUGINTYPEID where eventid = {0} and SCEXPERTPLUGINEVENTREGISTRATION.WAREHOUSEID = '{1}' and '{2}' like case isnull(SCEXPERTPLUGINEVENTREGISTRATION.CONSIGNEE,'') when '' then '%' else SCEXPERTPLUGINEVENTREGISTRATION.CONSIGNEE end", pEventId, whId, sConsignee);
            // Added for PWMS-732 Start
            if (oLogger != null) { oLogger.WriteLine("SQL for retriving Plugin Details :" + sSql,true);}
            //Added for PWMS-732 End
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sSql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
            int res = 0;
            string TransactionKey = "";
            string[] orderList = null;
            if (dt.Rows.Count == 0)
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("No Plugins were defined for handling the current event..."), true); }
                return;
            }
            if ((int)WMS.Logic.WMSEvents.EventType.WavePlaned == pEventId)
            {
                orderList = qSender.Values["DOCUMENTLINE"].ToString().Split(',');
            }
            bool isSkip = false;
            foreach (DataRow dr in dt.Rows)
            {
                // Get the XML document for transaction in Expert format
                if (oLogger != null) { oLogger.WriteLine(string.Format("Trying to produce the XML document for the plugin..."), true); }
                if (orderList != null && orderList.Length > 0)
                {
                    foreach (var item in orderList)
                    {
                        qSender.Values["DOCUMENT"] = item;
                        oXmlDoc = GetXMLDocument(pEventId, qSender, oLogger, out TransactionKey, dr["CONSIGNEE"].ToString());

                        //RWMS-2564 RWMS-2563 START
                        if (oLogger != null) { oLogger.WriteLine(string.Format("XML document created..."), true); }
                        if (oLogger != null) { oLogger.WriteLine(string.Format(oXmlDoc.InnerXml.ToString()), true); }
                        if (oLogger != null) { oLogger.WriteLine(string.Format("Will try to execute appropriate plugins..."), true); }
                        //RWMS-2564 RWMS-2563 END

                        BasePlugin oPlugin = GetPlugin(oLogger, dr);
                        isSkip = ShouldFilterTransaction(oPlugin, oXmlDoc, oLogger);
                        if (isSkip)
                        {
                            break;
                        }
                        res = TransformXml(pEventId, oLogger, oXmlDoc, res, TransactionKey, oPlugin);
                    }
                    if (isSkip)
                    {
                        isSkip = false;
                        if (oLogger != null) { oLogger.WriteLine(string.Format("Terminating transaction processing (transaction expoert filter match)."), true); }
                        continue;
                    }
                }
                else
                {
                    oXmlDoc = GetXMLDocument(pEventId, qSender, oLogger, out TransactionKey, dr["CONSIGNEE"].ToString());
                    BasePlugin oPlugin = GetPlugin(oLogger, dr);
                    //End Added for RWMS-504
                    if (ShouldFilterTransaction(oPlugin, oXmlDoc, oLogger))
                    {
                        if (oLogger != null) { oLogger.WriteLine(string.Format("Terminating transaction processing (transaction expoert filter match)."), true); }
                        continue;
                    }

                    // Transform the XML to required format
                    res = TransformXml(pEventId, oLogger, oXmlDoc, res, TransactionKey, oPlugin);
                }


            }
        }

        private int TransformXml(int pEventId, Made4Net.Shared.Logging.LogFile oLogger, XmlDocument oXmlDoc, int res, string TransactionKey, BasePlugin oPlugin)
        {
            XmlDocument XSLTransformedXmlDoc = null;
            if (oPlugin.ExportTranslationFile != null && oPlugin.ExportTranslationFile != "")
            {
                //Commented for RWMS-503
                //Made4Net.Shared.XSLTransformer oTransformer = new XSLTransformer();
                //XSLTransformedXmlDoc = oTransformer.Transform(oXmlDoc, oPlugin.ExportTranslationFile);
                //if (oLogger != null) { oLogger.WriteLine(string.Format("Output XML file Translated according to plugin translation file..."), true); }
                //End Commented for RWMS-503
                //Added for RWMS-503
                try
                {
                    if (oLogger != null) { oLogger.WriteLine(string.Format("Processing to save the raw xml file..."), true); }
                    oPlugin.SaveRawXML(oXmlDoc, pEventId.ToString() + "_" + oPlugin.BOType + "_" + oLogger.FileNameNoExtension);
                    if (oLogger != null) { oLogger.WriteLine(string.Format("raw xml file saved."), true); }
                    if (oLogger != null) { oLogger.WriteLine(string.Format("initializing the XSLTransformer..."), true); }
                    Made4Net.Shared.XSLTransformer oTransformer = new XSLTransformer();
                    if (oLogger != null) { oLogger.WriteLine(string.Format("processing xml doc to transformation file {0}...", oPlugin.ExportTranslationFile), true); }
                    XSLTransformedXmlDoc = oTransformer.Transform(oXmlDoc, oPlugin.ExportTranslationFile);
                    if (oLogger != null) { oLogger.WriteLine(string.Format("transformation completed"), true); }
                    if (oLogger != null) { oLogger.WriteLine(string.Format("Output XML file Translated according to plugin translation file..."), true); }

                }
                catch (Exception ex)
                {
                    if (oLogger != null)
                    {
                        oLogger.WriteLine(string.Format("Exception while Translating Output XML file..."), true);
                        oLogger.WriteLine(ex.Message.ToString(), true);
                    }
                }
                //End Added for RWMS-503
            }
            else
                XSLTransformedXmlDoc = oXmlDoc;

            res = oPlugin.Export(XSLTransformedXmlDoc);

            SCExpertConnectTransactionStatus.TransactionStatus sTransStatus;
            if (res != 0)
            {
                sTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Fail;
                //Added for PWMS-732 Start
                if (oLogger != null) { oLogger.WriteLine(string.Format("Tranasaction Status :" + sTransStatus), true); }
                //Added for PWMS-732 End
            }
            else
            {
                sTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Success;
                //Added for PWMS-732 Start
                if (oLogger != null) { oLogger.WriteLine(string.Format("Tranasaction Status :" + sTransStatus), true); }
                //Added for PWMS-732 End
            }
            // Add record for the connect transaction audit table
            SCExpertConnectTransaction oTransaction = new SCExpertConnectTransaction();
             //Added for RWMS-1509 and RWMS-1502
            oTransaction.PostTransaction(SCExpertConnectTransactionTypes.TransactionType.Export, oPlugin.PluginId,
            sTransStatus, "", oPlugin.BOType, TransactionKey, oXmlDoc.InnerXml.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-8""?>", ""), "", out stransactionID);
            //RWMS-1434 Strat
            SendAlertsToRecepients(oPlugin, sTransStatus, TransactionKey, stransactionID, oLogger);
            //RWMS-1434 End
            //Ended for RWMS-1509 and RWMS-1502
            if (oLogger != null) {

                //RWMS-2564 RWMS-2563 START
                oLogger.WriteLine(string.Format("Transaction Details as below:"), true);
                oLogger.WriteLine(string.Format("Transaction Key: ") + TransactionKey.ToString(), true);
                oLogger.WriteLine(string.Format("Transaction Status: ") + sTransStatus.ToString(), true);
                if (sTransStatus == SCExpertConnectTransactionStatus.TransactionStatus.Fail)
                {
                    oLogger.WriteLine(string.Format("Failed to process Transaction.Writing XML document to log:"), true);
                    oLogger.WriteLine(XSLTransformedXmlDoc.InnerXml.ToString(), true);
                }
                oLogger.WriteLine(XSLTransformedXmlDoc.InnerXml.ToString(), true);
                //RWMS-2564 RWMS-2563 END

                oLogger.WriteLine(string.Format("Message processed successfully..."), true);
            }
            return res;
        }

        private static BasePlugin GetPlugin(Made4Net.Shared.Logging.LogFile oLogger, DataRow dr)
        {
            if (oLogger != null) { oLogger.WriteLine(string.Format("XML document created..."), true); }
            if (oLogger != null) { oLogger.WriteLine(string.Format("Will try to execute appropriate plugins..."), true); }

            // Invoke the export method according to plugin assembly definition
            string sAssemblyName, sClassName;
            int iPluginId = Convert.ToInt32(dr["PLUGINID"]);
            sAssemblyName = dr["ASSEMBLYDLL"].ToString();
            sClassName = dr["CLASSNAME"].ToString();
            object[] CtorArgs = new object[1] { iPluginId };
            BasePlugin oPlugin = null;

            if (oLogger != null)
            {
                oLogger.WriteLine(string.Format("Trying to activate plugin {0} (assembly: {1} classname: {2})...",
                    iPluginId, sAssemblyName, sClassName), true);
            }

            oPlugin = (BasePlugin)Made4Net.DataAccess.Reflection.ReflectionHelper.CreateInstance(sAssemblyName, sClassName, CtorArgs);
            //Added for RWMS-504
            if (oPlugin == null)
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("Plugin Not Found."), true); }
            }
            else
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("Plugin Found."), true); }
            }
            return oPlugin;
        }

        private XmlDocument GetXMLDocument(int evt, QMsgSender qSender, Made4Net.Shared.Logging.LogFile oLogger, out string pTransactionKey, string pConsignee)
        {
            ObjectProcessor oProcessor = new ObjectProcessor();
            XmlDocument oDoc = null;
            pTransactionKey = "";
            switch (evt)
            {
                case (int)WMS.Logic.WMSEvents.EventType.SnapShot:
                    oDoc = oProcessor.ExportInventrySnapShot("", oLogger,pConsignee);
                    pTransactionKey = "";
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.LoadUnitsChanged:
                    oDoc = oProcessor.ExportInventoryAdjustment("", qSender, oLogger);
                    pTransactionKey = qSender.Values["FROMLOAD"].ToString();
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.WorkOrderLoadConsumed:
                    oDoc = oProcessor.ExportConsume("", qSender, oLogger);
                    pTransactionKey = qSender.Values["FROMLOAD"].ToString();
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.WorkOrderLoadProduced:
                    oDoc = oProcessor.ExportProduce("", qSender, oLogger);
                    pTransactionKey = qSender.Values["TOLOAD"].ToString();
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.LoadStatusChanged:
                case (int)WMS.Logic.WMSEvents.EventType.LoadLoaded:
                case (int)WMS.Logic.WMSEvents.EventType.LoadPacked:
                case (int)WMS.Logic.WMSEvents.EventType.LoadStaged:
                case (int)WMS.Logic.WMSEvents.EventType.LoadVerified:
                case (int)WMS.Logic.WMSEvents.EventType.LoadUnPacked:
                    oDoc = oProcessor.ExportInventoryAdjustment("", qSender, oLogger);
                    pTransactionKey = qSender.Values["FROMLOAD"].ToString();
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.OrderShipped:
                case (int)WMS.Logic.WMSEvents.EventType.OrderStatusChanged:
                case (int)WMS.Logic.WMSEvents.EventType.OutboundOrderLoaded:
                case (int)WMS.Logic.WMSEvents.EventType.OutboundOrderPicked:
                case (int)WMS.Logic.WMSEvents.EventType.OutboundOrderPacked:
                case (int)WMS.Logic.WMSEvents.EventType.OutBoundOrderVerified:
                case (int)WMS.Logic.WMSEvents.EventType.OrderStaged:
                case (int)WMS.Logic.WMSEvents.EventType.CompleteOrder:
                case (int)WMS.Logic.WMSEvents.EventType.OutboundOrderCancelled:
                // RWMS-514 Begin: //Included to support Order Planned status for Outbound orders
                case (int)WMS.Logic.WMSEvents.EventType.OrderPalnned:
                case (int)WMS.Logic.WMSEvents.EventType.WavePlaned:
                    //RWMS-514: End
                    oDoc = oProcessor.ExportOutBound("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.InboundUpdated:
                case (int)WMS.Logic.WMSEvents.EventType.InboundCreated:
                    oDoc = oProcessor.ExportInBound("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.ReceiptClose:
                case (int)WMS.Logic.WMSEvents.EventType.ReceiptAtDock:
                case (int)WMS.Logic.WMSEvents.EventType.ReceiptUpdated:
                case (int)WMS.Logic.WMSEvents.EventType.ReceiptCreated:
                case (int)WMS.Logic.WMSEvents.EventType.ReceiptCancelled:
                    oDoc = oProcessor.ExportReceipt("", qSender, oLogger);
                    pTransactionKey = string.Format("{0}", qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.CreateLoad:
                case (int)WMS.Logic.WMSEvents.EventType.UnReceiveLoad:
                case (int)WMS.Logic.WMSEvents.EventType.LoadAttributeChanged:
                    oDoc = oProcessor.ExportLoad("", qSender, oLogger);
                    pTransactionKey = qSender.Values["TOLOAD"].ToString();
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.SkuUpdated:
                case (int)WMS.Logic.WMSEvents.EventType.SkuUOMSettled:
                case (int)WMS.Logic.WMSEvents.EventType.SkuCreated:
                    oDoc = oProcessor.ExportSku("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["SKU"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.CompanyCreated:
                case (int)WMS.Logic.WMSEvents.EventType.CompanyUpdated:
                    oDoc = oProcessor.ExportCompany("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1};{2}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString(), qSender.Values["NOTES"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.BillingChargePosted:
                    oDoc = oProcessor.ExportCharges("", qSender, oLogger);
                    pTransactionKey = string.Format("{0}", qSender.Values["CHARGEID"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentShip:
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentAtDock:
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentLoaded:
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentStatusChanged:
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentComplete:
                case (int)WMS.Logic.WMSEvents.EventType.ShipmentCreated:
                    oDoc = oProcessor.ExportShipment("", qSender, oLogger);
                    pTransactionKey = string.Format("{0}", qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.FlowThroughShipped:
                case (int)WMS.Logic.WMSEvents.EventType.FlowThroughUpdated:
                case (int)WMS.Logic.WMSEvents.EventType.FlowThroughCreated:
                    oDoc = oProcessor.ExportFlowthrough("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.CarrierCreated:
                case (int)WMS.Logic.WMSEvents.EventType.CarrierUpdated:
                    oDoc = oProcessor.ExportCarrier("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["CARRIER"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.TransShipmentCreated:
                case (int)WMS.Logic.WMSEvents.EventType.TransShipmentUpdated:
                    oDoc = oProcessor.ExportTransShipment("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;

                case (int)WMS.Logic.WMSEvents.EventType.PackingListShipped:
                    oDoc = oProcessor.ExportPackingList("", qSender, oLogger);
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;
                case (int)WMS.Logic.WMSEvents.EventType.WorkOrderExecuted:
                case (int)WMS.Logic.WMSEvents.EventType.WorkOrderCancelled:
                case (int)WMS.Logic.WMSEvents.EventType.WorkOrderCompleted:
                    string sWorkOrderType = qSender.Values["DOCUMENTTYPE"].ToString().ToUpper();
                    switch(sWorkOrderType)
                    {
                        case "ASMBL":
                            oDoc = oProcessor.ExportAssemblyWorkOrder("", qSender, oLogger);
                            break;
                        case "DISASMBL":
                            oDoc = oProcessor.ExportDisassemblyWorkOrder("", qSender, oLogger);
                            break;
                        case "VALUEADDED":
                            oDoc = oProcessor.ExportValueAddedWorkOrder("", qSender, oLogger);
                            break;
                        default:
                            oDoc = oProcessor.ExportAssemblyWorkOrder("", qSender, oLogger);
                            break;
                    }
                    pTransactionKey = string.Format("{0};{1}", qSender.Values["CONSIGNEE"].ToString(), qSender.Values["DOCUMENT"].ToString());
                    break;
                default:
                    break;
            }
            return oDoc;
        }

        #endregion

        #region Transmit Transactions

        private void TransmitTransaction(QMsgSender qSender, Made4Net.Shared.Logging.LogFile oLogger)
        {
            int transId = Convert.ToInt32(qSender.Values["TransactionId"]);
            if (oLogger != null) { oLogger.WriteLine(string.Format("Trying to Load transaction id {0}....", transId), true); }
            string sql = string.Format("select * from SCEXPERTCONNECTTRANSACTION where TRANSACTIONID = {0}", transId);
            DataTable dt = new DataTable();
            if (oLogger != null) { oLogger.WriteLine(string.Format("Trying to load transaction data"), true); }
            DataInterface.FillDataset(sql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
            if (dt.Rows.Count == 0)
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("Transaction not found in transaction table, exiting operation."), true); }
                return;
            }
            else
            {
                DataRow dr = dt.Rows[0];
                string data = dr["TRANSACTIONDATA"].ToString();
                string sTransactionType = dr["TRANSACTIONTYPE"].ToString();
                string TransactionKey = dr["TRANSACTIONBOKEY"].ToString();
                XmlDocument oDoc = new XmlDocument();
                if (sTransactionType.Equals("IMPORT", StringComparison.OrdinalIgnoreCase))
                {
                    BasePlugin oPlugin = new BasePlugin(Convert.ToInt32(dr["PLUGINID"]));
                    oPlugin.TransactionSet = "";
                    oDoc.LoadXml("<TransactionData><Data></Data></TransactionData>");
                    oDoc.SelectSingleNode("TransactionData/Data").InnerXml = data;
                    List<XmlDocument> oTrans = new List<XmlDocument>();
                    oTrans.Add(oDoc);
                    if (oLogger != null) { oLogger.WriteLine(string.Format("Trying to activate the import file function..."), true); }
                    SCExpertConnectImporter oImporter = new SCExpertConnectImporter();
                    ImportProcessResultsEventArgs Transaction = new ImportProcessResultsEventArgs(oTrans);
                    oImporter.ProcessImportPluginResults(oPlugin, Transaction, false,true);
                    if (oLogger != null) { oLogger.WriteLine(string.Format("Transaction transmitted to system."), true); }
                }
                else
                {
                    sql = string.Format("select SCEXPERTCONNECTPLUGINS.PLUGINID,ASSEMBLYDLL,CLASSNAME from SCEXPERTCONNECTPLUGINS inner join SCEXPERTCONNECTPLUGINTYPES on SCEXPERTCONNECTPLUGINTYPES.PLUGINTYPEID = SCEXPERTCONNECTPLUGINS.PLUGINTYPEID where PLUGINID = {0}", Convert.ToInt32(dr["PLUGINID"]));
                    DataTable dtPlugin = new DataTable();
                    DataInterface.FillDataset(sql, dtPlugin, false, Made4Net.Schema.Constants.CONNECTION_NAME);
                    foreach (DataRow drPlugin in dtPlugin.Rows)
                    {
                        // Invoke the export method according to plugin assembly definition
                        string sAssemblyName, sClassName;
                        int iPluginId = Convert.ToInt32(drPlugin["PLUGINID"]);
                        sAssemblyName = drPlugin["ASSEMBLYDLL"].ToString();
                        sClassName = drPlugin["CLASSNAME"].ToString();
                        object[] CtorArgs = new object[1] { iPluginId };
                        BasePlugin oPlugin = null;

                        if (oLogger != null)
                        {
                            oLogger.WriteLine(string.Format("Trying to activate plugin {0} (assembly: {1} classname: {2})...",
                            iPluginId, sAssemblyName, sClassName), true);
                        }

                        oPlugin = (BasePlugin)Made4Net.DataAccess.Reflection.ReflectionHelper.CreateInstance(sAssemblyName, sClassName, CtorArgs);
                        int res = 0;
                        oDoc.InnerXml = data;
                        XmlDeclaration xmlDeclaration = oDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                        XmlElement root = oDoc.DocumentElement;
                        oDoc.InsertBefore(xmlDeclaration, root);
                        // Transform the XML to required format
                        XmlDocument XSLTransformedXmlDoc = null;
                        if (oPlugin.ExportTranslationFile != null && oPlugin.ExportTranslationFile != "")
                        {
                            Made4Net.Shared.XSLTransformer oTransformer = new XSLTransformer();
                            XSLTransformedXmlDoc = oTransformer.Transform(oDoc, oPlugin.ExportTranslationFile);
                            if (oLogger != null) { oLogger.WriteLine(string.Format("Output XML file Translated according to plugin translation file..."), true); }
                        }
                        else
                            XSLTransformedXmlDoc = oDoc;


                        res = oPlugin.Export(XSLTransformedXmlDoc);

                        SCExpertConnectTransactionStatus.TransactionStatus sTransStatus;
                        if (res != 0)
                        {
                            sTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Fail;
                            //Added for PWMS-732 Start
                            if (oLogger != null) { oLogger.WriteLine(string.Format("Tranasaction Status :" + sTransStatus), true); }
                            //Added for PWMS-732 End
                        }
                        else
                        {
                            sTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Success;
                            //Added for PWMS-732 Start
                            if (oLogger != null) { oLogger.WriteLine(string.Format("Tranasaction Status :" + sTransStatus), true); }
                            //Added for PWMS-732 End
                        }

                        // Add record for the connect transaction audit table
                        SCExpertConnectTransaction oTransaction = new SCExpertConnectTransaction();
                         //Added for RWMS-1509 and RWMS-1502
                        oTransaction.PostTransaction(SCExpertConnectTransactionTypes.TransactionType.Export, oPlugin.PluginId,
                            sTransStatus, "", oPlugin.BOType, dr["TRANSACTIONBOKEY"].ToString(), oDoc.InnerXml.ToString(), "", out stransactionID);
                        //RWMS-1434 Strat
                        SendAlertsToRecepients(oPlugin, sTransStatus, TransactionKey, stransactionID, oLogger);
                        //RWMS-1434 End
                        //Ended for RWMS-1509 and RWMS-1502
                        if (oLogger != null)
                        {
                            //RWMS-2563 START
                            oLogger.WriteLine(string.Format("Transaction Details as below:"), true);
                            oLogger.WriteLine(string.Format("Transaction key: ") + dr["TRANSACTIONBOKEY"].ToString(), true);
                            oLogger.WriteLine(string.Format("Transaction Status: ") + sTransStatus.ToString(), true);

                            if (sTransStatus == SCExpertConnectTransactionStatus.TransactionStatus.Fail)
                            {
                                oLogger.WriteLine(string.Format("Failed to process Transaction.Writing XML document to log:"), true);
                                oLogger.WriteLine(XSLTransformedXmlDoc.InnerXml.ToString(), true);
                            }
                            //RWMS-2563 END

                            oLogger.WriteLine(string.Format("Message processed successfully..."), true);
                        }
                    }
                }
            }
        }

        #endregion

        #region Plugin's Data Filters

        private bool ShouldFilterTransaction(BasePlugin oPlugin, XmlDocument oTransactionDoc, Made4Net.Shared.Logging.LogFile oLogger)
        {
            if (oLogger != null) { oLogger.WriteLine(string.Format("Checking data filters for current transaction."), true); }

            if (oPlugin.ExportTransactionContentFilter == null || oPlugin.ExportTransactionContentFilter == string.Empty)
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("Should not filter transaction, filter parameter is blank."), true); }
                return false;
            }
            string[] sFiltersArray = oPlugin.ExportTransactionContentFilter.Split(new char[] { ';' });
            string[] sCurrentFilter;
            for (int i = 0; i < sFiltersArray.Length; i++)
            {
                try
                {
                    if (sFiltersArray[i] != string.Empty)
                    {
                        if (oLogger != null) { oLogger.WriteLine(string.Format("Checking if transaction should be filtered according to definition: {0}", sFiltersArray[i]), true); }
                        sCurrentFilter = sFiltersArray[i].Split(new char[] { ',' });
                        if (oTransactionDoc.SelectSingleNode(sCurrentFilter[0]) == null)
                        {
                            if (oLogger != null) { oLogger.WriteLine(string.Format("Node {0} Not found in transaction file. Filter will not be applied.", sCurrentFilter[0]), true); }
                        }
                        else if (oTransactionDoc.SelectSingleNode(sCurrentFilter[0]).InnerText.Equals(sCurrentFilter[1], StringComparison.OrdinalIgnoreCase))
                        {
                            if (oLogger != null) { oLogger.WriteLine(string.Format("Match found according to filter {0}. Transaction will be filtered.", sCurrentFilter[0]), true); }
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (oLogger != null)
                    {
                        oLogger.WriteLine("Error Occured while matching filters in transaction data. Error details: ", true);
                        oLogger.WriteLine(ex.ToString(), true);
                    }
                }
            }
            return false;
        }

        #endregion

        #region Logging

        public Made4Net.Shared.Logging.LogFile InitLogger(string pLogFileName)
        {
            Made4Net.Shared.Logging.LogFile oLogger = null;
          //  if (System.Configuration.ConfigurationManager.AppSettings.Get("EnableLogs") == "1")
            if (Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectUseLogs) == "1")
            {
               // string sLogPath = System.Configuration.ConfigurationManager.AppSettings.Get("LogPath");
                string sLogPath = Made4Net.DataAccess.Util.GetInstancePath() + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectLogPath;  //PWMS-817
                string logName = string.Format("{0}_{1}_{2}", pLogFileName, DateTime.Now.ToString("yyyyMMdd hhmmss"), new Random().Next(100000).ToString());
                oLogger = new Made4Net.Shared.Logging.LogFile(sLogPath, logName, true);
                oLogger.WriteSeperator();
            }
            return oLogger;
        }

        #endregion

        #region Accessories
        //Added for RWMS-1509 and RWMS-1502
        private void SendAlertsToRecepients(BasePlugin oPlugin, SCExpertConnectTransactionStatus.TransactionStatus sTransStatus, string sTransactionKey, int stransactionID, Made4Net.Shared.Logging.LogFile oLogger)
        //Ended for RWMS-1509 and RWMS-1502
        {
            // Send alert by email if needed
            bool ProcessSucceeded = true;

            StringBuilder oStringbuilder = new StringBuilder();
            //Added for RWMS-1509 and RWMS-1502
            oStringbuilder.AppendLine(string.Format("Results for processing export request for warehouse {0}:", oPlugin.WarehouseId));
            //Ended for RWMS-1509 and RWMS-1502
            oStringbuilder.AppendLine(string.Format("Activity date and time: {0} {1}", DateTime.Now.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_DateFormat")), DateTime.Now.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_TimeFormat"))));
            oStringbuilder.AppendLine("");
            if (sTransStatus == SCExpertConnectTransactionStatus.TransactionStatus.Success)
            {
                oStringbuilder.AppendLine(string.Format("Transaction business object key (of type {1}) {0} succeeded.", sTransactionKey, oPlugin.BOType));
            }
            else
            {
                oStringbuilder.AppendLine(string.Format("Transaction business object key (of type {1}) {0} failed.", sTransactionKey, oPlugin.BOType));

                ProcessSucceeded = false;
            }
            string sAlertProcessResult = oPlugin.AlertProcessResult.ToLower();
            switch (sAlertProcessResult)
            {
                case "none":
                    break;
                case "onerror":
                    if (!ProcessSucceeded)
                         // RWMS-1434 start
                        // RWMS-1509 and RWMS-1502 Start
                        oPlugin.SendAlertsOnProcessComplete("Export Process Notification", oStringbuilder.ToString(), SCExpertConnectUtils.CreateMailAttachments(sTransactionKey, stransactionID, oLogger));
                    // RWMS-1509 and RWMS-1502 end
                // RWMS-1434 end
                    break;
                case "always":
                    // RWMS-1434 start
                    // RWMS-1509 and RWMS-1502 Start
                    oPlugin.SendAlertsOnProcessComplete("Export Process Notification", oStringbuilder.ToString(), SCExpertConnectUtils.CreateMailAttachments(sTransactionKey, stransactionID, oLogger));
                    // RWMS-1509 and RWMS-1502 end
                // RWMS-1434 end
                    break;
                default:
                    break;
            }
        }

        protected void PrintMessageContent(QMsgSender qSender, Made4Net.Shared.Logging.LogFile oLogger)
        {
            if (oLogger != null)
            {
                oLogger.WriteSeperator();
                for (int i = 0; i < qSender.Values.Count - 1; i++)
                    oLogger.WriteLine(string.Format("Field {0} : {1}", qSender.Values.Keys[i], qSender.Values[i]), true);
                oLogger.WriteSeperator();
            }
        }

        private void SetConnection(string pWarehouseId)
        {
            Warehouse.setCurrentWarehouse(pWarehouseId);
            DataInterface.ConnectionName = Warehouse.WarehouseConnection;
        }

        #endregion
    }

    #endregion
}
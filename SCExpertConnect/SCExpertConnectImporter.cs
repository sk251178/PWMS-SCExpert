using ExpertObjectMapper;
using Made4Net.DataAccess;
using Made4Net.DataAccess.Reflection;
using Made4Net.Shared;
using Made4Net.Shared.Logging;
using SCExpertConnectPlugins.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Xml;

namespace SCExpertConnect
{
    public class SCExpertConnectImporter
    {
        protected List<BasePlugin> mPluginCollection = new List<BasePlugin>();

        public SCExpertConnectImporter()
        {
        }

        #region Init Plugins

        public void InitPlugins()
        {
            using (LogFile oLogger = InitLogger("SCExpertConnectImportInit"))
            {
                try
                {
                    oLogger.SafeWriteLine(string.Format("SCExpert connect will try to initialize plugins for import process..."));
                    oLogger.SafeWriteLine(string.Format("Current Thread: {0}", Thread.CurrentThread.ManagedThreadId));
                    string sSql = string.Format("select PLUGINID,ASSEMBLYDLL,CLASSNAME from SCEXPERTCONNECTPLUGINS inner join SCEXPERTCONNECTPLUGINTYPES on SCEXPERTCONNECTPLUGINTYPES.PLUGINTYPEID = SCEXPERTCONNECTPLUGINS.PLUGINTYPEID where PLUGINTRANSACTIONTYPE = 'IMPORT'");
                    DataTable dt = new DataTable();
                    DataInterface.FillDataset(sSql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
                    mPluginCollection.Clear();
                    BasePlugin oPlugin = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sAssemblyName, sClassName;
                        int iPluginId = Convert.ToInt32(dr["PLUGINID"]);
                        sAssemblyName = dr["ASSEMBLYDLL"].ToString();
                        sClassName = dr["CLASSNAME"].ToString();
                        object[] CtorArgs = new object[1] { iPluginId };
                        try
                        {
                            oPlugin = (BasePlugin)ReflectionHelper.CreateInstance(sAssemblyName, sClassName, CtorArgs);
                            Subscribe(oPlugin);
                            mPluginCollection.Add(oPlugin);
                            oLogger.SafeWriteLine(string.Format("Plugin {0} (ID:{1}) Initialized...( {2} , {3} )",
                                oPlugin.PluginDescription, iPluginId.ToString(), sAssemblyName, sClassName));
                            oPlugin.InitLogger(sClassName);
                        }
                        catch (Exception ex)
                        {
                            oLogger.SafeWriteSeparator();
                            oLogger.SafeWriteLine(string.Format("Initialization for Plugin {0} failed: ", iPluginId.ToString()));
                            oLogger.SafeWriteLine(string.Format("Exception details: {0}", ex.ToString()));
                            oLogger.SafeWriteSeparator();
                        }
                    }
                    oLogger.SafeWriteLine(string.Format("Initialization completed successfully! "));
                    foreach (BasePlugin tempPlugin in mPluginCollection)
                    {
                        Thread _pluginTh = new Thread(tempPlugin.Import);
                        _pluginTh.Start();
                        _pluginTh.Join();
                        oLogger.SafeWriteLine(string.Format("Starting Thread: {0}", _pluginTh.ManagedThreadId));
                    }
                }
                catch (Exception ex)
                {
                    oLogger.SafeWriteSeparator();
                    oLogger.SafeWriteLine("Error occured while initializing plugins: " + ex.ToString());
                    oLogger.SafeWriteSeparator();
                }
            }
        }

        // Methods for handling the event raise by each plugin to notify the connect that process phase has completed.
        public void Subscribe(BasePlugin oPlugin)
        {
            oPlugin.ImportCompleted += new BasePlugin.ImportProcessCompleteHandler(ProcessImportPluginResults);
        }

        public void Dispose()
        {
            if (mPluginCollection != null)
            {
                try
                {
                    foreach (BasePlugin oPlugin in mPluginCollection)
                    {
                        oPlugin.Dispose();
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Process Plugin Result
        // Handle the event raised by the plugins
        public void ProcessImportPluginResults(object oPlugin, ImportProcessResultsEventArgs ImportResults, bool pShouldSendBackNotification, bool ReTransmitTransaction)
        {
            System.Diagnostics.Debugger.Break();
            using (LogFile oLogger = InitLogger(""))
            {
                if (ImportResults.lstResultXmlDocs != null)
                {
                    // Set the required connection as defined in the plugin
                    BasePlugin plugin = ((BasePlugin)oPlugin);
                    Dictionary<string, DataMapperProcessResult> mObjectDataMapperResult = new Dictionary<string, DataMapperProcessResult>();
                    SetConnection(plugin.WarehouseId);
                    bool bTransactionRequired = false;
                    string sTransactionSet = plugin.TransactionSet;
                    string sTransactionKey = string.Empty;
                    int stransactionID = 0;

                    // Process Result for each file
                    foreach (XmlDocument oXmlDoc in ImportResults.lstResultXmlDocs)
                    {
                        oLogger.SafeWriteLine(string.Format("Application Thread Id {0}.", Thread.CurrentThread.ManagedThreadId), true);
                        oLogger.SafeWriteLine(string.Format("Import phase started for xml document."), true);

                        oLogger.SafeWriteLine(string.Format("Conection was set to warehouse {0} (Connection name:{1})", Warehouse.WarehouseName, Warehouse.WarehouseConnection), true);
                        oLogger.SafeWriteLine(string.Format("Trying to parse the received XML and activate DataMapper on each transaction in file...."), true);

                        // Transform the received XML according to the plugin XSL definitions
                        XmlDocument XSLTransformedXmlDoc = null;
                        if (!ReTransmitTransaction && plugin.ImportTranslationFile != null && plugin.ImportTranslationFile != "")
                        {
                            Made4Net.Shared.XSLTransformer oTransformer = new XSLTransformer();
                            XSLTransformedXmlDoc = oTransformer.Transform(oXmlDoc, plugin.ImportTranslationFile);
                            oLogger.SafeWriteLine(string.Format("Input XML file Translated according to plugin translation file..."), true);
                        }
                        else
                            XSLTransformedXmlDoc = oXmlDoc;

                        int tempTransaction = 1;
                        XmlDocument tmpXmlDoc = new XmlDocument();

                        if (!ReTransmitTransaction)
                            XSLTransformedXmlDoc.RemoveChild(XSLTransformedXmlDoc.FirstChild);

                        tmpXmlDoc = XSLTransformedXmlDoc;
                        oLogger.SafeWriteLine(string.Format("Total transactions receievd in the file from the plugin: {0}, Transaction Set: {1}",
                            tmpXmlDoc.FirstChild.ChildNodes.Count.ToString(), sTransactionSet), true);
                        oLogger.SafeWriteLine(string.Format("XML : {0}", tmpXmlDoc.InnerXml), true);

                        foreach (XmlNode oXmlNode in tmpXmlDoc.FirstChild.ChildNodes)
                        {
                            try
                            {
                                oLogger.SafeWriteLine(string.Format("Working on transaction #{0}", tempTransaction.ToString()), true);
                                oLogger.SafeWriteLine(string.Format("The Xml node for the current transaction: {0}", oXmlNode.InnerXml), true);
                                if (ShouldFilterTransaction(plugin, oXmlNode, oLogger))
                                {
                                    tempTransaction += 1;
                                    continue;
                                }

                                sTransactionKey = GetTransactionKey(plugin, oXmlNode);
                                if (string.IsNullOrEmpty(sTransactionKey))
                                {
                                    oLogger.SafeWriteLine(string.Format("Query : 'Select * from SCEXPERTCONNECTPLUGINPARAMS where pluginid = {0}' yielded no result.", plugin.PluginId), true);
                                }

                                SCExpertConnectTransactionStatus.TransactionStatus oTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Success;
                                oLogger.SafeWriteSeparator();
                                XmlDocument oTempTransactionXML = new XmlDocument();
                                oTempTransactionXML.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><BUSINESSOBJECT><BOTYPE>{0}</BOTYPE></BUSINESSOBJECT>", ((BasePlugin)oPlugin).BOType));
                                oTempTransactionXML.SelectSingleNode("BUSINESSOBJECT").AppendChild(oTempTransactionXML.ImportNode(oXmlNode, true));

                                ObjectProcessor.DataMapperTransactionResult res;

                                // If shouldnt roll back on failure anyway - Do not run the mapper within a transaction
                                if (!plugin.CommitBOOnBoElementFailure)
                                {
                                    bTransactionRequired = true;
                                    Made4Net.DataAccess.DataInterface.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                                }
                                string sOutErrorMessage = string.Empty;
                                res = ProcessFile(oTempTransactionXML, oLogger, ref sOutErrorMessage);
                                switch (res)
                                {
                                    case ObjectProcessor.DataMapperTransactionResult.Success:
                                        //need to move the txt file to Success folder
                                        oLogger.SafeWriteLine(string.Format("Import result from Data Mapper: {0}. Committing transaction.",
                                            ObjectProcessor.TransactionResultToString(res)), true);
                                        if (bTransactionRequired)
                                            Made4Net.DataAccess.DataInterface.CommitTransaction();
                                        break;
                                    case ObjectProcessor.DataMapperTransactionResult.BOElementFailed:
                                        //need to move the txt file to Failed folder
                                        if (((BasePlugin)oPlugin).CommitBOOnBoElementFailure)
                                        {
                                            oLogger.SafeWriteLine(string.Format("Import result from Data Mapper: {0}. Committing transaction according to plugin definition(CommitBOOnBoElementFailure property)", ObjectProcessor.TransactionResultToString(res)), true);
                                            oTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Fail;
                                            if (bTransactionRequired)
                                                Made4Net.DataAccess.DataInterface.CommitTransaction();
                                        }
                                        else
                                        {
                                            oLogger.SafeWriteLine(string.Format("Import result from Data Mapper: {0}. Rolling back transaction according to plugin definition(CommitBOOnBoElementFailure property)", ObjectProcessor.TransactionResultToString(res)), true);
                                            oTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Fail;
                                            if (bTransactionRequired)
                                                Made4Net.DataAccess.DataInterface.RollBackTransaction();
                                        }
                                        break;
                                    case ObjectProcessor.DataMapperTransactionResult.BOFailed:
                                        //need to move the txt file to Failed folder
                                        oLogger.SafeWriteLine(string.Format("Import result from Data Mapper: {0}. Rolling back transaction.", ObjectProcessor.TransactionResultToString(res)), true);
                                        oTransStatus = SCExpertConnectTransactionStatus.TransactionStatus.Fail;
                                        if (bTransactionRequired)
                                            Made4Net.DataAccess.DataInterface.RollBackTransaction();
                                        break;
                                    default:
                                        if (bTransactionRequired)
                                            Made4Net.DataAccess.DataInterface.CommitTransaction();
                                        break;
                                }

                                // Add record for the connect transaction audit table
                                SCExpertConnectTransaction oTransaction = new SCExpertConnectTransaction();
                                oTransaction.PostTransaction(SCExpertConnectTransactionTypes.TransactionType.Import, plugin.PluginId,
                                oTransStatus, sTransactionSet, plugin.BOType, sTransactionKey, oXmlNode.InnerXml.ToString(), sOutErrorMessage, out stransactionID);
                                // Add the transaction key and result to the process result set to return to plugin object
                                if (sTransactionKey != string.Empty && !mObjectDataMapperResult.ContainsKey(sTransactionKey))
                                {
                                    DataMapperProcessResult oProcRes = new DataMapperProcessResult(res, sOutErrorMessage);
                                    mObjectDataMapperResult.Add(sTransactionKey, oProcRes);

                                    oLogger.SafeWriteLine(string.Format("Transaction key {0} added to return object collection.. {1}", sTransactionKey, oProcRes.TransactionErrorMessage), true);

                                    foreach (KeyValuePair<string, ExpertObjectMapper.DataMapperProcessResult> kvp in mObjectDataMapperResult)
                                    {
                                        if (kvp.Value.TransactionResult != ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success)
                                        {
                                            //Commented for RWMS-408
                                            //oLogger.SafeWriteLine(string.Format("Transaction ID {0} failed by the SCExpert Connect.", kvp.Key), true);
                                            //oLogger.SafeWriteLine(string.Format("Error message and description:", kvp.Value.TransactionErrorMessage), true);
                                            //End Commented for RWMS-408
                                            //Added for RWMS-408
                                            oLogger.SafeWriteLine(string.Format("Transaction ID {0} failed by the SCExpertConnectImporter 1.", kvp.Key), true);
                                            oLogger.SafeWriteLine(string.Format("Error message and description:{0}", kvp.Value.TransactionErrorMessage), true);
                                            //End Added for RWMS-408
                                        }
                                    }
                                }
                                tempTransaction += 1;
                            }
                            catch (Exception ex)
                            {
                                oLogger.SafeWriteSeparator();
                                oLogger.SafeWriteLine("Import to Expert failed. Exception details: " + ex.ToString(), true);
                                oLogger.SafeWriteSeparator();
                                if (bTransactionRequired)
                                    Made4Net.DataAccess.DataInterface.RollBackTransaction();
                            }

                        }


                        try
                        {
                            // Send an email alerting with the process result if neccessary
                            if (mObjectDataMapperResult.Count > 0)
                                // RWMS-1434 start
                                //Added for RWMS-1509 and RWMS-1502
                                SendAlertsToRecepients(plugin, sTransactionSet, mObjectDataMapperResult, sTransactionKey, stransactionID, oLogger);
                            //Ended for RWMS-1509 and RWMS-1502
                            // RWMS-1434 start End
                        }
                        catch (Exception ex)
                        {
                            oLogger.SafeWriteLine(string.Format("Error occured while sending a message to alert recipients: {0}", ex.ToString()), true);
                        }
                    }
                    try
                    {
                        if ((sTransactionSet != string.Empty) && pShouldSendBackNotification)
                        {
                            using (LogFile Logger2 = InitLogger("ResultLog"))
                            {
                                //Logging
                                foreach (KeyValuePair<string, ExpertObjectMapper.DataMapperProcessResult> kvp in mObjectDataMapperResult)
                                {
                                    if (kvp.Value.TransactionResult != ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success)
                                    {
                                        Logger2.WriteLine(string.Format("Transaction ID {0} failed by the SCExpertConnectImporter 2.", kvp.Key), true);
                                        Logger2.WriteLine(string.Format("Error message and description:{0}", kvp.Value.TransactionErrorMessage), true);
                                        //End Added for RWMS-408
                                    }
                                }
                            }
                            plugin.ProcessResultFromSCExpertConect(sTransactionSet, mObjectDataMapperResult);
                        }
                    }
                    catch { }
                }
            }
        }

        private void SendAlertsToRecepients(BasePlugin oPlugin, string pTransactionSet, Dictionary<string, ExpertObjectMapper.DataMapperProcessResult> pObjectDataMapperResult, string sTransactionKey, int stransactionID, Made4Net.Shared.Logging.LogFile oLogger)
        {
            // Send alert by email if needed
            bool ProcessSucceeded = true;

            StringBuilder oStringbuilder = new StringBuilder();
            oStringbuilder.AppendLine(string.Format("Results for processing import request for transaction set: {0} for warehouse {1}", pTransactionSet, oPlugin.WarehouseId));
            oStringbuilder.AppendLine(string.Format("Activity date and time: {0} {1}", DateTime.Now.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_DateFormat")), DateTime.Now.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_TimeFormat"))));
            oStringbuilder.AppendLine(string.Format("Total transactions processed within the current transaction set: {0}", pObjectDataMapperResult.Count.ToString()));
            oStringbuilder.AppendLine("");
            foreach (KeyValuePair<string, ExpertObjectMapper.DataMapperProcessResult> kvp in pObjectDataMapperResult)
            {
                if (kvp.Value.TransactionResult == ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success)
                {
                    oStringbuilder.AppendLine(string.Format("Transaction ID {0} succeeded.", kvp.Key));
                }
                else
                {
                    oStringbuilder.AppendLine(string.Format("Transaction ID {0} failed.", kvp.Key));
                    ProcessSucceeded = false;
                }
            }
            string sAlertProcessResult = oPlugin.AlertProcessResult.ToLower();
            switch (sAlertProcessResult)
            {
                case "none":
                    break;
                case "onerror":
                    if (!ProcessSucceeded)
                    {
                        oPlugin.SendAlertsOnProcessComplete("Import Process Notification", oStringbuilder.ToString(),
                            SCExpertConnectUtils.CreateMailAttachments(sTransactionKey, stransactionID, oLogger));
                    }
                    break;
                case "always":
                    oPlugin.SendAlertsOnProcessComplete("Import Process Notification", oStringbuilder.ToString(),
                        SCExpertConnectUtils.CreateMailAttachments(sTransactionKey, stransactionID, oLogger));
                    break;
                default:
                    break;
            }
        }


        #region Plugin's Data Filters

        private bool ShouldFilterTransaction(BasePlugin oPlugin, XmlNode oTransactionNode, Made4Net.Shared.Logging.LogFile oLogger)
        {
            oLogger.SafeWriteLine(string.Format("Checking data filters for current transaction."), true);


            if (oPlugin.ImportTransactionContentFilter == null || oPlugin.ImportTransactionContentFilter == string.Empty)
            {
                oLogger.SafeWriteLine(string.Format("Should not filter transaction, filter parameter is blank."), true);

                return false;
            }
            string[] sFiltersArray = oPlugin.ImportTransactionContentFilter.Split(new char[] { ';' });
            string[] sCurrentFilter;
            for (int i = 0; i < sFiltersArray.Length; i++)
            {
                try
                {
                    if (sFiltersArray[i] != string.Empty)
                    {
                        oLogger.SafeWriteLine(string.Format("Checking if transaction should be filtered according to definition: {0}", sFiltersArray[i]), true);
                        sCurrentFilter = sFiltersArray[i].Split(new char[] { ',' });
                        if (oTransactionNode.SelectSingleNode(sCurrentFilter[0]) == null)
                        {
                            oLogger.SafeWriteLine(string.Format("Node {0} Not found in transaction file. Filter will not be applied.", sCurrentFilter[0]), true);
                        }
                        else if (oTransactionNode.SelectSingleNode(sCurrentFilter[0]).InnerText.Equals(sCurrentFilter[1], StringComparison.OrdinalIgnoreCase))
                        {
                            oLogger.SafeWriteLine(string.Format("Match found according to filter {0}. Transaction will be filtered.", sCurrentFilter[0]), true);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    oLogger.SafeWriteLine("Error Occured while matching filters in transaction data. Error details: ", true);
                    oLogger.SafeWriteLine(ex.ToString(), true);
                }
            }
            return false;
        }

        #endregion

        private string GetTransactionKey(BasePlugin oPlugin, XmlNode oXmlDoc)
        {
            try
            {
                string sKey = string.Empty;
                foreach (string key in oPlugin.KeysCollection)
                {
                    sKey = sKey + "~" + oXmlDoc.SelectSingleNode(key).InnerText;
                }
                return sKey.TrimStart(new char[] { '~' });
            }
            catch
            {
                return string.Empty;
            }
        }

        public ObjectProcessor.DataMapperTransactionResult ProcessFile(XmlDocument oXmlDoc, Made4Net.Shared.Logging.LogFile oLogger, ref string OutErrorMessage)
        {
            ObjectProcessor oObjProc = new ObjectProcessor();
            ObjectProcessor.DataMapperTransactionResult ProcResult = ObjectProcessor.DataMapperTransactionResult.Success;
            try
            {
                string sObjeType = oXmlDoc.SelectSingleNode("BUSINESSOBJECT/BOTYPE").InnerText;
                switch (sObjeType.ToUpper())
                {
                    case ExpertObjectTypes.COMPANY:
                        ProcResult = oObjProc.ProcessCompany(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.CARRIER:
                        ProcResult = oObjProc.ProcessCarrier(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.SKU:
                        ProcResult = oObjProc.ProcessSku(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.OUTBOUND:
                        ProcResult = oObjProc.ProcessOutbound(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.INBOUND:
                        ProcResult = oObjProc.ProcessInbound(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.ASSEMBLY:
                        ProcResult = oObjProc.ProcessAssemblyOrder(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.DISASSEMBLY:
                        ProcResult = oObjProc.ProcessDisAssemblyOrder(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.VALUEADDED:
                        ProcResult = oObjProc.ProcessValueAddedOrder(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.RECEIPT:
                        ProcResult = oObjProc.ProcessReceipt(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.CONTACT:
                        ProcResult = oObjProc.ProcessContact(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.FLOWTHROUGH:
                        ProcResult = oObjProc.ProcessFlowthrough(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.SHIPMENT:
                        ProcResult = oObjProc.ProcessShipment(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.TRANSSHIPMENT:
                        ProcResult = oObjProc.ProcessTransshipment(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                    case ExpertObjectTypes.DBTABLE:
                        ProcResult = oObjProc.ProcessGenericTable(oXmlDoc, oLogger, ref OutErrorMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                oLogger.SafeWriteSeparator();
                oLogger.SafeWriteLine("Import to Expert exception : " + ex.ToString(), true);
                oLogger.SafeWriteSeparator();
            }

            return ProcResult;
        }

        private void SetConnection(string pWarehouseId)
        {
            Warehouse.setCurrentWarehouse(pWarehouseId);
            DataInterface.ConnectionName = Warehouse.WarehouseConnection;
        }

        #endregion

        #region Logging
        public LogFile InitLogger(string pLogFileName)
        {
            LogFile oLogger = null;
            // if (System.Configuration.ConfigurationManager.AppSettings.Get("EnableLogs") == "1")
            if (Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectUseLogs) == "1")
            {
                // string sLogPath = System.Configuration.ConfigurationManager.AppSettings.Get("LogPath");
                string sLogPath = Made4Net.DataAccess.Util.GetInstancePath() + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectLogPath;  //PWMS-817
                string logName = string.Format("{0}_{1}_{2}", pLogFileName, DateTime.Now.ToString("yyyyMMdd hhmmss"), new Random().Next(100000).ToString());
                oLogger = new LogFile(sLogPath, logName, true);
                oLogger.SafeWriteSeparator();
            }
            return oLogger;
        }

        #endregion
    }
}
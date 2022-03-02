using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using System.Xml;
using RWMSIntegrationService.BO;
using Made4Net.Shared;
using Made4Net.DataAccess;
using ExpertObjectMapper;
using System.IO;
using RWMSIntegrationService.Interfaces;

//Commented for RWMS-1185 Start
//using NCR.GEMS.Core.DataCaching;
//using NCR.GEMS.WMS.Core.WmsQueryCaching;
//Commented for RWMS-1185 End

namespace RWMSIntegrationService
{
    public partial class RWMSIntegrationService : ServiceBase
    {
       #region Members
        private string _logType = string.Empty;
        Made4Net.Shared.Logging.LogFile mLogger = new Made4Net.Shared.Logging.LogFile();
        protected string mLicensUser = "RWMS Integration Service";
        protected Thread ImporterThread;
       // protected RWMSIntegrationExporter mExporter = null;

        #endregion

        #region Service Handlers
        public RWMSIntegrationService()
        {
            InitializeComponent();
        }

        private void InitLogger()
        {
            string mLogPath = System.Configuration.ConfigurationManager.AppSettings.Get("LogPath");
            //string UseLogs = System.Configuration.ConfigurationSettings.AppSettings.Get("UseLogs");
            string UseLogs = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSIntegrationServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.PWMSIntegrationServiceUseLogs);
            if ((mLogPath != string.Empty) && (string.Equals(UseLogs,"1") ))
            {
                string LogFilePrefix = "rds";

                string logName = string.Format("{0}_{1}_{2}", LogFilePrefix, DateTime.Now.ToString("yyyyMMdd hhmmss"), new Random().Next(100000).ToString());
                mLogger = new Made4Net.Shared.Logging.LogFile(mLogPath, logName, true);
                mLogger.WriteSeperator();
            }
        }

        protected void writeToLog(string pText)
        {
            if (mLogger == null)
                return;
            mLogger.WriteLine(pText, true);
        }


        protected override void OnStart(string[] args)
        {
            //System.Threading.Thread.Sleep(20000);
            try
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.InitializeMonitor(WmsQueryCachingMonitorType.Integration);
                //Commented for RWMS-1185 End

                ImporterThread = new Thread(InitPlugins);
                ImporterThread.Start();
            }
            catch (Exception ex)
            {
                //Commented for RWMS-1185 Start
                //GemsQueryCachingMonitor.StopMonitor();
                //Commented for RWMS-1185 End

                writeToLog("error occured:");
                writeToLog(ex.ToString());
                throw ex;
            }
        }

        protected override void OnStop()
        {
            //Commented for RWMS-1185 Start
            //GemsQueryCachingMonitor.StopMonitor();
            //Commented for RWMS-1185 End

            writeToLog("service stopped ");
           // mExporter.StopQueue();
        }

        #endregion

        #region ImportPlugins

        private void InitPlugins()
        {


            XmlDocument oXmlDoc = GetServiceConfigurationFile();
            if (!oXmlDoc.Equals(null))
            {

                InitActivePlugins(oXmlDoc);
            }

        }
        /// <summary>
        /// reads the RWMS plugins xml configuration file
        /// and initialize them accordingly
        /// </summary>
        /// <param name="oXmlDoc"></param>
        private void InitActivePlugins(XmlDocument oXmlDoc)
        {
            initImportPlugins(oXmlDoc.SelectNodes("/PLUGINSCONFIG/IMPORT/*[Active[text()='1']]"));
            initExportPlugins(oXmlDoc.SelectNodes("/PLUGINSCONFIG/EXPORT/*[Active[text()='1']]"));
        }

        private void initExportPlugins(XmlNodeList pNodesList)
        {
            for (int i = 0; i < pNodesList.Count; i++)
            {
                XmlNode xn = pNodesList.Item(i);

                BaseIntegrationExportPlugin plugin = null;

                switch (xn.FirstChild.ParentNode.Name.ToUpper())
                {
                    case "SKU":
                        plugin = new SKUExporter(xn);
                        break;
                    case "CARRIER":
                        plugin = new CarrierExporter(xn);
                        break;
                    case "INBOUNDORDER":
                        plugin = new InboundOrderExporter(xn);
                        break;
                    case "COMPANY":
                        plugin = new CompanyExporter(xn);
                        break;
                    case "FLOWTHROUGH":
                        plugin = new FlowThroughExporter(xn);
                        break;
                    case "TRANSSHIPMENT":
                        plugin = new TransShipmentExporter(xn);
                        break;
                    case "OUTBOUNDORDER":
                        plugin = new OutboundOrderExporter(xn);
                        break;
                    default:
                        break;
                }

                if(plugin != null)
                    plugin.Start(xn.FirstChild.ParentNode.Name.ToUpper());
            }
        }

        private void initImportPlugins(XmlNodeList pNodesList)
        {
            for (int i = 0; i < pNodesList.Count; i++)
            {
                XmlNode xn = pNodesList.Item(i);

                BaseIntegrationImportPlugin plugin = null;

                switch (xn.FirstChild.ParentNode.Name.ToUpper())
                {
                    case "RECEIPT":
                        plugin = new ReceiptImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    case "INBOUND":
                        plugin = new InboundOrderImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    case "OUTBOUNDORDER":
                        plugin = new OutboundOrderImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    case "TRANSSHIPMENT":
                        plugin = new TransshipmentImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    //case "FLOWTHROUGH":
                    //    plugin = new FlowthroughImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                    //    break;
                    case "FLOWTHROUGH_INBOUND":
                        plugin = new FlowThroughInboundImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    case "FLOWTHROUGH_OUTBOUND":
                        plugin = new FlowThroughOutboundImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    case "CARRIER":
                        plugin = new CarrierImporter(xn, xn.SelectSingleNode("CONSIGNEE").InnerText);
                        break;
                    default:
                        break;
                }

                if (plugin != null)
                    plugin.Start(xn.FirstChild.ParentNode.Name.ToUpper());
            }
        }

        private XmlDocument GetServiceConfigurationFile()
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
              //Added for RWMS-2015 and RWMS-2007 Start
              //string fPath = System.Configuration.ConfigurationSettings.AppSettings.Get("XmlConfigurationFile");
              string configPath = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PWMSIntegrationServiceSection,
              Made4Net.Shared.ConfigurationSettingsConsts.PWMSIntegrationServiceConfigFilePath);
              string path = Made4Net.DataAccess.Util.BuildAndGetFilePath(configPath);
              bool fileExist = Made4Net.DataAccess.Util.CheckFileExists(path);
              //To do loggin if the file does not exist
              xDoc.Load(path);
              //Added for RWMS-2015 and RWMS-2007 End
              return xDoc;
            }
            catch
            {
                return null;
            }
        }

        #endregion

}
}
using System;
using System.Collections.Generic;
using System.Text;
using Made4Net.DataAccess;
using Made4Net.Shared;
using System.Data;
using System.Xml;
using System.IO;
using System.Timers;
using System.Xml.Xsl;

namespace RWMSIntegrationService.Interfaces
{
    class BaseIntegrationExportPlugin : IBaseIntegrationPluginDataProvider
    {
        #region properties

        private string _pluginID;

        public string PluginID
        {
            get
            {
                return _pluginID;
            }
            set
            {
                _pluginID = value;
            }
        }


        private string enablelogging;

        public string EnableLogging
        {
            get { return enablelogging; }
            set { enablelogging = value; }
        }

        private string logfileprefix;

        public string LogFilePrefix
        {
            get { return logfileprefix; }
            set { logfileprefix = value; }
        }

        private string dataSourceName;

        public string DataSourceName
        {
            get { return dataSourceName; }
            set { dataSourceName = value; }
        }

        private string logfiledir;

        public string LogFileDir
        {
            get { return logfiledir; }
            set { logfiledir = value; }
        }

        private DateTime lastrundate;

        public DateTime LastRunDate
        {
            get { return lastrundate; }
            set { lastrundate = value; }
        }


        private int runinterval;

        public int RunInterval
        {
            get { return runinterval; }
            set { runinterval = value; }
        }

        private string warehouseid;

        public string WarehouseID
        {
            get { return warehouseid; }
            set { warehouseid = value; }
        }

        private string exportcustomtranslationfile;

        public string ExportCustomTranslationFile
        {
            get { return exportcustomtranslationfile; }
            set { exportcustomtranslationfile = value; }
        }
        private int exportfilenamecounter;

        public int ExportFileNameCounter
        {
            get { return exportfilenamecounter; }
            set { exportfilenamecounter = value; }
        }
        private string exportfilenameprimarykeys;

        public string ExportFileNamePrimaryKeys
        {
            get { return exportfilenameprimarykeys; }
            set { exportfilenameprimarykeys = value; }
        }
        private string exportfilenametimestampformat;

        public string ExportFileNameTimeStampFormat
        {
            get { return exportfilenametimestampformat; }
            set { exportfilenametimestampformat = value; }
        }

        //private string exportoutputadjustmentfilenameprefix;

        //public string ExportOutputAdjustmentFileNamePrefix
        //{
        //    get { return exportoutputadjustmentfilenameprefix; }
        //    set { exportoutputadjustmentfilenameprefix = value; }
        //}

        private string exportoutputfilenameprefix;

        public string ExportOutputFileNamePrefix
        {
            get { return exportoutputfilenameprefix; }
            set { exportoutputfilenameprefix = value; }
        }
        private string exportoutputfolder;

        public string ExportOutputFolder
        {
            get { return exportoutputfolder; }
            set { exportoutputfolder = value; }
        }
        #endregion

        #region members
        private string _logType=string.Empty;
        private static readonly object fileLock = new object();
        protected Made4Net.Shared.Logging.LogFile mLogger;
        protected string mLogPath = string.Empty;
        protected double C_DEFAULT_IMPORT_INTERVAL = 30 * 60 * 1000;
        //protected string C_KEYWORD_EXPORT_PRIMARY_KEY = "KEY#";
        private char C_PRIMARY_KEY_DELIMETER = ';';
        protected Timer _exportTimer = null;
        private XmlNode _objectNode;// the xmlConfigfile node of the current object being processed
        private bool _logEnabeld =false;
        protected bool mExportdocLinesInSeparateFile = false;

        private int mDataSourceFilterInterval = 0;


        private DateTime mTempNewLastRunDate;

        #endregion

        #region Ctor
        /// <summary>
        /// initialize object's parameters accroding to a given xml
        /// </summary>
        /// <param name="xn">xml node representing the object </param>
        //public BaseIntegrationExportPlugin(XmlNode xn, bool pExportdocLinesInSeparateFile)
        public BaseIntegrationExportPlugin(XmlNode xn)
        {

            Made4Net.Shared.Warehouse.setCurrentWarehouse(xn.SelectSingleNode("WarehouseID").InnerText);
            DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;

            int intrvl = 0;
            _objectNode = xn.Clone();
            mLogPath = xn.SelectSingleNode("LogFileDir").InnerText;
            try
            {
                this.LastRunDate = DateTime.Parse(xn.SelectSingleNode("LastRunDate").InnerText);
            }
            catch (Exception)
            {
                this.LastRunDate = DateTime.MinValue;

            }
            this.LogFilePrefix = xn.SelectSingleNode("LogFilePrefix").InnerText;
            this.WarehouseID = xn.SelectSingleNode("WarehouseID").InnerText;
            this.EnableLogging = xn.SelectSingleNode("EnableLogging").InnerText;
            Made4Net.Shared.Warehouse.setCurrentWarehouse(this.WarehouseID);
            DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
            this.ExportCustomTranslationFile = xn.SelectSingleNode("ExportCustomTranslationFile").InnerText;
            this.DataSourceName = xn.SelectSingleNode("DataSourceName").InnerText;
            this.ExportFileNamePrimaryKeys = xn.SelectSingleNode("ExportFileNamePrimaryKeys").InnerText;
            this.ExportFileNameTimeStampFormat = xn.SelectSingleNode("ExportFileNameTimeStampFormat").InnerText;
            this.ExportOutputFileNamePrefix = xn.SelectSingleNode("ExportOutputFileNamePrefix").InnerText;
            this.ExportOutputFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("ExportOutputFolder").InnerText);
            int.TryParse(xn.SelectSingleNode("RunInterval").InnerText, out intrvl);
            //mExportdocLinesInSeparateFile = pExportdocLinesInSeparateFile;
            int.TryParse(xn.SelectSingleNode("DataSourceFilterInterval").InnerText, out mDataSourceFilterInterval);


            if (mDataSourceFilterInterval <= 0)
                mDataSourceFilterInterval = 3;

            this.PluginID = xn.SelectSingleNode("PluginID").InnerText;

            try
            {
                mExportdocLinesInSeparateFile = bool.Parse(xn.SelectSingleNode("SeparateLines").InnerText);
            }
            catch
            {
                mExportdocLinesInSeparateFile = false;
            }
            this.RunInterval = intrvl * 60 * 1000;

        }

        public BaseIntegrationExportPlugin()
        {
        }
        #endregion

        #region Methods


        #region Logging

        public void InitLogger(string pLogFileName)
        {
            if (_logEnabeld)
            {

                if (mLogPath != string.Empty)
                {
                    string logName = string.Format("{0}_{1}_{2}", pLogFileName, DateTime.Now.ToString("yyyyMMdd hhmmss"), new Random().Next(100000).ToString());
                    if (!Directory.Exists(mLogPath))
                        Directory.CreateDirectory(mLogPath);

                    mLogger = new Made4Net.Shared.Logging.LogFile(mLogPath, logName, true);
                    mLogger.WriteSeperator();
                }
            }
        }

        protected void writeToLog(string pText)
        {
            if (mLogger == null)
                return;
            mLogger.WriteLine(pText, true);
        }

        #endregion

        public void Start(string logType)
        { _logType =logType;
        _logEnabeld = string.Equals("1", this.EnableLogging);


            initTimer();

            _exportTimer_Elapsed(null, null);
            _exportTimer.Elapsed += new ElapsedEventHandler(_exportTimer_Elapsed);
            _exportTimer.Start();

        }
        private void initTimer()
        {
            if (_exportTimer != null)
                return;
            try
            {
                _exportTimer = new Timer(double.Parse(RunInterval.ToString()));
            }
            catch
            {
                _exportTimer = new Timer(C_DEFAULT_IMPORT_INTERVAL);
            }
        }
        private void _exportTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            Made4Net.Shared.Logging.LogFile tmpLogger = new Made4Net.Shared.Logging.LogFile();

            if (_exportTimer != null)
                _exportTimer.Enabled = false;
            if (_logEnabeld)
            {

                InitLogger("ExportLog_"+ _logType);
            }

            Made4Net.Shared.Warehouse.setCurrentWarehouse(this.WarehouseID);
            DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;

            mTempNewLastRunDate = DateTime.Now;

           // getting lastrundate from xml
            try
            {
                this.LastRunDate = DateTime.Parse(_objectNode.SelectSingleNode("/LastRunDate").InnerText);
            }
            catch (Exception)
            {
                this.LastRunDate = DateTime.MinValue;
                writeToLog(string.Format("LastRunDate node could not be found"));
            }

            //  XmlDocument OutDoc = null;
            try
            {
                DataTable dt = new DataTable();

                dt = GetData();

                int i = 0;
                switch (dt.Rows.Count)
                {
                    case 0:
                        writeToLog(string.Format("No Objects were found. \r\n"));
                        break;
                    case 1:
                        writeToLog(string.Format("One Object was found. \r\n"));
                        break;
                    default:
                        writeToLog(string.Format("{0} Objects were found. \r\n", dt.Rows.Count));
                        break;
                }

                tmpLogger = mLogger;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    //writeToLog(string.Format("proccessing Object No {0}:\r\n", i));
                    XmlDocument xDoc = new XmlDocument();
                    XmlDocument xTranslatedDoc = new XmlDocument();
                    xDoc = GetObjectAsXml(dr);
                    writeToLog(string.Format("Xml to be translated : \r\n {0}", xDoc.InnerXml));

                    try
                    {
                        xTranslatedDoc = ConvertRWMSXmlToRDSXml(xDoc);
                    }
                    catch (Exception ex)
                    {
                          writeToLog(string.Format("Error occured on the translation process : \r\n "));
                        throw new M4NException(ex.ToString());
                    }
                    UpdateObjectLastRunDateNodeInXmlConfigFile(_objectNode);
                    SaveTranslatedXslDoc(xTranslatedDoc, BuildFileName(xDoc));
                }
                mLogger = tmpLogger;

            }
            catch (Exception ex)
            {
                mLogger = tmpLogger;
                writeToLog(ex.ToString());

            }
            _exportTimer.Enabled = true;

        }

        private void UpdateObjectLastRunDateNodeInXmlConfigFile(XmlNode _objectNode)
        {
            //writeToLog(string.Format("updating {0} LastRunDate in config file :", _objectNode.Name));
            writeToLog(string.Format("updating {0} LastRunDate in config file (pluginid = {1}) :", _objectNode.Name, this.PluginID));
            _objectNode.SelectSingleNode("//LastRunDate").InnerText = mTempNewLastRunDate.ToString(); //DateTime.Now.ToString();

            writeToLog(string.Format("LastRunDate = {0}", _objectNode.SelectSingleNode("//LastRunDate").InnerText));

            string fPath = System.Configuration.ConfigurationManager.AppSettings.Get("XmlConfigurationFile");
            writeToLog(string.Format("config file found in {0} :", fPath));

            FileInfo fInfo = new FileInfo(fPath);
            string sConfigurationString = string.Empty;
            //read specific file from specific folder
            try
            {
                lock (fileLock)
                {
                    XmlDocument xDoc = new XmlDocument();
                    using (StreamReader sr = fInfo.OpenText())
                    {

                        sConfigurationString = sr.ReadToEnd();
                        sr.Close();
                        xDoc.LoadXml(sConfigurationString);

                    }
                    XmlNodeList list = xDoc.SelectNodes("/PLUGINSCONFIG/EXPORT/" + _objectNode.Name);
                    foreach (XmlNode node in list)
                    {
                        if (node.SelectSingleNode("PluginID").InnerText == this.PluginID)
                        {
                            node.SelectSingleNode("LastRunDate").InnerText = _objectNode.SelectSingleNode("//LastRunDate").InnerText;
                            xDoc.Save(fPath);
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                writeToLog(ex.ToString());
            }

        }
        private void SaveTranslatedXslDoc(XmlDocument doc, string fileName)
        {
            try
            {
                writeToLog("Exporting Translated XML...");
                if (! mExportdocLinesInSeparateFile)
                {
                    string filePath = Path.Combine(this.ExportOutputFolder, fileName);
                    writeToLog(string.Format("Trying to save file to {0}.", filePath));
                    doc.Save(filePath);
                    writeToLog(string.Format("File saved successfully.", filePath));
                }
                else
                {
                    List<XmlDocument> oFilesList = SplitXmlDocFile(doc);
                    Int32 i = 0;
                    foreach (XmlDocument oTempDoc in oFilesList)
                    {
                        if (i > 0) //this is the detail file, alter the file name
                            fileName = "Lines_" + fileName;
                        string filePath = Path.Combine(this.ExportOutputFolder, fileName);
                        writeToLog(string.Format("Trying to save file to {0}.", filePath));
                        oTempDoc.Save(filePath);
                        writeToLog(string.Format("File saved successfully.", filePath));
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                writeToLog(string.Format("Error occured in export process: \r\n {0}", ex.ToString()));
            }
        }

        /// <summary>
        /// This method will be used by document plugins which should export header and details file separately.
        /// </summary>
        /// <param name="oXmlDoc"></param>
        /// <returns></returns>
        public virtual List<XmlDocument> SplitXmlDocFile(XmlDocument oXmlDoc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private string BuildFileName(XmlDocument pXMLDoc)
        {
            StringBuilder fileName = new StringBuilder();
            fileName.Append(this.ExportOutputFileNamePrefix);
            fileName.Append("_");


            List<string> primaryKeys = null;


            primaryKeys = getPrimaryKeys();
            string key = "";
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                try
                {
                    key = pXMLDoc.SelectSingleNode(primaryKeys[i]).InnerXml;
                }
                catch (Exception ex)
                {
                    key = "";
                    writeToLog(string.Format("Error in {0}:", primaryKeys[i]));
                    writeToLog(ex.ToString());
                }
                fileName.Append(key);
                fileName.Append("_");
            }

            Random random = new Random();
            int randomNumber = random.Next(0, 9999);
            fileName.Append(randomNumber);
            fileName.Append("_");
            fileName.Append(DateTime.Now.ToString(this.ExportFileNameTimeStampFormat));
            fileName.Append(".xml");
            return fileName.ToString();
        }

        private List<string> getPrimaryKeys()
        {
            List<string> primaryKeysList = null;
            try
            {
                string primaryKeys = this.ExportFileNamePrimaryKeys;
                if (!string.IsNullOrEmpty(primaryKeys))
                {
                    if (primaryKeys.Contains(C_PRIMARY_KEY_DELIMETER.ToString()))
                        primaryKeysList = new List<string>(primaryKeys.Split(C_PRIMARY_KEY_DELIMETER));
                    else
                    {
                        primaryKeysList = new List<string>();
                        primaryKeysList.Add(primaryKeys);
                    }
                }
            }
            catch { return null; }
            return primaryKeysList;
        }

        #endregion

        #region IBaseIntegrationPluginDataProvider Members

        public DataTable GetData()
        {
            DataTable dt = null;
            string sql;
            if (!string.IsNullOrEmpty(this.DataSourceName))
            {
                writeToLog(string.Format("DataSourceName being used : {0} .", this.DataSourceName));
                if (DateTime.MinValue == this.LastRunDate)
                {
                    sql = string.Format("select * from {0}", this.DataSourceName);
                    writeToLog(string.Format("LastRunDate is not defiend. \r\n"));
                }
                else
                {
                    DateTime lastRunDateWithThreshold = this.lastrundate.AddMinutes(-this.mDataSourceFilterInterval);
                    //sql = string.Format(" select * from {0} where DATEDIFF(mi, {1},LASTUPDATED )>0 ", this.DataSourceName, Made4Net.Shared.Util.FormatField(this.LastRunDate, this.LastRunDate.ToString(), false));
                   // sql = string.Format(" select * from {0} where DATEDIFF(mi, {1},LASTUPDATED )>0 ",

                        //Added for PWMS-776 and RWMS-812
                    sql = string.Format(" select * from {0} where DATEDIFF(second, {1},LASTUPDATED )>=0 ",
                        this.DataSourceName, Made4Net.Shared.Util.FormatField(lastRunDateWithThreshold, lastRunDateWithThreshold.ToString(), false));
                    //End Added for PWMS-776 and RWMS-812
                }
                writeToLog(string.Format("sql query for getting the last changes  : \r\n {0}", sql));
                dt = new DataTable();
               // Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, true, this.WarehouseID);
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, true, null);

            }
            else
            {
                throw new M4NException(string.Format("DataSourceName was not defiened"));

            }



            return dt;

        }

        public virtual XmlDocument GetObjectAsXml(DataRow dr)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public XmlDocument ConvertRWMSXmlToRDSXml(XmlDocument doc)
        {
            writeToLog(string.Format("xml translation started."));

            string strXml = string.Empty;

            string strXstFile = this.ExportCustomTranslationFile;
            if (string.IsNullOrEmpty(strXstFile))
            {
                string errStr = string.Format("xml translation file location is not defined.");
                writeToLog(errStr);
                return doc;
            }
            writeToLog(string.Format("xml translation file location :\r\n {0}", strXstFile));

            Made4Net.Shared.XSLTransformer xslt = new XSLTransformer();

            return xslt.Transform(doc, strXstFile);
        }


        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Made4Net.DataAccess;
using Made4Net.Shared;
using System.Data;
using System.IO;
using System.Timers;
using System.Xml.Xsl;

namespace RWMSIntegrationService.Interfaces
{
    class BaseIntegrationImportPlugin
    {

        #region properties
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

        private String consignee ;

        public String CONSIGNEE
        {
            get { return consignee ; }
            set { consignee  = value; }
        }

        private Made4Net.Shared.XSLTransformer xslttransformer;

        public Made4Net.Shared.XSLTransformer XsltTransformer
        {
            get { return xslttransformer; }
            set { xslttransformer = value; }
        }

        private string moveonsuccessfolder;

        public string MoveOnSuccessFolder
        {
            get { return moveonsuccessfolder; }
            set { moveonsuccessfolder = value; }
        }

        private string moveonfailurefolder;

        public string MoveOnFailureFolder
        {
            get { return moveonfailurefolder; }
            set { moveonfailurefolder = value; }
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

        //private string importcustomtranslationfile;

        //public string ImportCustomTranslationFile
        //{
        //    get { return importcustomtranslationfile; }
        //    set { importcustomtranslationfile = value; }
        //}

        private string importdirectory;

        public string ImportDirectory
        {
            get { return importdirectory; }
            set { importdirectory = value; }
        }
        private string importfilenamefilter;

        public string ImportFileNameFilter
        {
            get { return importfilenamefilter; }
            set { importfilenamefilter = value; }
        }

        public string ImportCustomTranslationFile
        {
            get { return mImportCustomTranslationFile; }
            set { mImportCustomTranslationFile = value; }
        }

        #endregion

        #region members
        string _logType;

        bool _logEnabeld;
        protected Made4Net.Shared.Logging.LogFile mLogger;
        protected string mLogPath = string.Empty;
        protected string mImportCustomTranslationFile = string.Empty;
        protected double C_DEFAULT_IMPORT_INTERVAL = 1 * 60 * 1000;
        //protected string C_KEYWORD_import_PRIMARY_KEY = "KEY#";
        protected Timer _importTimer = null;
        private XmlNode _objectNode;// the xmlConfigfile node of the current object being processed

        #endregion

        #region Ctor
        /// <summary>
        /// initialize object's parameters accroding to a given xml
        /// </summary>
        /// <param name="xn">xml node representing the object </param>
        public BaseIntegrationImportPlugin(XmlNode xn, string sConsignee)
        {
            this.CONSIGNEE = sConsignee;

            Made4Net.Shared.Warehouse.setCurrentWarehouse(xn.SelectSingleNode("WarehouseID").InnerText);
            DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;

            int intrvl = 0;
            _objectNode = xn.Clone();
            mLogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("LogFileDir").InnerText);
            this.EnableLogging = xn.SelectSingleNode("EnableLogging").InnerText;
            this.LogFilePrefix = xn.SelectSingleNode("LogFilePrefix").InnerText;
            this.WarehouseID = xn.SelectSingleNode("WarehouseID").InnerText;
            Made4Net.Shared.Warehouse.setCurrentWarehouse(this.WarehouseID);
            DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
            //this.DataSourceName = xn.SelectSingleNode("//" + xn.Name + "/DataSourceName").InnerText;
            //this.ImportCustomTranslationFile = xn.SelectSingleNode("//" + xn.Name + "/ImportCustomTranslationFile").InnerText;
            this.ImportDirectory = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("ImportDirectory").InnerText);
            this.ImportFileNameFilter = xn.SelectSingleNode("ImportFileNameFilter").InnerText;
            this.LogFileDir = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("LogFileDir").InnerText);
            this.MoveOnFailureFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("MoveOnFailureFolder").InnerText);
            this.MoveOnSuccessFolder = Made4Net.DataAccess.Util.BuildAndGetFilePath(xn.SelectSingleNode("MoveOnSuccessFolder").InnerText);
            this.ImportCustomTranslationFile = xn.SelectSingleNode("ImportCustomTranslationFile").InnerText;
            int.TryParse(xn.SelectSingleNode("RunInterval").InnerText, out intrvl);
            this.RunInterval = intrvl * 60 * 1000;

        }

        public BaseIntegrationImportPlugin()
        {

        }
        #endregion

        #region Methods


        #region Logging

        public void InitLogger(string pLogFileName)
        {
            if (string.Equals(this.EnableLogging,"1"))
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
        {
            _logType = logType;
            _logEnabeld = string.Equals("1", this.EnableLogging);

            initTimer();

            _importTimer_Elapsed(null, null);
            _importTimer.Elapsed += new ElapsedEventHandler(_importTimer_Elapsed);
            _importTimer.Start();

        }

        private void initTimer()
        {
            if (_importTimer != null)
                return;
            try
            {
                _importTimer = new Timer(double.Parse(RunInterval.ToString()));
            }
            catch
            {
                _importTimer = new Timer(C_DEFAULT_IMPORT_INTERVAL);
            }
        }

        private void _importTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                if (_importTimer != null)
                    _importTimer.Enabled = false;

                Made4Net.Shared.Warehouse.setCurrentWarehouse(this.WarehouseID);
                DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;

                if (_logEnabeld)
                {

                    InitLogger("ImportLog_" + _logType);
                }
                DirectoryInfo di = new DirectoryInfo(this.ImportDirectory);
                bool bFilesFound = false;

                foreach (FileInfo oInputFile in di.GetFiles(this.ImportFileNameFilter))
                {
                    try
                    {
                        bFilesFound = true;
                        if (mLogger != null)
                        {
                            mLogger.WriteSeperator();
                            mLogger.WriteLine(string.Format("Importing file {0}..",oInputFile.Name),true);
                        }
                        // Translate document if needed
                        XmlDocument xdoc = TransformXml(GetXmlFromFile(oInputFile));
                        if (UpdateDataFromTranslatedXml(xdoc))
                        {
                            MoveFileToSuccessedFolder(oInputFile);
                        }
                        else
                        {
                            MoveFileToFailedFolder(oInputFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        mLogger.WriteError(ex.ToString());
                        MoveFileToFailedFolder(oInputFile);
                    }
                }
                if (!bFilesFound && mLogger != null)
                {
                    mLogger.WriteLine("No files found to process.",true);
                }
            }
            catch (Exception ex)
            {
                if (mLogger != null)
                    mLogger.WriteError(ex.ToString());
            }
            finally
            {
                if (_importTimer != null)
                    _importTimer.Enabled = true;
            }
        }

        protected XmlDocument TransformXml(XmlDocument doc)
        {
            writeToLog(string.Format("File translation started."));

            string strXml = string.Empty;
            string strXstFile = this.ImportCustomTranslationFile;
            if (string.IsNullOrEmpty(strXstFile))
            {
                string errStr = string.Format("Translation file not defined.");
                writeToLog(errStr);
                return doc;
            }
            Made4Net.Shared.XSLTransformer xslt = new XSLTransformer();
            return xslt.Transform(doc, strXstFile);
        }

        private void MoveFileToSuccessedFolder(FileInfo oInputFile)
        {
            string sflienewName = string.Concat(oInputFile.Name.TrimEnd(".xml".ToCharArray()), "_", DateTime.Now.ToString("yyyyMMdd_hhmmss"),".xml");
            writeToLog(string.Format("File process finished, file {0} is moved to {1}/{2} ", oInputFile.Name, this.MoveOnSuccessFolder, sflienewName));
            System.IO.File.Move(oInputFile.FullName, System.IO.Path.Combine(this.MoveOnSuccessFolder, sflienewName));
        }

        private void MoveFileToFailedFolder(FileInfo oInputFile)
        {
            string sflienewName = string.Concat(oInputFile.Name.TrimEnd(".xml".ToCharArray()), "_", DateTime.Now.ToString("yyyyMMdd_hhmmss"), ".xml");
            writeToLog(string.Format("File process failed, file {0} is moved to {1}/{2} ", oInputFile.Name, this.MoveOnFailureFolder, sflienewName));
            System.IO.File.Move(oInputFile.FullName, System.IO.Path.Combine(this.MoveOnFailureFolder, sflienewName));
        }

        private XmlDocument GetXmlFromFile(FileInfo oInputFile)
        {
            try
            {
               StreamReader sr = new StreamReader(oInputFile.FullName);
                {
                    String sXmlText = sr.ReadToEnd();
                    sr.Close();
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(sXmlText);
                    return xdoc;
                }
            }
            catch (Exception e)
            {
                writeToLog(string.Format("Error occurred while reading input file... {0}", e.ToString()));
                return null ;
            }
        }

        protected virtual bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    #endregion
}
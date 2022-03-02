using ExpertObjectMapper;
using Made4Net.DataAccess;
using Made4Net.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;


namespace SCExpertConnectPlugins.BO
{
    public class BasePlugin : ISCExpertExporter, ISCExpertImporter, IDisposable
    {
        #region Memebers

        protected int mPluginId;
        protected string mPluginDescription = string.Empty;
        protected int mPluginTypeId;
        protected string mPluginTransactionType = string.Empty;
        protected string mLogPath = string.Empty;
        protected int mLogLevel;
        protected string mImportTranslationFile = string.Empty;
        protected string mExportTranslationFile = string.Empty;
        protected string mWarehouseId = string.Empty;
        protected string mBOType = string.Empty;
        protected bool mCommitBOOnBoElementFailure;
        protected string mAlertProcessResult = string.Empty;
        protected string mAlertRecipient = string.Empty;
        protected string mImportTransactionContentFilter;
        protected string mExportTransactionContentFilter = string.Empty;
        protected string mTransactionSet;

        private Hashtable mParamsCollection;
        private List<string> mKeysCollection;
        private Made4Net.Shared.Logging.LogFile mLogger;
        private bool allowAckFuture = false;
        private string strInstancePath = string.Empty;//PWMS-817
        #endregion

        #region Properties

        private string WhereClause
        {
            get { return string.Format("PluginId = {0}", mPluginId); }
        }

        public int PluginId
        {
            get { return mPluginId; }
            set { mPluginId = value; }
        }

        public string PluginDescription
        {
            get { return mPluginDescription; }
            set { mPluginDescription = value; }
        }

        public int PluginTypeId
        {
            get { return mPluginTypeId; }
            set { mPluginTypeId = value; }
        }

        public string PluginTransactionType
        {
            get { return mPluginTransactionType; }
            set { mPluginTransactionType = value; }
        }

        public string LogPath
        {
            get { return mLogPath; }
            set { mLogPath = value; }
        }

        public int LogLevel
        {
            get { return mLogLevel; }
            set { mLogLevel = value; }
        }

        public string ImportTranslationFile
        {
            get { return mImportTranslationFile; }
            set { mImportTranslationFile = value; }
        }

        public string ExportTranslationFile
        {
            get { return mExportTranslationFile; }
            set { mExportTranslationFile = value; }
        }

        public string WarehouseId
        {
            get { return mWarehouseId; }
            set { mWarehouseId = value; }
        }

        public string BOType
        {
            get { return mBOType; }
            set { mBOType = value; }
        }

        public bool CommitBOOnBoElementFailure
        {
            get { return mCommitBOOnBoElementFailure; }
            set { mCommitBOOnBoElementFailure = value; }
        }

        public string AlertProcessResult
        {
            get { return mAlertProcessResult; }
            set { mAlertProcessResult = value; }
        }

        public string AlertRecipient
        {
            get { return mAlertRecipient; }
            set { mAlertRecipient = value; }
        }

        public Hashtable PluginParametrs
        {
            get { return mParamsCollection; }
        }

        public List<string> KeysCollection
        {
            get { return mKeysCollection; }
        }

        public Made4Net.Shared.Logging.LogFile Logger
        {
            get { return mLogger; }
        }

        public string TransactionSet
        {
            get { return mTransactionSet; }
            set { mTransactionSet = value; }
        }

        public string ImportTransactionContentFilter
        {
            get { return mImportTransactionContentFilter; }
            set { mImportTransactionContentFilter = value; }
        }

        public string ExportTransactionContentFilter
        {
            get { return mExportTransactionContentFilter; }
            set { mExportTransactionContentFilter = value; }
        }

        #endregion

        #region Constructors

        public BasePlugin(int pPluginId)
        {
            mPluginId = pPluginId;
            Load();
        }

        #endregion

        #region Methods

        #region Accessories

        public string GetParameterValue(string pParamName)
        {
            if (PluginParametrs.ContainsKey(pParamName.ToUpper()))
                return ((PluginParams)PluginParametrs[pParamName.ToUpper()]).ParamValue;
            string errMsg = string.Format("Parameter {0} not found in the parameters collection for current plugin", pParamName);
            throw new M4NException(new Exception(), errMsg, errMsg);
        }
        public bool HasParameterValue(string pParamName)
        {
            try
            {
                string _tempValue = GetParameterValue(pParamName);
                if (_tempValue.Equals(""))
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void Load()
        {
            string sSql = string.Format("select * from SCEXPERTCONNECTPLUGINS where {0}", WhereClause);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sSql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                if (!(dr["PLUGINDESCRIPTION"] is DBNull)) mPluginDescription = dr["PLUGINDESCRIPTION"].ToString();
                if (!(dr["PLUGINTYPEID"] is DBNull)) mPluginTypeId = Convert.ToInt32(dr["PLUGINTYPEID"]);
                if (!(dr["PLUGINTRANSACTIONTYPE"] is DBNull)) mPluginTransactionType = dr["PLUGINTRANSACTIONTYPE"].ToString();

                if (!(dr["LOGPATH"] is DBNull))
                {
                    //mLogPath = dr["LOGPATH"].ToString();
                    mLogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dr["LOGPATH"].ToString());
                }

                if (!(dr["LOGLEVEL"] is DBNull)) mLogLevel = Convert.ToInt32(dr["LOGLEVEL"]);
                if (!(dr["IMPORTTRANSLATIONFILE"] is DBNull)) mImportTranslationFile = dr["IMPORTTRANSLATIONFILE"].ToString();
                if (!(dr["EXPORTTRANSLATIONFILE"] is DBNull)) mExportTranslationFile = dr["EXPORTTRANSLATIONFILE"].ToString();
                if (!(dr["WAREHOUSEID"] is DBNull)) mWarehouseId = dr["WAREHOUSEID"].ToString();
                if (!(dr["BOTYPE"] is DBNull)) mBOType = dr["BOTYPE"].ToString();
                if (!(dr["COMMITBOONBOELEMENTFAILURE"] is DBNull)) mCommitBOOnBoElementFailure = Convert.ToBoolean(dr["COMMITBOONBOELEMENTFAILURE"]);
                if (!(dr["ALERTPROCESSRESULT"] is DBNull)) mAlertProcessResult = dr["ALERTPROCESSRESULT"].ToString();
                if (!(dr["ALERTRECIPIENT"] is DBNull)) mAlertRecipient = dr["ALERTRECIPIENT"].ToString();
                if (!(dr["IMPORTTRANSACTIONCONTENTFILTER"] is DBNull)) mImportTransactionContentFilter = dr["IMPORTTRANSACTIONCONTENTFILTER"].ToString();
                if (!(dr["EXPORTTRANSACTIONCONTENTFILTER"] is DBNull)) mExportTransactionContentFilter = dr["EXPORTTRANSACTIONCONTENTFILTER"].ToString();

                LoadParams();
                LoadKeys();
                if (HasParameterValue("ResponseAckFolder"))
                    if (HasParameterValue("ResponseAckFilter"))
                        if (HasParameterValue("ResponseAckInMap"))
                            if (HasParameterValue("ResponseAckOutMap"))
                                allowAckFuture = true;
            }
            else
            {
                string errMsg = string.Format("Plugin {0} not found in plugins table", mPluginId);
                throw new M4NException(new Exception(), errMsg, errMsg);
            }
        }

        private void LoadParams()
        {
            string TempParamValue = string.Empty;
            mParamsCollection = new Hashtable();
            string sSql = string.Format("select isnull(PARAMNAME,'') as PARAMNAME,isnull(PARAMVALUE,'') as PARAMVALUE from SCEXPERTCONNECTPLUGINPARAMS where {0}", WhereClause);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sSql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
            foreach (DataRow dr in dt.Rows)
            {
                mParamsCollection.Add(dr["PARAMNAME"].ToString().ToUpper(), new PluginParams(mPluginId, dr));
            }
        }

        private void LoadKeys()
        {
            mKeysCollection = new List<string>();
            string sSql = string.Format("select TRANSACTIONKEY from SCEXPERTCONNECTPLUGINTRANSACTIONKEYS where {0}", WhereClause);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sSql, dt, false, Made4Net.Schema.Constants.CONNECTION_NAME);
            foreach (DataRow dr in dt.Rows)
            {
                mKeysCollection.Add(dr["TRANSACTIONKEY"].ToString());
            }
        }

        #endregion

        #region Logging

        public void InitLogger(string pLogFileName)
        {
            if (mLogger != null)
            {
                mLogger.Dispose();
                mLogger = null;
            }
            if (mLogPath != string.Empty)
            {
                string logName = string.Format("{0}_{1}_{2}", pLogFileName, DateTime.Now.ToString("yyyyMMdd hhmmss"), new Random().Next(100000).ToString());
                mLogger = new Made4Net.Shared.Logging.LogFile(mLogPath, logName, true);
                mLogger.WriteSeperator();
            }
        }

        #endregion

        #region Alerts

        public void SendAlertsOnProcessComplete(string pSubject, string pBody, List<string> pAttachments)
        {
            try
            {
                if (mAlertRecipient == string.Empty)
                {
                    if (Logger != null) { Logger.WriteLine(string.Format("Mail recipient is blank, will not send alerts!"), true); }
                }
                if (Logger != null) { Logger.WriteLine(string.Format("Trying to send alerts according to plugin definition..."), true); }
                Made4Net.Shared.Mail.MailSender oMailSender = new Made4Net.Shared.Mail.MailSender();
                oMailSender.Body = pBody;
                oMailSender.From = Made4Net.Shared.Util.GetSystemParameterValue("SCExpertConnectMailSenderFrom");
                if (Logger != null) { Logger.WriteLine(string.Format("Mail recipients: {0}", mAlertRecipient), true); }
                oMailSender.To = mAlertRecipient;
                oMailSender.Subject = pSubject;
                oMailSender.Attachments = pAttachments;
                oMailSender.SMTPServer = Made4Net.Shared.Util.GetSystemParameterValue("SystemMailSenderSMTPServer");
                oMailSender.Send();
                if (Logger != null) { Logger.WriteLine(string.Format("Mail sent."), true); }
            }
            catch (Exception ex)
            {
                if (Logger != null) { Logger.WriteLine(string.Format("Error occured while trying to send alert. Error details:"), true); }
                if (Logger != null) { Logger.WriteLine(string.Format("{0}", ex.ToString()), true); }
            }
        }

        #endregion

        #endregion

        #region ISCExpertExporter Members

        public virtual int Export(System.Xml.XmlDocument oXMLDoc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Added for RWMS-503
        public virtual void SaveRawXML(XmlDocument oXMLDoc, string Refname)
        {
            throw new Exception("The method SaveRawXML is not implemented.");
        }
        //End Added for RWMS-503
        #endregion

        #region ISCExpertImporter Members

        public virtual void Import()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void ProcessResultFromSCExpertConect(string pTransactionSet, Dictionary<string, DataMapperProcessResult> pObjectDataMapperResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void ProcessResponseFromSCExpertConect(string pTransactionSet, Dictionary<string, DataMapperProcessResult> pObjectDataMapperResult)
        {
            if (allowAckFuture)
            {
                XmlDocument _resxml = new XmlDocument();
                _resxml.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><SCEXPERTCONNECTRESPONSE><TRANSACTIONSET>{0}</TRANSACTIONSET><RESPONSE></RESPONSE></SCEXPERTCONNECTRESPONSE>", pTransactionSet));
                foreach (KeyValuePair<string, ExpertObjectMapper.DataMapperProcessResult> kvp in pObjectDataMapperResult)
                {
                    XmlNode _xmlnode = _resxml.CreateElement("TRANSACTION");
                    XmlNode _key = _resxml.CreateElement("KEY");
                    XmlNode _val = _resxml.CreateElement("RESULT");
                    _resxml.SelectSingleNode(".//RESPONSE").AppendChild((XmlNode)_xmlnode);
                    _key.InnerText = kvp.Key;
                    if (kvp.Value.TransactionResult == ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success)
                    {
                        _val.InnerText = "SUCCESS";
                    }
                    else
                    {
                        _val.InnerText = "FAILED";
                    }
                    _xmlnode.AppendChild(_key);
                    _xmlnode.AppendChild(_val);
                }
                ProcessResponseFromSCExpertConect(_resxml);
            }
        }
        public virtual void ProcessResponseFromSCExpertConect(XmlDocument rXml)
        {

        }
        #endregion

        #region Delegates

        public delegate void ImportProcessCompleteHandler(object oPlugin, ImportProcessResultsEventArgs ImportResults, bool pShouldSendBackNotification, bool pRetransmitTransaction);

        // The event to publish
        public event ImportProcessCompleteHandler ImportCompleted;

        // The method which fires the Event
        private void OnImportCompleted(object oPlugin, ImportProcessResultsEventArgs ImportResults)
        {
            if (ImportCompleted != null)
                ImportCompleted(oPlugin, ImportResults, true, false);
        }

        public void NotifyImportProcessComplete(List<XmlDocument> ResultFiles)
        {
            // And notify the connect when the import is finished by raising the propriate event.
            ImportProcessResultsEventArgs ImportResults = new ImportProcessResultsEventArgs(ResultFiles);
            this.OnImportCompleted(this, ImportResults);
        }

        #endregion

        #region "IDisposable"
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Logger != null)
                    {
                        Logger.Dispose();
                    }
                }
                disposedValue = true;
            }
        }
        ~BasePlugin()
        {
            Dispose(false);
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
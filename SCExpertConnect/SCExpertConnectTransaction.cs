using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using Made4Net.Shared;
using Made4Net.DataAccess;

namespace SCExpertConnect
{
    #region SCExpertConnectTransaction

    public class SCExpertConnectTransaction
    {
        #region Memebers

        //private int mTransactionId;
        private DateTime mTransactionDate;
        private SCExpertConnectTransactionTypes.TransactionType mTransactionType;
        private int mPluginId;
        private SCExpertConnectTransactionStatus.TransactionStatus mTransactionStatus;
        private string mTransactionSet;
        private string mTransactionBOType;
        private string mTransactionBOKey;
        private string mTransactionData;
        private string mTransactionError;
        //Added for RWMS-1509 and RWMS-1502
        private string mTransactionDataMailAttachment;   
        private string mTransactionErrorMailAttachment;
        //Ended for RWMS-1509 and RWMS-1502

        #endregion

        // RWMS-1434 start
        #region Properties

        public string TransactionError
        {
            get { return mTransactionError; }
        }
        //Added for RWMS-1509 and RWMS-1502
       public string TransactionDataMailAttachment   
      {   
      get { return mTransactionDataMailAttachment; }   
      }   
         
      public string TransactionErrorMailAttachment   
      {   
      get { return mTransactionErrorMailAttachment; }   
      }

      //Ended for RWMS-1509 and RWMS-1502
        #endregion
        // RWMS-1434 end


        #region Constructors

        public SCExpertConnectTransaction() 
        {
        }
        // RWMS-1434 start
        public SCExpertConnectTransaction(string sTransactionKey, int stransactionID)
        {
            //Fetch from table scexpertconnecttransaction. 
            string sSql = string.Format("SELECT TRANSACTIONDATA,TRANSACTIONBOKEY, TRANSACTIONERROR FROM SCEXPERTCONNECTTRANSACTION WHERE TRANSACTIONBOKEY='" + sTransactionKey + "' AND TRANSACTIONID=" + stransactionID);  
            DataTable dtScTransaction = new DataTable();
            DataInterface.FillDataset(sSql,dtScTransaction,false,Made4Net.Schema.Constants.CONNECTION_NAME);
            mTransactionData = dtScTransaction.Rows[0]["TRANSACTIONDATA"].ToString();
            mTransactionBOKey = dtScTransaction.Rows[0]["TRANSACTIONBOKEY"].ToString();   
              if (!dtScTransaction.Rows[0].IsNull("TRANSACTIONERROR"))   
              mTransactionError = dtScTransaction.Rows[0]["TRANSACTIONERROR"].ToString();   
              else   
              mTransactionError = "";   
    
        }
        // RWMS-1434 end
        #endregion

        #region Methods

        public void PostTransaction(SCExpertConnectTransactionTypes.TransactionType pTransactionType, int pPluginId, SCExpertConnectTransactionStatus.TransactionStatus pTransactionStatus,
                string pTransactionSet, string pTransactionBOType, string pTransactionBOKey, string pTransactionData, string pTransactionError, out int stransactionID)  
        {
            mTransactionDate = DateTime.Now;
            mTransactionType = pTransactionType;
            mPluginId = pPluginId;
            mTransactionStatus = pTransactionStatus;
            mTransactionSet = pTransactionSet;
            mTransactionBOType = pTransactionBOType;
            mTransactionBOKey = pTransactionBOKey;
            mTransactionData = pTransactionData;
            mTransactionError = pTransactionError;

           string sSql = string.Format("INSERT INTO SCEXPERTCONNECTTRANSACTION (TRANSACTIONDATE, TRANSACTIONTYPE, PLUGINID, TRANSACTIONSTATUS, TRANSACTIONSET," +   
                         "TRANSACTIONBOTYPE, TRANSACTIONBOKEY, TRANSACTIONDATA, TRANSACTIONERROR) OUTPUT INSERTED.TRANSACTIONID VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                Made4Net.Shared.Util.FormatField(mTransactionDate, "", false), Made4Net.Shared.Util.FormatField(SCExpertConnectTransactionTypes.TransactionTypeToDBString(mTransactionType), "", false),
                Made4Net.Shared.Util.FormatField(mPluginId, "", false), Made4Net.Shared.Util.FormatField(SCExpertConnectTransactionStatus.TransactionStatusToDBString(mTransactionStatus), "", false),
                Made4Net.Shared.Util.FormatField(mTransactionSet, "", false), Made4Net.Shared.Util.FormatField(mTransactionBOType, "", false),
                Made4Net.Shared.Util.FormatField(mTransactionBOKey, "", false), Made4Net.Shared.Util.FormatField(mTransactionData, "", false),
                Made4Net.Shared.Util.FormatField(mTransactionError, "", false));

           // Made4Net.DataAccess.DataInterface.RunSQL(sSql,Made4Net.Schema.Constants.CONNECTION_NAME);
           stransactionID = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql, Made4Net.Schema.Constants.CONNECTION_NAME));  
        }
        // RWMS-1434 start
        public void CreateMailAttachments()
        {
            //Create an xml file which will contain the data in mTransactionData. 
            //The XML file will be saved in location C:\RWMS\Logs\Connect\MailAttachment
            //Each attachment will have the file name sTransactionKey_Mailer.xml
            //The file path of the mail attachment will be saved in the property mTransactionMailAttachment
            //XmlDocument oDoc = new XmlDocument();
            string attachmentPath = System.Configuration.ConfigurationManager.AppSettings.Get("AttachmentPath");
            string fileName = mTransactionBOKey + "_TransactionData.txt";   
              mTransactionDataMailAttachment = attachmentPath + fileName;   
              System.IO.File.WriteAllBytes(mTransactionDataMailAttachment, GetTruncatedAttachmentContent(mTransactionData));   
              mTransactionErrorMailAttachment = "";   
             //If transaction error info exists then create attachment of the same   
             if (mTransactionError.Length > 0)   
              {   
              fileName = mTransactionBOKey + "_TransactionError.txt";   
              mTransactionErrorMailAttachment = attachmentPath + fileName;   
              System.IO.File.WriteAllBytes(mTransactionErrorMailAttachment, GetTruncatedAttachmentContent(mTransactionError));   
              }  

         }
        // RWMS-1434 start
        private byte[] GetTruncatedAttachmentContent(String strcontents)   
              {   
              const int sizeLimitInBytes = 1 * 1024 * 1024;   
              System.IO.MemoryStream stream = new System.IO.MemoryStream();   
              using (var writer = new System.IO.StreamWriter(stream))   
              {   
              foreach (var currentData in strcontents)   
              {   
              if (stream.Position > sizeLimitInBytes)   
              {   
              break;   
              }   
              writer.Write(currentData);   
              }   
              }   
              byte[] conts = stream.ToArray();   
              stream.Dispose();   
              return conts;   
            }   

        #endregion
    }

    #endregion

    #region SCExpertConnectTransactionStatus

    public class SCExpertConnectTransactionStatus
    {
        public enum TransactionStatus { Fail = -1, Success = 0 };

        public static string TransactionStatusToDBString(TransactionStatus pTransactionStatus)
        {
            string res;
            switch (pTransactionStatus)
            {
                case TransactionStatus.Success:
                    res = "SUCCESS";
                    break;
                case TransactionStatus.Fail:
                    res = "FAIL";
                    break;
                default:
                    res = string.Empty;
                    break;
            }
            return res;
        }
    }

    #endregion

    #region SCExpertConnectTransactionTypes

    public class SCExpertConnectTransactionTypes
    {
        public enum TransactionType { Import, Export };

        public static string TransactionTypeToDBString(TransactionType pTransactionType)
        {
            string res;
            switch (pTransactionType)
            {
                case TransactionType.Import:
                    res = "IMPORT";
                    break;
                case TransactionType.Export:
                    res = "EXPORT";
                    break;
                default:
                    res = string.Empty;
                    break;
            }
            return res;
        }
    }

    #endregion
}

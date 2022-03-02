using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCExpertConnectPlugins.BO;
using ExpertObjectMapper;

namespace SCExpertConnect
{
    class SCExpertConnectUtils
    {

        public static List<string> CreateMailAttachments(string sTransactionKey, int stransactionID, Made4Net.Shared.Logging.LogFile oLogger)
        {
            SCExpertConnectTransaction oSCExpertConnectTransaction = null;
            List<string> pAttachments = null;
            try
            {
                oSCExpertConnectTransaction = new SCExpertConnectTransaction(sTransactionKey, stransactionID);
                oSCExpertConnectTransaction.CreateMailAttachments();
                pAttachments = new List<string>();
                pAttachments.Add(oSCExpertConnectTransaction.TransactionDataMailAttachment);
                if (oSCExpertConnectTransaction.TransactionErrorMailAttachment.Length > 0)
                {
                    pAttachments.Add(oSCExpertConnectTransaction.TransactionErrorMailAttachment);
                }
            }
            catch (Exception ex)
            {
                if (oLogger != null) { oLogger.WriteLine(string.Format("Error occured while trying to access attachment. Error details:"), true); }
                if (oLogger != null) { oLogger.WriteLine(string.Format("{0}", ex.ToString()), true); }
            }
            return pAttachments;
        }

    }
}

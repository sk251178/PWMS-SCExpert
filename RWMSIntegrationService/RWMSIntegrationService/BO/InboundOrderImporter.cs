using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;
using WMS.Logic;

namespace RWMSIntegrationService.BO
{
    class InboundOrderImporter : BaseIntegrationImportPlugin
    {
        public InboundOrderImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            try
            {
                bool bProcSucceeded = true;
                foreach (XmlNode oNode in xdoc.SelectNodes("DATACOLLECTION/DATA"))
                {
                    string sConsignee, sOrderId;
                    sConsignee = oNode.SelectSingleNode("CONSIGNEE").InnerText;
                    sOrderId = oNode.SelectSingleNode("ORDERID").InnerText;
                    if (!WMS.Logic.InboundOrderHeader.Exists(sConsignee, sOrderId))
                    {
                        writeToLog(string.Format("Ivalid order id. Order id {0} does not exist in the warehouse.",sOrderId));
                        bProcSucceeded = false;
                        continue;
                    }
                    else
                    {
                        // Validate that the order status is NEW
                        WMS.Logic.InboundOrderHeader oOrder = new WMS.Logic.InboundOrderHeader(sConsignee, sOrderId, true);
                        if (!oOrder.STATUS.Equals(WMS.Lib.Statuses.InboundOrderHeader.STATUSNEW, StringComparison.OrdinalIgnoreCase))
                        {
                            writeToLog(string.Format("Ivalid order status (order id {0}).", sOrderId));
                            bProcSucceeded = false;
                        }
                        else
                        {
                            XmlDocument oTransactionXML = new XmlDocument();
                            oTransactionXML.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><BUSINESSOBJECT><BOTYPE>INBOUND</BOTYPE></BUSINESSOBJECT>"));
                            oTransactionXML.SelectSingleNode("BUSINESSOBJECT").AppendChild(oTransactionXML.ImportNode(oNode, true));

                            string err = "";
                            ExpertObjectMapper.ObjectProcessor op = new ExpertObjectMapper.ObjectProcessor();
                            op.ProcessInbound(oTransactionXML, mLogger, ref err);
                            if (!string.IsNullOrEmpty(err))
                            {
                                bProcSucceeded = false;
                                if (mLogger != null)
                                    mLogger.WriteError(err);
                            }
                            else
                            {
                                UpdateOrderTimeStamp(sConsignee, sOrderId);
                            }
                        }
                    }
                }
                return bProcSucceeded;               
            }
            catch (Exception ex)
            {
                if (mLogger != null)
                    mLogger.WriteError(ex.ToString());
                
                return false;
            }
        }

        private void UpdateOrderTimeStamp(string pConsignee, string pOrderId)
        {
            WMS.Logic.InboundOrderHeader oOrder = new WMS.Logic.InboundOrderHeader(pConsignee, pOrderId,true);
            oOrder.EDITDATE = DateTime.Now;
            oOrder.EDITUSER = "SYSTEM";
            oOrder.Save();
        }
    }
}

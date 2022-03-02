using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;

namespace RWMSIntegrationService.BO
{
    class OutboundOrderImporter : BaseIntegrationImportPlugin
    {
        public OutboundOrderImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            try
            {
                bool bProcSucceeded = true;
                foreach (XmlNode oNode in xdoc.SelectNodes("DATACOLLECTION/DATA"))
                {
                    XmlDocument oTransactionXML = new XmlDocument();
                    oTransactionXML.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><BUSINESSOBJECT><BOTYPE>OUTBOUND</BOTYPE></BUSINESSOBJECT>"));
                    oTransactionXML.SelectSingleNode("BUSINESSOBJECT").AppendChild(oTransactionXML.ImportNode(oNode, true));

                    string err = "";
                    ExpertObjectMapper.ObjectProcessor op = new ExpertObjectMapper.ObjectProcessor();
                    op.ProcessOutbound(oTransactionXML, mLogger, ref err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        bProcSucceeded = false;
                        if (mLogger != null)
                            mLogger.WriteError(err);
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
    }
}

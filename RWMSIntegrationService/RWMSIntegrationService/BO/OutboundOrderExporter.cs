using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;
using System.Data;
using ExpertObjectMapper;
using Made4Net.Shared;

namespace RWMSIntegrationService.BO
{
    class OutboundOrderExporter : BaseIntegrationExportPlugin
    {

        public OutboundOrderExporter(XmlNode xn)
            : base(xn)
        {}


        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            qSender.Add("DOCUMENT", dr["ORDERID"].ToString());
            xmlDocument = op.ExportOutBound("", qSender, null);
            writeToLog(string.Format("original Xml from table : \r\n {0}", xmlDocument.InnerXml));

            return xmlDocument;

        }
    }
}

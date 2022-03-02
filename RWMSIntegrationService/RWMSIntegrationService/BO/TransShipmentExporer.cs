using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Data;
using System.Xml;
using ExpertObjectMapper;
using Made4Net.Shared;

namespace RWMSIntegrationService.BO
{
    class TransShipmentExporter : BaseIntegrationExportPlugin
    {
        public TransShipmentExporter()
        {

        }
        public TransShipmentExporter(XmlNode xn): base(xn)
        {}
        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            qSender.Add("DOCUMENT", dr["TRANSSHIPMENT"].ToString());
            xmlDocument = op.ExportTransShipment("", qSender, null);
            writeToLog(string.Format("original Xml from table : \r\n {0}", xmlDocument.InnerXml));
           
            return xmlDocument;

        }
       
    }
}

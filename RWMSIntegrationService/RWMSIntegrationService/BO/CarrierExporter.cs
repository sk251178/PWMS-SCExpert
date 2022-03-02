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
    class CarrierExporter : BaseIntegrationExportPlugin
    {
        public CarrierExporter()
        {

        }
        public CarrierExporter(XmlNode xn)
            : base(xn)
        { }


   

        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("carrier", dr["carrier"].ToString());
            xmlDocument = op.ExportCarrier("", qSender, null);

            return xmlDocument;
        }
    }
}

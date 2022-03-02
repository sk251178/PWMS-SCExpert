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
    class CompanyExporter : BaseIntegrationExportPlugin
    {
        public CompanyExporter(){}

        public CompanyExporter(XmlNode xn): base(xn){ }

        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            qSender.Add("DOCUMENT", dr["COMPANY"].ToString());
            qSender.Add("NOTES", dr["COMPANYTYPE"].ToString());
            xmlDocument = op.ExportCompany("", qSender, null);

            return xmlDocument;
        }
    }
}

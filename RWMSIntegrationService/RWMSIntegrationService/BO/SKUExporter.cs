using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Data;
using System.Xml;
using Made4Net.DataAccess;
using Made4Net.Shared;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using ExpertObjectMapper;


namespace RWMSIntegrationService.BO
{
    class SKUExporter : BaseIntegrationExportPlugin
    {
        public SKUExporter(XmlNode xn): base(xn)
        { }

        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("SKU", dr["SKU"].ToString());
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            xmlDocument = op.ExportSku("", qSender, null);

            SetHomeSlot(ref xmlDocument);

            return xmlDocument;
        }

        private void SetHomeSlot(ref XmlDocument oXmlDoc)
        {
            string sConsignee = "", sSku = "";
            sConsignee = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/CONSIGNEE").InnerText;
            sSku = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/SKU").InnerText;

            string sSql = string.Format("select top 1 isnull(location,'') as HomeSlot from PICKLOC where CONSIGNEE = {0} and SKU = {1}",
                    Made4Net.Shared.Util.FormatField(sConsignee, "", false), Made4Net.Shared.Util.FormatField(sSku, "", false));
            string sHomeSlot = Convert.ToString(DataInterface.ExecuteScalar(sSql,""));

            XmlElement oXmlElem = oXmlDoc.CreateElement("HomeSlot");
            oXmlElem.InnerText = sHomeSlot;
            oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(oXmlElem);
        }

    }
}

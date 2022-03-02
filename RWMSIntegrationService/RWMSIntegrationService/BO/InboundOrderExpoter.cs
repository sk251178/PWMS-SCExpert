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
    class InboundOrderExporter : BaseIntegrationExportPlugin
    {
        public InboundOrderExporter()
        {}
        
        public InboundOrderExporter(XmlNode xn): base(xn)
        {}

        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            qSender.Add("DOCUMENT", dr["ORDERID"].ToString());
            xmlDocument = op.ExportInBound("", qSender, null);
            writeToLog(string.Format("original Xml from table : \r\n {0}", xmlDocument.InnerXml));
            //InsertNodesOfTotals(xmlDocument);
            xmlDocument = AddNodesToXml(xmlDocument);
            return xmlDocument;
        }

        private XmlDocument AddNodesToXml(XmlDocument xmlDocument)
        {
            string sConsignee;
            string sOrderid;
            Dictionary<string, string> dictXmlNodesAndValues = new Dictionary<string, string>();

            sConsignee = xmlDocument.SelectSingleNode("DATACOLLECTION/DATA/CONSIGNEE").InnerText;
            sOrderid = xmlDocument.SelectSingleNode("DATACOLLECTION/DATA/ORDERID").InnerText;


            dictXmlNodesAndValues.Add("RECIEVEDPALLETS", GetQtyRecieved("PALLET", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("RECIEVEDCASES", GetQtyRecieved("CASE", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("ORDEREDPALLETS", GetQtyOrdered("PALLET", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("ORDEREDCASES", GetQtyOrdered("CASE", sConsignee, sOrderid));

            dictXmlNodesAndValues.Add("TOTALWEIGHT", GetTotalWeight("EACH", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("TOTALRECIEVEDWEIGHT", GetTotalRecievedWeight("EACH", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("TOTALCUBE", GetTotalCube("EACH", sConsignee, sOrderid));
            dictXmlNodesAndValues.Add("TOTALRECIEVEDCUBE", GetTotalRecievedCube("EACH", sConsignee, sOrderid));

            foreach (KeyValuePair<string, string> KVP in dictXmlNodesAndValues)
            {
                XmlNode xn = xmlDocument.CreateElement(KVP.Key);
                xn.InnerText = KVP.Value;
                xmlDocument.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(xn);
            }
            return xmlDocument;
        }

        private string GetQtyRecieved(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED / su.UNITSPERLOWESTUOM) AS DECIMAL(18,2))) as QTYRECEIVED from INBOUNDORDDETAIL id
                                       left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                       where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                       group by id.CONSIGNEE, ORDERID",
                                      Uom,
                                      Consignee,
                                      Orderid
                                      );
            return GetAns(sql);
        }

        private string GetQtyOrdered(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORDERED / su.UNITSPERLOWESTUOM) AS DECIMAL(18,2))) as QTYORDERED from INBOUNDORDDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                        group by id.CONSIGNEE, ORDERID",
                                       Uom,
                                       Consignee,
                                       Orderid
                                       );
            return GetAns(sql);
        }

        private string GetTotalCube(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORDERED * su.VOLUME) AS DECIMAL(18,2))) as TOTALCUBE from INBOUNDORDDETAIL id
                                       left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                       where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                       group by id.CONSIGNEE, ORDERID",
                                      Uom,
                                      Consignee,
                                      Orderid
                                      );
            return GetAns(sql);
        }

        private string GetTotalWeight(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORDERED * su.GROSSWEIGHT) AS DECIMAL(18,2))) as TOTALWEIGHT from INBOUNDORDDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                        group by id.CONSIGNEE, ORDERID",
                                       Uom,
                                       Consignee,
                                       Orderid
                                       );
            return GetAns(sql);
        }

        private string GetTotalRecievedCube(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED * su.VOLUME) AS DECIMAL(18,2))) as TOTALCUBE from INBOUNDORDDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                        group by id.CONSIGNEE, ORDERID",
                                       Uom,
                                       Consignee,
                                       Orderid
                                       );
            return GetAns(sql);
        }

        private string GetTotalRecievedWeight(String Uom, String Consignee, String Orderid)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED * su.GROSSWEIGHT) AS DECIMAL(18,2)))  as TOTALWEIGHT from INBOUNDORDDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and ORDERID = '{2}'
                                        group by id.CONSIGNEE, ORDERID",
                                       Uom,
                                       Consignee,
                                       Orderid
                                       );
            return GetAns(sql);
        }

        private string GetAns(String sql)
        {

            try
            {
                return Made4Net.DataAccess.DataInterface.ExecuteScalar(sql).ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public override List<XmlDocument> SplitXmlDocFile(XmlDocument oXmlDoc)
        {
            SetDownloadType(oXmlDoc);
            //AppendHeaderDataToLines(oXmlDoc);
            string sLinesXML = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/LINES").InnerXml;
            XmlDocument oDetailDoc = new XmlDocument();
            oDetailDoc.LoadXml(string.Format("<DATACOLLECTION><DATA>{0}</DATA></DATACOLLECTION>", sLinesXML));
            List<XmlDocument> oFiles = new List<XmlDocument>();
            XmlNode LinesNode = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/LINES");
            LinesNode.ParentNode.RemoveChild(LinesNode);
            oFiles.Add(oXmlDoc);
            oFiles.Add(oDetailDoc);
            return oFiles;
        }

        //private void AppendHeaderDataToLines(XmlDocument oXmlDoc)
        //{
        //    XmlNodeList oLines = oXmlDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE");
        //    for (int i = 0; i < oLines.Count; i++)
        //    {
        //        XmlNode oCurrentLine = oLines[i];
        //        XmlElement oPONumber = oXmlDoc.CreateElement("PO_Number");
        //        XmlElement oOrderDate = oXmlDoc.CreateElement("Order_Date");
        //        XmlElement oOwnerCode = oXmlDoc.CreateElement("Owner_Code");
        //        oCurrentLine.AppendChild(oPONumber);
        //        oCurrentLine.AppendChild(oOrderDate);
        //        oCurrentLine.AppendChild(oOwnerCode);
        //    }
        //}

        private void SetDownloadType(XmlDocument oXmlDoc)
        {
            string sConsignee ="", sOrderid = "";
            sConsignee = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/BuyerCode").InnerText;
            sOrderid = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/OrderNumber").InnerText;
            string sSql = string.Format("select isnull(datediff(mi,adddate,editdate),0) from inboundordheader where consignee = {0} and orderid = {1}",
                Made4Net.Shared.Util.FormatField(sConsignee, "", false), Made4Net.Shared.Util.FormatField(sOrderid, "", false));
            Int32 ret = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql, ""));
            string sDownloadType = "";
            switch (ret)
            {
                case 0:
                    sDownloadType = "A";
                    break;
                default:
                    sDownloadType = "B";
                    break;
            }
            oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/DownloadType").InnerText = sDownloadType;
        }
    }
}
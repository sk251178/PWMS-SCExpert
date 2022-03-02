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
    class FlowThroughExporter : BaseIntegrationExportPlugin
    {
        public FlowThroughExporter()
        {

        }
        public FlowThroughExporter(XmlNode xn)
            : base(xn)
        { }



        public override XmlDocument GetObjectAsXml(DataRow dr)
        {
            InitLogger(this.LogFilePrefix);
            XmlDocument xmlDocument = new XmlDocument();
            ObjectProcessor op = new ObjectProcessor();
            QMsgSender qSender = new QMsgSender();
            qSender.Add("CONSIGNEE", dr["CONSIGNEE"].ToString());
            qSender.Add("DOCUMENT", dr["FLOWTHROUGH"].ToString());
            xmlDocument = op.ExportFlowthrough("", qSender, null);
            writeToLog(string.Format("original Xml from table : \r\n {0}", xmlDocument.InnerXml));
            //InsertNodesOfTotals(xmlDocument);
            xmlDocument = AddNodesToXml(xmlDocument);
            return xmlDocument;

        }

        private XmlDocument AddNodesToXml(XmlDocument xmlDocument)
        {
            string sConsignee;
            string sFlowthrough;
            Dictionary<string, string> dictXmlNodesAndValues = new Dictionary<string, string>();

            sConsignee = xmlDocument.SelectSingleNode("DATACOLLECTION/DATA/CONSIGNEE").InnerText;
            sFlowthrough = xmlDocument.SelectSingleNode("DATACOLLECTION/DATA/FLOWTHROUGH").InnerText;


            dictXmlNodesAndValues.Add("RECIEVEDPALLETS", GetQtyRecieved("PALLET", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("RECIEVEDCASES", GetQtyRecieved("CASE", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("ORDEREDPALLETS", GetQTYORIGINAL("PALLET", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("ORDEREDCASES", GetQTYORIGINAL("CASE", sConsignee, sFlowthrough));

            dictXmlNodesAndValues.Add("TOTALWEIGHT", GetTotalWeight("EACH", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("TOTALRECIEVEDWEIGHT", GetTotalRecievedWeight("EACH", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("TOTALCUBE", GetTotalCube("EACH", sConsignee, sFlowthrough));
            dictXmlNodesAndValues.Add("TOTALRECIEVEDCUBE", GetTotalRecievedCube("EACH", sConsignee, sFlowthrough));

            foreach (KeyValuePair<string, string> KVP in dictXmlNodesAndValues)
            {
                XmlNode xn = xmlDocument.CreateElement(KVP.Key);
                xn.InnerText = KVP.Value;
                xmlDocument.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(xn);
            }


            appendSkuDataToLines(xmlDocument);
            SetDownloadType(xmlDocument);

            return xmlDocument;
        }

        private void appendSkuDataToLines(XmlDocument pDoc)
        {
            string consignee = pDoc.SelectSingleNode("DATACOLLECTION/DATA/CONSIGNEE").InnerText;
            string flowthrough = pDoc.SelectSingleNode("DATACOLLECTION/DATA/FLOWTHROUGH").InnerText;

            string sql = string.Format("select sku.consignee, sku.sku, sku.skudesc, sku.STORAGECLASS from flowthroughdetail fd inner join sku on fd.consignee = sku.consignee and fd.sku = sku.sku where fd.consignee={0} and fd.flowthrough={1}",
                Made4Net.Shared.Util.FormatField(consignee, null, false), Made4Net.Shared.Util.FormatField(flowthrough, null, false));
            DataTable SKUdt = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(sql, SKUdt, false, null);

            DataTable UOMdt = new DataTable();
            sql = string.Format("select fd.consignee, fd.SKU, fd.QTYMODIFIED, VOLUME, GROSSWEIGHT from skuuom inner join flowthroughdetail fd on fd.consignee = skuuom.consignee and fd.sku = skuuom.sku where uom='EACH' and fd.consignee={0} and fd.flowthrough={1}",
                Made4Net.Shared.Util.FormatField(consignee, null, false), Made4Net.Shared.Util.FormatField(flowthrough, null, false));

            Made4Net.DataAccess.DataInterface.FillDataset(sql, UOMdt, false, null);

            string sku = "";

            XmlNode descNode;
            XmlNode strgClassNode;

            XmlNode volumeNode;
            XmlNode weightNode;
            DataRow[] dr;
            
            foreach (XmlNode node in pDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE"))
            {
                sku = node.SelectSingleNode("SKU").InnerText;
                dr = SKUdt.Select(string.Format("CONSIGNEE={0} and SKU={1}", Made4Net.Shared.Util.FormatField(consignee, null, false), Made4Net.Shared.Util.FormatField(sku, null, false)));
                
                descNode = pDoc.CreateElement("SKUDESC");
                strgClassNode = pDoc.CreateElement("STORAGECLASS");
                if (dr.Length > 0)
                {
                    descNode.InnerText = dr[0]["SKUDESC"].ToString();
                    strgClassNode.InnerText = dr[0]["STORAGECLASS"].ToString();
                }
                node.AppendChild(descNode);
                node.AppendChild(strgClassNode);

                dr = UOMdt.Select(string.Format("CONSIGNEE={0} and SKU={1}", Made4Net.Shared.Util.FormatField(consignee, null, false), Made4Net.Shared.Util.FormatField(sku, null, false)));

                volumeNode = pDoc.CreateElement("EXTENDEDCUBE");
                weightNode = pDoc.CreateElement("EXTENDEDWEIGHT");
                if (dr.Length > 0)
                {
                    volumeNode.InnerText = (Convert.ToDecimal(dr[0]["QTYMODIFIED"]) * Convert.ToDecimal(dr[0]["VOLUME"])).ToString();
                    weightNode.InnerText = (Convert.ToDecimal(dr[0]["QTYMODIFIED"]) * Convert.ToDecimal(dr[0]["GROSSWEIGHT"])).ToString();
                }

                node.AppendChild(volumeNode);
                node.AppendChild(weightNode);
            }
        }

        private void SetDownloadType(XmlDocument oXmlDoc)
        {
            try
            {
                string sConsignee = "", sOrderid = "";
                sConsignee = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/CONSIGNEE").InnerText;
                sOrderid = oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/FLOWTHROUGH").InnerText;
                string sSql = string.Format("select isnull(datediff(mi,adddate,editdate),0) from FLOWTHROUGHHEADER where consignee = {0} and FLOWTHROUGH = {1}",
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
                XmlNode node = oXmlDoc.CreateElement("DOWNLOADTYPE");
                //oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA/DownloadType").InnerText = sDownloadType;
                node.InnerText = sDownloadType;
                oXmlDoc.SelectSingleNode("DATACOLLECTION/DATA").AppendChild(node);
            }
            catch { }
        }

        private string GetQtyRecieved(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED / su.UNITSPERLOWESTUOM) AS DECIMAL(18,2))) as QTYRECEIVED from FLOWTHROUGHDETAIL id
                                       left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                       where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                       group by id.CONSIGNEE, FLOWTHROUGH",
                                      Uom,
                                      Consignee,
                                      Flowthrough
                                      );
            return GetAns(sql);
        }

        private string GetQTYORIGINAL(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORIGINAL / su.UNITSPERLOWESTUOM) AS DECIMAL(18,2))) as QTYORIGINAL from FLOWTHROUGHDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                        group by id.CONSIGNEE, FLOWTHROUGH",
                                       Uom,
                                       Consignee,
                                       Flowthrough
                                       );
            return GetAns(sql);
        }

        private string GetTotalCube(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORIGINAL * su.VOLUME) AS DECIMAL(18,2))) as TOTALCUBE from FLOWTHROUGHDETAIL id
                                       left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                       where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                       group by id.CONSIGNEE, FLOWTHROUGH",
                                      Uom,
                                      Consignee,
                                      Flowthrough
                                      );
            return GetAns(sql);
        }

        private string GetTotalWeight(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYORIGINAL * su.GROSSWEIGHT) AS DECIMAL(18,2))) as TOTALWEIGHT from FLOWTHROUGHDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                        group by id.CONSIGNEE, FLOWTHROUGH",
                                       Uom,
                                       Consignee,
                                       Flowthrough
                                       );
            return GetAns(sql);
        }

        private string GetTotalRecievedCube(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED * su.VOLUME) AS DECIMAL(18,2))) as TOTALCUBE from FLOWTHROUGHDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                        group by id.CONSIGNEE, FLOWTHROUGH",
                                       Uom,
                                       Consignee,
                                       Flowthrough
                                       );
            return GetAns(sql);
        }

        private string GetTotalRecievedWeight(String Uom, String Consignee, String Flowthrough)
        {
            String sql = String.Format(@"select (CAST(SUM(QTYRECEIVED * su.GROSSWEIGHT) AS DECIMAL(18,2)))  as TOTALWEIGHT from FLOWTHROUGHDETAIL id
                                        left outer join SKUUOM su on su.CONSIGNEE = id.CONSIGNEE and id.SKU = su.SKU and su.UOM = '{0}'
                                        where id.CONSIGNEE = '{1}' and FLOWTHROUGH = '{2}'
                                        group by id.CONSIGNEE, FLOWTHROUGH",
                                       Uom,
                                       Consignee,
                                       Flowthrough
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
            //SetDownloadType(oXmlDoc);
            ////AppendHeaderDataToLines(oXmlDoc);
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

    }
}
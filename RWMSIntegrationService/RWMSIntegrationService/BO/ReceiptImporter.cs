using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Data;
using System.Xml;
using ExpertObjectMapper;
using Made4Net.Shared;
using WMS.Logic;
using System.Globalization;

namespace RWMSIntegrationService.BO
{
    class ReceiptImporter :BaseIntegrationImportPlugin
    {
        public ReceiptImporter()
        {

        }
        public ReceiptImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }


        private bool createReceiptHeaderAndFillReceiptLinesFromAppontmentXml(XmlNode xn)
        {
            InitLogger(this.LogFilePrefix);
            string docType = WMS.Lib.DOCUMENTTYPES.INBOUNDORDER;
           
            ReceiptHeader oReceipt = new ReceiptHeader();

            try
            {
                writeToLog(string.Format("creating  receipt({0}) header for order  {1} . ", xn.SelectSingleNode("APPOINTMENTID").InnerXml, xn.SelectSingleNode("ORDERID").InnerXml));

                if (WMS.Logic.ReceiptHeader.Exists(xn.SelectSingleNode("APPOINTMENTID").InnerXml))
                {
                    writeToLog(string.Format("Receipt {0} already exist. Terminating process.", xn.SelectSingleNode("APPOINTMENTID").InnerXml));
                    return false;
                }
                else{
                    oReceipt.CreateNew(xn.SelectSingleNode("APPOINTMENTID").InnerXml, DateTime.ParseExact(xn.SelectSingleNode("GATEINTIME").InnerXml, "MM/dd/yyyy H:m", CultureInfo.InvariantCulture), xn.SelectSingleNode("APPOINTMENTID").InnerXml,
                                         string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, false, Common.GetCurrentUser());
                }

            }
            catch (Exception ex)
            {

                string err = string.Format("Failed create receipt({0}) header for order {1}  due to the following Error {2} ", xn.SelectSingleNode("APPOINTMENTID").InnerXml, xn.SelectSingleNode("ORDERID").InnerXml, ex.ToString());

                writeToLog(err);
                return false;
            }
            string sOrderid = xn.SelectSingleNode("ORDERID").InnerXml;
            InboundOrderHeader oOrder = new WMS.Logic.InboundOrderHeader(this.CONSIGNEE, sOrderid,true);
            writeToLog(string.Format("strating to create receipt lines for order  {0} . ",sOrderid));
            foreach (InboundOrderDetail id in oOrder.Lines)
            {
                writeToLog(string.Format("create receipt line for order {0} , line : {1} . ", sOrderid,id.ORDERLINE));
                try
                {

                    oReceipt.addLineFromOrder(id.CONSIGNEE, id.ORDERID, id.ORDERLINE, null, null, id.INVENTORYSTATUS, oOrder.SOURCECOMPANY,
                                                      oOrder.COMPANYTYPE, -1, null, null, id.Attributes.Attributes, 0, null, null, docType);
                }
                catch (Exception ex)
                {

                 string err= string.Format("Failed create receipt line for order {0} , line : {1} . due to the following Error {2} ", sOrderid, id.ORDERLINE,ex.ToString());

                 writeToLog(err);
                 return false;
                }
            }
             writeToLog(string.Format("creating receipt lines from order {0} finished succssesfully",sOrderid) );
             return true;
        }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            int iNumNodes = xdoc.SelectNodes("RETALIX/APPT").Count;

            if (mLogger != null) { mLogger.WriteLine(string.Format("Receipt file loaded and contains {0} nodes", iNumNodes.ToString()),true); }
            
            foreach (XmlNode xn in xdoc.SelectNodes("RETALIX/APPT"))
            {
                if (!createReceiptHeaderAndFillReceiptLinesFromAppontmentXml(xn))
                    return false;
            }
            return true;
        }
       
    }
}

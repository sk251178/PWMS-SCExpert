using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;

namespace RWMSIntegrationService.BO
{
    class FlowthroughImporter : BaseIntegrationImportPlugin
    {
        public FlowthroughImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            //WMS.Logic.Flowthrough ft;
            //foreach (XmlNode oNode in xdoc.SelectNodes("DATACOLLECTION/DATA"))
            //{
            //    string sConsignee, sFlowThrough;
            //    sConsignee = oNode.SelectSingleNode("CONSIGNEE").InnerText;
            //    sFlowThrough = oNode.SelectSingleNode("FLOWTHROUGH").InnerText;
            //    if (!WMS.Logic.Flowthrough.Exists(sConsignee, sFlowThrough))
            //    {
            //        writeToLog(string.Format("Ivalid Flowthrough id. Flowthrough {0} does not exist in the warehouse.", sFlowThrough));
            //        //bProcSucceeded = false;
            //        //continue;
            //        return false;
            //    }

            //    ft = new WMS.Logic.Flowthrough(sConsignee, sFlowThrough,true);
            //    if (!ft.STATUS.Equals(WMS.Lib.Statuses.Flowthrough.STATUSNEW, StringComparison.OrdinalIgnoreCase))
            //    {
            //        writeToLog(string.Format("Ivalid Flowthrough status. Flowthrough {0} is not in status NEW.", sFlowThrough));
            //        return false;
            //    }
            //}
            //Commented for PWMS-841 start  
            //bool res = ValidateFTData(xdoc);
            //if (!res)
            //    return false;
            //End commented for PWMS-841 end   

            string err = "";
            ExpertObjectMapper.ObjectProcessor op = new ExpertObjectMapper.ObjectProcessor();
            bool failed = false;
            XmlDocument oTransactionXML;
            foreach (XmlNode oNode in xdoc.SelectNodes("DATACOLLECTION/DATA"))
            {
                oTransactionXML = new XmlDocument();
                oTransactionXML.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><BUSINESSOBJECT><BOTYPE>FLOWTHROUGH</BOTYPE></BUSINESSOBJECT>"));
                oTransactionXML.SelectSingleNode("BUSINESSOBJECT").AppendChild(oTransactionXML.ImportNode(oNode, true));
                op.ProcessFlowthrough(oTransactionXML, mLogger, ref err);
                if (!string.IsNullOrEmpty(err))
                    failed = true;
            }
            if (failed)
                return false;

            return true;
        }

        protected virtual bool ValidateFTData(XmlDocument pDoc)
        {
            return true;
        }
    }
}

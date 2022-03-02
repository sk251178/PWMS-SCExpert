using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RWMSIntegrationService.BO
{
    class FlowThroughOutboundImporter : FlowthroughImporter
    {
        public FlowThroughOutboundImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }

        protected override bool ValidateFTData(System.Xml.XmlDocument pDoc)
        {
            WMS.Logic.Flowthrough ft;
            foreach (XmlNode oNode in pDoc.SelectNodes("DATACOLLECTION/DATA"))
            {
                string sConsignee, sFlowThrough;
                sConsignee = oNode.SelectSingleNode("CONSIGNEE").InnerText;
                sFlowThrough = oNode.SelectSingleNode("FLOWTHROUGH").InnerText;
                if (!WMS.Logic.Flowthrough.Exists(sConsignee, sFlowThrough))
                {
                    writeToLog(string.Format("Ivalid Flowthrough id. Flowthrough {0} does not exist in the warehouse.", sFlowThrough));
                    return false;
                }

                ft = new WMS.Logic.Flowthrough(sConsignee, sFlowThrough, true);
                if (ft.STATUS.Equals(WMS.Lib.Statuses.Flowthrough.LOADING, StringComparison.OrdinalIgnoreCase) ||
                    ft.STATUS.Equals(WMS.Lib.Statuses.Flowthrough.LOADED, StringComparison.OrdinalIgnoreCase) ||
                    ft.STATUS.Equals(WMS.Lib.Statuses.Flowthrough.SHIPPING, StringComparison.OrdinalIgnoreCase) ||
                    ft.STATUS.Equals(WMS.Lib.Statuses.Flowthrough.SHIPPED, StringComparison.OrdinalIgnoreCase))
                {
                    writeToLog(string.Format("Ivalid Flowthrough status. Flowthrough {0} status is {1}.", sFlowThrough, ft.STATUS));
                    return false;
                }
            }
            return true;
        }

    }

}

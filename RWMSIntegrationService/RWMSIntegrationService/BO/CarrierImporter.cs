using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;

namespace RWMSIntegrationService.BO
{
    class CarrierImporter : BaseIntegrationImportPlugin
    {
        public CarrierImporter(XmlNode xn, string sConsignee): base(xn, sConsignee){ }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            string err = "";
            ExpertObjectMapper.ObjectProcessor op = new ExpertObjectMapper.ObjectProcessor();
            op.ProcessCarrier(xdoc, mLogger, ref err);
            if (string.IsNullOrEmpty(err))
                return true;

            return false;
        }
    }
}

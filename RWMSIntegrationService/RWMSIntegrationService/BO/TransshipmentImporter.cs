using System;
using System.Collections.Generic;
using System.Text;
using RWMSIntegrationService.Interfaces;
using System.Xml;

namespace RWMSIntegrationService.BO
{
    class TransshipmentImporter : BaseIntegrationImportPlugin
    {
        public TransshipmentImporter(XmlNode xn, string sConsignee)
            : base(xn, sConsignee)
        { }

        protected override bool UpdateDataFromTranslatedXml(XmlDocument xdoc)
        {
            string err = "";
            ExpertObjectMapper.ObjectProcessor op = new ExpertObjectMapper.ObjectProcessor();
            op.ProcessTransshipment(xdoc, mLogger, ref err);
            if (string.IsNullOrEmpty(err))
                return true;


            return false;
        }
    }
}

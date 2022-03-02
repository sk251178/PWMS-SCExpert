using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;

namespace RWMSIntegrationService.Interfaces
{
    interface IBaseIntegrationPluginDataProvider
    {
        DataTable GetData();
        XmlDocument GetObjectAsXml(DataRow dr);
        XmlDocument ConvertRWMSXmlToRDSXml(XmlDocument xDoc);
    }
}

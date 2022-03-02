using System.Xml;

namespace SCExpertConnectPlugins
{
    public interface ISCExpertExporter
    {
        int Export(XmlDocument oXMLDoc);

        void SaveRawXML(XmlDocument oXMLDoc, string Refname);
    }
}
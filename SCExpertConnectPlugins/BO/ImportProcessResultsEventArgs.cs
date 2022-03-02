using System;
using System.Collections.Generic;
using System.Xml;

namespace SCExpertConnectPlugins.BO
{
    public class ImportProcessResultsEventArgs : EventArgs
    {
        public readonly List<XmlDocument> lstResultXmlDocs;

        public ImportProcessResultsEventArgs(List<XmlDocument> lstProcessedFiles)
        {
            this.lstResultXmlDocs = lstProcessedFiles;
        }
    }
}
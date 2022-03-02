using System;
using System.Collections.Generic;
using System.Text;
using Made4Net.Shared;
using System.IO;
using System.Xml;

namespace SCExpertConnectPlugins
{
    public interface IFileImporter
    {
        XmlDocument ProcessFile(FileInfo oFile, Made4Net.Shared.Logging.LogFile oLogger);
    }
}

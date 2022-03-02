using ExpertObjectMapper;
using System.Collections.Generic;

namespace SCExpertConnectPlugins
{
    public interface ISCExpertImporter
    {
        void Import();

        void ProcessResultFromSCExpertConect(string pTransactionSet, Dictionary<string, DataMapperProcessResult> pObjectDataMapperResult);
    }
}
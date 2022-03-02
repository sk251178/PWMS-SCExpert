using System.Data;

namespace SCExpertConnectPlugins.BO
{
    public class PluginParams
    {

        public int PluginId { get; set; }

        public string ParamName { get; set; }

        public string ParamValue { get; set; }


        public PluginParams(int pPluginId, DataRow row)
        {
            PluginId = pPluginId;
            if (row != null)
            {
                ParamName = row["PARAMNAME"].ToString();
                ParamValue = row["PARAMVALUE"].ToString();
            }
        }
    }
}
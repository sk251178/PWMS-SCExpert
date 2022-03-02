using System;
using System.Data;
using Made4Net.DataAccess;
using WMS.Logic;

namespace WMS.SchedulerTasks
{
    public class LimboMerge
    {
        public LimboMerge()
        {

        }

        public void MergeLoads(string strLoggerPath)
        {
            // Get All loads in Limbo
            string SqlLimboLoads = "SELECT CONSIGNEE, SKU, COUNT(1) LIMBOLOADS FROM INVLOAD WHERE STATUS='LIMBO' GROUP BY CONSIGNEE, SKU HAVING COUNT(1) > 1";
            DataTable dt = new DataTable();
            DataInterface.FillDataset(SqlLimboLoads, dt, false, "");
            Random random = new Random();
            Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile(strLoggerPath, "LOADMERGER_" + random.Next(100000).ToString() , false);
            _qhlogger.WriteSeperator();
            _qhlogger.WriteLine("Found " + dt.Rows.Count + " for loads merge", false);
            foreach (DataRow dr in dt.Rows)
            {
                _qhlogger.WriteLine("Starting to merge " + Convert.ToString(dr["SKU"]) + ", merging " + Convert.ToString(dr["LIMBOLOADS"]), false);
                DataTable dtSkuLoads = new DataTable();
                string SqlSkuLoads = "SELECT LOADID FROM INVLOAD WHERE STATUS='LIMBO' AND CONSIGNEE = '" + Convert.ToString(dr["CONSIGNEE"]) + "' AND SKU = '" + Convert.ToString(dr["SKU"]) + "'";
                DataInterface.FillDataset(SqlSkuLoads, dtSkuLoads, false, "");

                WMS.Logic.Load oLoad = new WMS.Logic.Load(  Convert.ToString( dtSkuLoads.Rows[0]["LOADID"]) , true);
                foreach (DataRow drload in dtSkuLoads.Rows)
                {
                    if (Convert.ToString(drload["LOADID"]) != oLoad.LOADID)
                    {
                        _qhlogger.WriteLine("Merging load " + Convert.ToString(dr["LOADID"]), false);
                        WMS.Logic.Load tmpLoad = new WMS.Logic.Load(Convert.ToString(drload["LOADID"]), true);
                        oLoad.Merge(tmpLoad);
                    }
                }
            }
            _qhlogger.WriteSeperator();
            _qhlogger.Close();

        }
    }
}
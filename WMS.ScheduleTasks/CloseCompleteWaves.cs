using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Made4Net.DataAccess;
using WMS.Logic;

namespace WMS.ScheduleTasks
{
    public class CloseCompleteWaves
    {
        public CloseCompleteWaves()
        {

        }

        public void Close()
        {
            try
            {
                string waves = "SELECT WAVE FROM WAVE WHERE (STATUS='PLANNED' OR STATUS='RELEASED')";
                DataTable dt = new DataTable();
                DataInterface.FillDataset(waves, dt, false, "");
                foreach (DataRow wdr in dt.Rows)
                {
                    string OpenOrder = "SELECT COUNT(1) FROM OUTBOUNDORHEADER WHERE WAVE ='" + Convert.ToString(wdr["WAVE"]) + "' AND (STATUS<>'CANCELED' AND STATUS<>'PICKED' AND STATUS<>'STAGED' AND STATUS<>'PACKED' AND STATUS<>'LOADED' AND STATUS<>'SHIPPED')";
                    if (Convert.ToInt16(DataInterface.ExecuteScalar(OpenOrder)) == 0)
                    {
                        Wave ws = new Wave(Convert.ToString(wdr["WAVE"]), true);
                        ws.Complete("SCHEDULER");
                    }
                }
            }
            catch
            {

            }
        }
    }
}

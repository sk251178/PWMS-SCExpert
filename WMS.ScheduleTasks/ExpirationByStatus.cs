using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;

namespace WMS.SchedulerTasks
{
    public class ExpirationByStatus
    {
        public ExpirationByStatus()
        {

        }

        //public void SetExpiredStatus()
        //{
        //    string sql = "SELECT IV.LOADID FROM INVLOAD IV INNER JOIN LOADATTRIBUTES LD ON IV.LOADID=LD.LOADID AND EXPIRYDATE IS NOT NULL WHERE LD.EXPIRYDATE < GETDATE() AND IV.STATUS IN ('AVAILABLE','RETURN')";
        //    DataTable dt = new DataTable();
        //    DataInterface.FillDataset(sql, dt, false, "");
        //    foreach (DataRow drLoad in dt.Rows)
        //    {
        //        Load ld = new Load(Convert.ToString(drLoad["LOADID"]), true);
        //        ld.setStatus("EXPIRED", "", "ScheduleTasks");
        //    }
        //}

        public void SetExpiredStatus(string StatusList)
        {
            string sql = "SELECT IV.LOADID FROM INVLOAD IV INNER JOIN LOADATTRIBUTES LD ON IV.LOADID=LD.LOADID AND EXPIRYDATE IS NOT NULL WHERE LD.EXPIRYDATE < GETDATE() AND IV.STATUS IN (" + StatusList + ")";
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sql, dt, false, "");
            foreach (DataRow drLoad in dt.Rows)
            {
                try
                {
                    Load ld = new Load(Convert.ToString(drLoad["LOADID"]), true);
                    ld.setStatus("EXPIRED", "", "ScheduleTasks");
                }
                catch { }
                
            }
        }
    }
}

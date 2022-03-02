using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Made4Net.DataAccess; 

namespace WMS.SchedulerTasks
{
    public class SnapShotToFile
    {
        public SnapShotToFile()
        {

        }

        public void CreateFile(string FileFullPath)
        {
            string FileSql = "SELECT * FROM COLMOBIL_SNAPSHOT";
            DataTable dt = new DataTable();
            DataInterface.FillDataset(FileSql, dt, false, "");

            StreamWriter sw = new StreamWriter(FileFullPath);

            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Convert.ToString(dr["wms402"]));

                sb.Append("\t");

                sb.Append(Convert.ToString(dr["snapshotdate"]));

                sb.Append("\t");

                sb.Append(Convert.ToString(dr["SKU"]));

                sb.Append("\t");

                sb.Append(Convert.ToString(dr["status"]));

                sb.Append("\t");

                sb.Append(Convert.ToString(dr["units"]));

                sw.WriteLine( sb.ToString() );

                sw.Flush(); 
            }

            sw.Close(); 
        }
    }
}

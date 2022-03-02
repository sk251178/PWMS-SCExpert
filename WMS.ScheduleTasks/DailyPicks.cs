using System;
using System.Data;
using Made4Net.DataAccess;
using WMS.Logic; 

namespace WMS.ScheduleTasks
{
	/// <summary>
	/// Summary description for DailyPicks.
	/// </summary>
	public class DailyPicks : Made4Net.Shared.JobScheduling.ScheduledJobBase
	{
		protected int _daysrange;

		public DailyPicks() : base()
		{
			try
			{
				_daysrange = Convert.ToInt32( Made4Net.Shared.Util.GetSystemParameterValue("DailyPicksDays") );
			}
			catch
			{
				_daysrange = 1;
			}
		}

		public override void Execute()
		{
			// Get list of all SKU's
			DataTable SkuTable = new DataTable();
			string DailyPickSql = "SELECT CONSIGNEE, SKU, ROUND( CAST ( COUNT(SKU) as decimal(9,2)) / " + _daysrange + ",2) AS DAILYPICKS FROM PICKDETAIL WHERE STATUS='COMPLETE' AND DATEDIFF(d, CONVERT(nvarchar(10), ADDDATE, 101) , CONVERT(nvarchar(10), DATEADD(d, -" + _daysrange + " ,GETDATE()), 101) ) < " + _daysrange + " GROUP BY CONSIGNEE, SKU";
			DataInterface.FillDataset(DailyPickSql, SkuTable, false, "");
			// For each SKU 
			foreach(DataRow dr in SkuTable.Rows)
			{
				// Update SKU data
				DataInterface.RunSQL("UPDATE SKU SET DAILYPICKS = " + Convert.ToDecimal( dr["DAILYPICKS"] ) + " WHERE CONSIGNEE = '" + Convert.ToString( dr["CONSIGNEE"] ) + "' AND SKU = '" + Convert.ToString( dr["SKU"] ) + "'","");
			}
		}

	}
}

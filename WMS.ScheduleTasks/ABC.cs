using System;
using System.Data;
using Made4Net.DataAccess;
using WMS.Logic; 

namespace WMS.ScheduleTasks
{
	/// <summary>
	/// Summary description for ABC.
	/// </summary>
	public class ABC : Made4Net.Shared.JobScheduling.ScheduledJobBase
	{
		protected int _dayscutoff; 
		protected string _checktype; 
		protected int _aper; 
		protected int _bper; 

		public ABC(int iDaysCutOff, string sCheckType, int aPercent, int bPercent) : base() 
		{ 
			try 
			{ 
				_dayscutoff = iDaysCutOff; 
			} 
			catch
			{ 
				_dayscutoff = 30; 
			} 
			_checktype = sCheckType; 
			if (_checktype == null | _checktype == string.Empty) 
			{ 
				_checktype = "LINES"; 
			} 
			try 
			{ 
				_aper = aPercent; 
			} 
			catch
			{ 
				_aper = 10; 
			} 
			try 
			{ 
				_bper = bPercent; 
			} 
			catch
			{ 
				_bper = 80; 
			} 
		} 


		public ABC() : base()
		{

		}


		public override void Execute() 
		{     
			DataTable dt = new DataTable(); 
			DataRow dr; 
			string sql = string.Format("select sku.consignee, sku.sku, isnull(numlines/checkdate,0) AvgNumLines, isnull(unitsordered/checkdate,0) AvgUnits from sku left outer join " + "(select od.consignee, od.sku, count(1) as numlines, sum(qtyoriginal) as unitsordered, {0} as Checkdate " + "from outboundordetail od join sku on od.consignee = sku.consignee and od.sku = sku.sku " + "join outboundorheader oh on od.consignee = oh.consignee and od.orderid = oh.orderid " + "where(SKU.ADDDATE <= DateAdd(dd, -{0}, getdate()) And oh.createdate > DateAdd(dd, -{0}, getdate())) " + "group by od.consignee, od.sku " + "union " + "select od.consignee, od.sku, count(1) as numlines, sum(qtyoriginal) as unitsordered, datediff(dd,sku.adddate,getdate()) as Checkdate " + "from outboundordetail od join sku on od.consignee = sku.consignee and od.sku = sku.sku " + "join outboundorheader oh on od.consignee = oh.consignee and od.orderid = oh.orderid " + "where sku.adddate > dateadd(dd,-{0},getdate()) and oh.createdate >= sku.adddate " + "group by od.consignee, od.sku, sku.adddate) as Uni " + "on sku.consignee = uni.consignee and sku.sku = uni.sku", _dayscutoff); 
			if (_checktype == "LINES") 
			{ 
				sql = sql + " ORDER BY avgNumLines desc"; 
			} 
			else 
			{ 
				sql = sql + " ORDER BY AvgUnits desc"; 
			} 
    
			DataInterface.FillDataset(sql, dt, false, ""); 
			Int32 cntrec = dt.Rows.Count; 
			Int32 idx; 
			Int32 lastidx; 
			SKU sk; 
    
			for (idx = 0; idx <= cntrec * _aper / 100; idx++) 
			{ 
				dr = dt.Rows[idx]; 
				sk = new SKU((string)dr["CONSIGNEE"], (string)dr["SKU"], true); 
				sk.VELOCITY = "A"; 
				sk.Save(); 
			} 
			lastidx = idx; 
			for (idx = lastidx; idx <= lastidx + (cntrec * _bper / 100); idx++) 
			{ 
				dr = dt.Rows[idx]; 
				sk = new SKU((string)dr["CONSIGNEE"], (string)dr["SKU"], true);
				sk.VELOCITY = "B"; 
				sk.Save(); 
			} 
			lastidx = idx; 
			for (idx = lastidx; idx <= cntrec - 1; idx++) 
			{ 
				dr = dt.Rows[idx]; 
				sk = new SKU((string)dr["CONSIGNEE"], (string)dr["SKU"], true); 
				sk.VELOCITY = "C"; 
				sk.Save(); 
			} 
    
			dt.Dispose(); 
		} 

	}
}

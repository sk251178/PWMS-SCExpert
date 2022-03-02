using System;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;
using System.Configuration;

namespace WMS.Counting
{
	/// <summary>
	/// Summary description for CycleCounts.
	/// </summary>
	public class CycleCounts:Made4Net.Shared.QHandler
	{

		#region "Ctor"

		public CycleCounts():base("Counting",false)
		{
		}

		#endregion

		#region "Process Queue"

		protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
		{
			string evt = "";
			string consignee = "";
			string sku = "";
			string skuGroup = "";
			string pUser = "";
			string LoggingEnabled;
			WMS.Logic.LogHandler oCountingLogger = null;
			try
			{
				evt = qSender.Values["EVENT"];
				pUser = qSender.Values["USER"];

				//LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceUseLogs);
				if (LoggingEnabled == "1")
				{
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath() + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.CountingServiceLogDirectory;  
					oCountingLogger = new LogHandler(dirpath, "Counting_" + WMS.Logic.WMSEvents.get_EventDescription(Convert.ToInt32(evt)) + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + ".txt");
				}

				switch (Convert.ToInt32(evt))
				{
					case (int)WMS.Logic.WMSEvents.EventType.LoadCycleCounts:
						consignee = qSender.Values["CONSIGNEE"];
						sku = qSender.Values["SKU"];
						skuGroup = qSender.Values["SKUGROUP"];
						WMS.Logic.LoadsCycleCounts lcc = new WMS.Logic.LoadsCycleCounts();
						lcc.CreateCycleCountTasks(consignee,sku,skuGroup,oCountingLogger,pUser);
						break;
					case (int)WMS.Logic.WMSEvents.EventType.LocationCycleCounts:
						WMS.Logic.LocationCycleCounts lc = new WMS.Logic.LocationCycleCounts();
						lc.CreateCycleCountTasks(oCountingLogger,pUser);
						break;
				}

			}
			catch (Made4Net.Shared.M4NException ex)
			{
				if (oCountingLogger != null)
				{
					oCountingLogger.Write("Error Occured while Creating Cycle Counts Tasks... Error Details:");
					oCountingLogger.Write(ex.GetErrMessage(0));
				}
			}
			catch (Exception ex)
			{
				if (oCountingLogger != null)
				{
					oCountingLogger.Write("Error Occured while Creating Cycle Counts Tasks... Error Details:");
					oCountingLogger.Write(ex.ToString());
				}
			}
		}

		#endregion

	}
}
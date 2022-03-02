using System;
using System.Messaging;
using Made4Net.DataAccess;
using WMS.Logic;
using System.Data;
using Made4Net.Shared;

namespace WMS.Planner
{
	public class Planner:Made4Net.Shared.QHandler
	{
		public Planner():base("Planner",false)
		{

		}

		#region "Process Wave & Order"

		protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
		{
			string Action = "";
			bool SoftPlanning = false;
			string Wave = "";
			string consignee = "";
			string orderid = "";
			string pUser = "";
			bool shouldRelease = false;
			WMS.Logic.LogHandler oPlannerLogger = null;

			try
			{
				Action = qSender.Values["ACTION"];

				// Get from the message the variable that will tell the planner to run in soft mode
				SoftPlanning = Convert.ToBoolean(qSender.Values["SOFTPALN"]);

				if(Action == WMS.Lib.Actions.Audit.PLANWAVE)
					Wave = qSender.Values["WAVE"];
				else if(Action == WMS.Lib.Actions.Audit.PLANORDER)
				{
					consignee = qSender.Values["CONSIGNEE"];
					orderid = qSender.Values["ORDERID"];
				}
				pUser = qSender.Values["USER"];
				shouldRelease = Convert.ToBoolean(qSender.Values["DORELEASE"]);
			}
			catch
			{
				return;
			}

			if (pUser == null || pUser == "")
				pUser = WMS.Lib.USERS.SYSTEMUSER;

			try
			{
				// After MSG received create new logger
				string LoggingEnabled;
				//LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                LoggingEnabled =Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.PlannerServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.PlannerServiceUseLogs);
				// Checking if logging is enabled and creating new log file
				if (LoggingEnabled == "1")
				{
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.PlannerServiceLogDirectory;

					oPlannerLogger = new LogHandler(dirpath, "PL_" + Action + DateTime.Now.ToString("_ddMMyyyy_hhmmss_") + consignee + "_" + orderid + ".txt");
					oPlannerLogger.WriteTimeStamp = false;
				}

				// Try to set connection by one that arrived in the message
				try
				{
					Made4Net.Shared.Warehouse.setCurrentWarehouse(qSender.Values["WAREHOUSE"]);
					oPlannerLogger.Write("Processing message from [" + qSender.Values["WAREHOUSE"] + "] warehouse");
					DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
					oPlannerLogger.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
				}
				catch
				{

				}

				if (Action == WMS.Lib.Actions.Audit.PLANWAVE)
				{
					WMS.Logic.Planner planner = new WMS.Logic.Planner(SoftPlanning);
					planner.Plan(Wave, shouldRelease, pUser, oPlannerLogger);
				}
				else if (Action == WMS.Lib.Actions.Audit.PLANORDER)
				{
					WMS.Logic.Planner planner = new WMS.Logic.Planner(SoftPlanning);
					planner.PlanOrder(consignee, orderid, shouldRelease, pUser, oPlannerLogger);
				}
			}
			catch (Made4Net.Shared.M4NException ex)
			{
				if (oPlannerLogger != null)
				{
					oPlannerLogger.Write("Error Occured while planning Wave/Order... Error Details:");
					oPlannerLogger.Write(ex.Description);
				}
			}
			catch (Exception ex)
			{
				if (oPlannerLogger != null)
				{
					oPlannerLogger.Write("Error Occured while planning Wave/Order... Error Details:");
					oPlannerLogger.Write(ex.ToString());
				}
			}
		}

		#endregion

	}
}
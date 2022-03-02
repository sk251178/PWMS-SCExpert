using System;
using WMS.Logic;
using WMS.Lib;
using Made4Net.Shared;

namespace WMS.Releasor
{
	/// <summary>
	/// Summary description for Releasor.
	/// </summary>
	public class Releasor:Made4Net.Shared.QHandler
	{
		public Releasor():base("Releasor",false)
		{}

		protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
		{
			WMS.Logic.LogHandler oLogger = null;
			string LoggingEnabled;
			try
			{
				string Action = "";
				string Wave = "";
				string picklist = "";
				string pUser = "";
			//	LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ReleasorServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ReleasorServiceUseLogs);
				if (LoggingEnabled == "1")
				{
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ReleasorServiceLogDirectory;
					//string dirpath = Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", null);

					oLogger = new LogHandler(dirpath, "RL_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss_") + ".txt");
				}
				try
				{
					Action = qSender.Values["ACTION"];

					if (Action == WMS.Lib.Actions.Audit.RELEASEWAVE)
					{
						Wave = qSender.Values["WAVE"];
						if (LoggingEnabled == "1")
						{
							oLogger.Write("Starting to Release Wave :" + Wave);
						}
					}
					else if(Action == WMS.Lib.Actions.Audit.RELEASEPICKLIST)
					{
						picklist = qSender.Values["PICKLISTID"];
						if (LoggingEnabled == "1")
						{
							oLogger.Write("Starting to Release Pick List :" + picklist);
						}
					}
					pUser = qSender.Values["USER"];
				}
				catch(Exception Ex)
				{
					if (LoggingEnabled == "1")
					{
						oLogger.Write("Error Occured: " + Ex.ToString());
						oLogger.EndWrite();
					}
					return;
				}

				if (pUser == null || pUser == "")
					pUser = WMS.Lib.USERS.SYSTEMUSER;

				try
				{

					if (Action == WMS.Lib.Actions.Audit.RELEASEWAVE)
					{
						WMS.Logic.Releasor rl = new WMS.Logic.Releasor();
						rl.Release(Wave,pUser,oLogger);
					}
					else if(Action == WMS.Lib.Actions.Audit.RELEASEPICKLIST)
					{
						WMS.Logic.Releasor rl = new WMS.Logic.Releasor();
						rl.ReleasePickList(picklist,pUser,oLogger);
					}
					if (oLogger != null)
					{
						oLogger.Write("Releasing Wave / PickList completed.");
						oLogger.EndWrite();
					}
				}
				catch (Made4Net.Shared.M4NException ex)
				{
					if (oLogger != null)
					{
						oLogger.Write("Error Occured while Releasing Wave/PickList... Error Details:");
						oLogger.Write(ex.Description);
						oLogger.EndWrite();
					}
				}
				catch (Exception ex)
				{
					if (oLogger != null)
					{
						oLogger.Write("Error Occured while Releasing Wave/PickList... Error Details:");
						oLogger.Write(ex.ToString());
						oLogger.EndWrite();
					}
				}
			}
			catch (Made4Net.Shared.M4NException ex)
			{
				if (oLogger != null)
				{
					oLogger.Write("Error Occured while Releasing Wave/PickList... Error Details:");
					oLogger.Write(ex.Description);
					oLogger.EndWrite();
				}
			}
			catch (Exception ex)
			{
				if (oLogger != null)
				{
					oLogger.Write("Error Occured while Releasing Wave/PickList... Error Details:");
					oLogger.Write(ex.ToString());
					oLogger.EndWrite();
				}
			}
		}
	}
}
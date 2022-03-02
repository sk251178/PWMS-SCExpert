using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WMS.Lib;
using WMS.Logic;
using Made4Net.DataAccess;
using Made4Net.Shared;

namespace WMS.OredersAutomation
{
    class OrdersAutomation : Made4Net.Shared.QHandler
    {
        private bool mUseLogs = false;
		private string mLogDir = "";
		private LogHandler oLogger = null;

		public OrdersAutomation():base("OrdersAutomation",false)
		{
           // mUseLogs = Convert.ToBoolean(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("UseLogs")));
            mUseLogs = Convert.ToBoolean(Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceSection  ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceUseLogs)));
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
            DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.OrdersAutomationServiceLogsDir;
           // mLogDir = System.Configuration.ConfigurationManager.AppSettings.Get("LogsDir");
            //RWMS-1891 Start
            mLogDir = DirPath;
            //RWMS-1891 End
        }

		protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
		{
            if (mUseLogs)
			{
                oLogger = new LogHandler(mLogDir, "OrdersAutomation_" + DateTime.Now.ToString("ddMMyy_hhmmss_") + new Random().Next() + ".txt");
				oLogger.StartWrite();
			}
            try
            {
                string sTemplateName = qSender.Values["TEMPLATENAME"];
                switch ((WMS.Logic.WMSEvents.EventType)Convert.ToInt32(qSender.Values["EVENT"]))
                {
                    case WMSEvents.EventType.AssignOutboundOrderToShipment:
                        if (mUseLogs)
                        {
                            oLogger.Write("Message for Assigning Orders to Shipment Received. Starting to process message...");
                            oLogger.Write(string.Format("Will try to assign orders to shipments according to template - {0}",sTemplateName));
                        }
                        //Initiate the Assign Object
                        OrdersAutomationShipmentAssignment SA = new OrdersAutomationShipmentAssignment(sTemplateName, oLogger);
                        SA.AssigneOrdersToShipment();
                        break;
                    case WMSEvents.EventType.AssignOutboundOrderToWave:
                        if (mUseLogs)
                        {
                            oLogger.Write("Message for Assigning Orders to Wave Received. Starting to process message...");
                            oLogger.Write(string.Format("Will try to assign orders to Wave according to template - {0}", sTemplateName));
                        }
                         //Initiate the Assign Object
                        OrdersAutomationWaveAssignment WA = new OrdersAutomationWaveAssignment(sTemplateName,oLogger);
                        WA.AssigneOrdersToWave();
                        break;
                }
            }
            catch (Exception ex)
            {
                if (mUseLogs)
                {
                    oLogger.Write("Exception Was thrown from the orders automation service.");
                    oLogger.Write("Error Details: " + ex.ToString());
                }
                throw; // added by Mohanraj to move the messsage to dead letter queue since it has exception.
            }
            finally
            {
                //End Logger Write
                if (mUseLogs)
                {
                    oLogger.Write("Message was proccessed successfully....");
                    oLogger.EndWrite();
                }
            }
		}
    }
}
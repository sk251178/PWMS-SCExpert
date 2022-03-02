using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;
using Made4Net.DataAccess;
using WMS.Logic;
using System.Data;
using Made4Net.Shared;

namespace WMS.Shipper
{
    public class Shipper : Made4Net.Shared.QHandler
    {
        #region Ctor

        public Shipper(): base("Shipper", false)
		{

        }

        #endregion

        #region Process

        protected override void ProcessQueue(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            string pUser = "";
            string shipmentId = "";
            string docId = "";
            string consignee = "";
            WMS.Logic.LogHandler oLogger = null;

            try
            {
                pUser = qSender.Values["USER"];
                if (pUser == null || pUser == "")
                    pUser = WMS.Lib.USERS.SYSTEMUSER;
               // string LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
                string LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceSection  ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceUseLogs);
                if (LoggingEnabled == "1")
                {
                  //  string dirpath = Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", null);
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ShipperServiceLogDirectory;
                    oLogger = new LogHandler(dirpath, "Shipper_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss_") + new Random().Next(1000) + ".txt");
                    oLogger.WriteTimeStamp = true;
                }
                switch ((WMS.Logic.WMSEvents.EventType)Convert.ToInt32(qSender.Values["EVENT"].ToString()))
                {
                    case WMS.Logic.WMSEvents.EventType.ShipmentShip:
                        shipmentId = qSender.Values["DOCUMENT"].ToString();
                        if (oLogger != null)
                            oLogger.Write(string.Format("Starting to ship shipment {0}...",shipmentId));
                        WMS.Logic.Shipment oShip = new WMS.Logic.Shipment(shipmentId, true);
                        oShip.ShipShipment(pUser, oLogger);
                        if (oLogger != null)
                            oLogger.Write(string.Format("Shipment Shipped successfully"));
                        break;
                        //oShip = null;
                    case WMS.Logic.WMSEvents.EventType.OrderShipped:
                        consignee = qSender.Values["CONSIGNEE"].ToString();
                        docId = qSender.Values["DOCUMENT"].ToString();
                        if (oLogger != null)
                            oLogger.Write(string.Format("Starting to ship outbound order; id: {0} for client: {1}...",docId,consignee));
                        WMS.Logic.OutboundOrderHeader oOrder = new WMS.Logic.OutboundOrderHeader(consignee,docId, true);
                        oOrder.ShipOrder(pUser, oLogger);
                        if (oLogger != null)
                            oLogger.Write(string.Format("Order Shipped successfully"));
                        break;
                    case WMS.Logic.WMSEvents.EventType.FlowThroughShipped:
                        consignee = qSender.Values["CONSIGNEE"].ToString();
                        docId = qSender.Values["DOCUMENT"].ToString();
                        if (oLogger != null)
                            oLogger.Write(string.Format("Starting to ship Flow Through; id: {0} for client: {1}...", docId, consignee));
                        WMS.Logic.Flowthrough oFlowThrough = new WMS.Logic.Flowthrough(consignee, docId, true);
                        oFlowThrough.ShipFlowThrough(pUser);
                        if (oLogger != null)
                            oLogger.Write(string.Format("Flow Through Shipped successfully"));
                        break;
                }
            }
            catch (Made4Net.Shared.M4NException ex)
            {
                if (oLogger != null)
                {
                    oLogger.Write("Error Occured while shipping document... Error Details:");
                    oLogger.Write(ex.Description);
                }
            }
            catch (Exception ex)
            {
                if (oLogger != null)
                {
                    oLogger.Write("Error Occured while shipping document... Error Details:");
                    oLogger.Write(ex.ToString());
                }
            }
            finally
            {
                if (oLogger != null)
                {
                    oLogger.EndWrite();
                    oLogger = null;
                }
            }
        }

        #endregion
    }
}
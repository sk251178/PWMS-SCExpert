using System;
using WMS.Logic;
using WMS.Lib;
using Made4Net.Shared;
using System.Messaging;
using System.Data;

namespace WMS.Replenishment
{
    /// <summary>
    /// Summary description for Replenishment.
    /// </summary>
    public class Replenishment:Made4Net.Shared.QHandler
    {

        public Replenishment():base("Replenishment",false)
        {
        }

        protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
         {
            try
            {
                int EventType = System.Convert.ToInt32(qSender.Values["EVENT"]);
                if (IsOrdinalReplenishment(EventType))
                    OrdinalReplanishmentMessageHandling(qMsg, qSender, EventType);
                else
                    ReplenishmentSpecialEventHandling(qMsg, qSender, EventType);
            }
            catch
            {}
        }

        private void ReplenishmentSpecialEventHandling(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, int EventType)
        {
            // After MSG received create new logger
            WMS.Logic.LogHandler oRepLogger = null;
            WMS.Logic.Location lc = null;
            WMS.Logic.Replenishment repl = new WMS.Logic.Replenishment();
            string LoggingEnabled;
            string location;
            string warehouseArea;
            string consignee;
            string sku;
          //  LoggingEnabled = Made4Net.Shared.AppConfig.Get("UseLogs", null);
            LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceUseLogs);
            // Checking if logging is enabled and creating new log file
            try
            {
                if (LoggingEnabled == "1")
                {
                  //  string dirpath = Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", null);
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceLogDirectory;
                    oRepLogger = new LogHandler(dirpath, "RP_" + EventType + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt");
                    oRepLogger.WriteTimeStamp = true;
                    oRepLogger.StartWrite();
                    oRepLogger.Write("Received  Replenishment Event");
                    // Try to set connection by one that arrived in the message
                    try
                    {
                        Made4Net.Shared.Warehouse.setCurrentWarehouse(qSender.Values["WAREHOUSE"]);
                        oRepLogger.Write("Processing message from [" + qSender.Values["WAREHOUSE"] + "] warehouse");
                        Made4Net.DataAccess.DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
                        oRepLogger.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                    }
                    catch
                    {}
                }

                switch (EventType)
                {
                    case (Int32)WMS.Logic.WMSEvents.EventType.LoadCount:
                        #region Load Count

                        location = qSender.Values["FROMLOC"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["FROMWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["FROMWAREHOUSEAREA"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("FROMLOC " + location);
                        oRepLogger.Write("FROMWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location,warehouseArea, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc,consignee,sku, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.LocationCount:
                        #region Location Count

                        location = qSender.Values["FROMLOC"];
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["FROMWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["FROMWAREHOUSEAREA"];
                        lc = new Location(location,warehouseArea, true);
                        string sSql = string.Format("select distinct consignee,sku from vPickloc where location = '{0}' and warehousearea='{1}'", location, warehouseArea);
                        DataTable dt = new DataTable();
                        Made4Net.DataAccess.DataInterface.FillDataset(sSql, dt, true, "");
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        foreach (DataRow dr in dt.Rows)
                        {
                            consignee = dr["consignee"].ToString();
                            if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                            {
                                continue;
                            }
                            sku = dr["sku"].ToString();
                            repl.Replenishment(lc, consignee, sku, oRepLogger);
                        }

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.LoadUnitsChanged:
                        #region Load units changes

                        location = qSender.Values["TOLOC"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("TOLOC " + location);
                        oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location,warehouseArea, true);
                        // RWMS-2315 : Auto cancellation when the sum of Allocated Qty > MAxReplenQty is not required, hence passing processCancellation parameter as false.
                        //repl.Replenishment(lc, consignee,sku, oRepLogger, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc, consignee, sku, oRepLogger, false);
                        // RWMS-2315 : Auto cancellation when the sum of Allocated Qty > MAxReplenQty is not required, hence passing processCancellation parameter as false.

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.LoadLocationChanged:
                        #region Load Move

                        location = qSender.Values["TOLOC"];
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        consignee = qSender.Values["CONSIGNEE"];
                        sku = qSender.Values["SKU"];
                        if (!WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            oRepLogger.Write("TOLOC " + location);
                            oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                            oRepLogger.Write("CONSIGNEE " + consignee);
                            oRepLogger.Write("SKU " + sku);
                            lc = new Location(location, warehouseArea, true);
                            repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                            repl.Replenishment(lc, consignee, sku, oRepLogger);
                        }
                        location = qSender.Values["FROMLOC"];
                        if (!WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            oRepLogger.Write("FROMLOC " + location);
                            oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                            oRepLogger.Write("CONSIGNEE " + consignee);
                            oRepLogger.Write("SKU " + sku);
                            lc = new Location(location, warehouseArea, true);
                            repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                            repl.Replenishment(lc, consignee, sku, oRepLogger);
                        }

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.PickLoad:
                        #region Pick

                        location = qSender.Values["TOLOC"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("TOLOC " + location);
                        oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location, warehouseArea, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc,consignee,sku, oRepLogger);

                        #endregion
                        break;

                    // PWMS-756 Added code changes for Event for Pick List Cancellation
                    case (Int32)WMS.Logic.WMSEvents.EventType.PickListUnAlloc:
                        #region Pick list Cancel/Unallocate
                        string picklistId = qSender.Values["DOCUMENT"];
                        oRepLogger.Write("Triggering picklist Cancellation");
                        oRepLogger.Write("PICKLIST =" + picklistId);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.ProcessPickListCancel(picklistId, oRepLogger, true);
                        #endregion
                        break;
                    // PWMS-756 Ended code changes for event Pick List Cancellation
                    case (Int32)WMS.Logic.WMSEvents.EventType.PickLocationCreated:
                        #region New Pick Location Created

                        location = qSender.Values["LOCATION"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = qSender.Values["WAREHOUSE"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("LOCATION " + location);
                        oRepLogger.Write("WAREHOUSE " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location, warehouseArea, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc, consignee, sku, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.PickLocationUpdated:
                        #region New Pick Location Updated
                        location = qSender.Values["LOCATION"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = qSender.Values["WAREHOUSE"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("LOCATION " + location);
                        oRepLogger.Write("WAREHOUSE " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location, warehouseArea, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc, consignee, sku, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.PickListAssign:
                        #region Pick List Assignment
                        //RWMS-2583 - Replen Phase 2 - Assignment of Pick tasks
                        oRepLogger.Write("Processing Picklist Assignment Event(187)");
                        oRepLogger.Write("Task =" + qSender.Values["TASK"] );
                        string TaskId = qSender.Values["TASK"];

                        PrioritizeReplenishments pr = new PrioritizeReplenishments();
                        pr.PickListAssignmentPrioritization(TaskId, oRepLogger);

                        ReplenishmentDueDate.ReEstimateDueDateOnPickListAssign(TaskId, oRepLogger);

                        #endregion
                        break;

                    case (Int32)WMS.Logic.WMSEvents.EventType.LoadPutaway:
                        #region Putaway

                        location = qSender.Values["TOLOC"];
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        consignee = qSender.Values["CONSIGNEE"];
                        sku = qSender.Values["SKU"];
                        if (!WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            oRepLogger.Write("TOLOC " + location);
                            oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                            oRepLogger.Write("CONSIGNEE " + consignee);
                            oRepLogger.Write("SKU " + sku);
                            lc = new Location(location, warehouseArea, true);
                            repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                            repl.Replenishment(lc, consignee, sku, oRepLogger, false);
                        }
                        location = qSender.Values["FROMLOC"];
                        if (!WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            oRepLogger.Write("FROMLOC " + location);
                            oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                            oRepLogger.Write("CONSIGNEE " + consignee);
                            oRepLogger.Write("SKU " + sku);
                            lc = new Location(location, warehouseArea, true);
                            repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                            repl.Replenishment(lc, consignee, sku, oRepLogger, false);
                        }

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.LoadReplenish:
                        #region Load Replenish

                        location = qSender.Values["FROMLOC"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["FROMWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["FROMWAREHOUSEAREA"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("FROMLOC " + location);
                        oRepLogger.Write("FROMWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        //RWMS-2743 Commented
                        //lc = new Location(location, warehouseArea, true);
                        //RWMS-2743 Commented END
                        //RWMS-2743 START
                        if (String.IsNullOrEmpty(warehouseArea))
                        {
                            lc = new Location(location);
                        }
                        else
                        {
                            lc = new Location(location, warehouseArea, true);
                        }
                        //RWMS-2743 END
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc,consignee,sku, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.OrderPalnned:
                        #region Order planned
                        string orderId = qSender.Values["DOCUMENT"];
                        consignee = qSender.Values["CONSIGNEE"];
                        oRepLogger.Write("ORDERID : " + orderId);
                        oRepLogger.Write("CONSIGNEE : " + consignee);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.OrderPlanner;
                        repl.EstimateDueDateOnCreateOrderId = orderId;
                        repl.Replenishment(consignee, orderId, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.WavePlaned:
                        #region Wave planned

                        string waveId = qSender.Values["DOCUMENT"];
                        oRepLogger.Write("WAVEID " + waveId);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(waveId, oRepLogger, false);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.WaveRelease:
                        #region Wave Released

                        string waverId = qSender.Values["DOCUMENT"];
                        oRepLogger.Write("WAVEID " + waverId);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(waverId, oRepLogger);

                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.PutawayOverried:
                        #region Putaway overried

                        location = qSender.Values["TOLOC"];
                        consignee = qSender.Values["CONSIGNEE"];
                        if (WMS.Logic.PickLoc.IsBatchPickLocation(consignee, location, oRepLogger))
                        {
                            break;
                        }
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("TOLOC " + location);
                        oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location,warehouseArea, true);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        repl.Replenishment(lc, consignee, sku, oRepLogger);

                        #endregion
                        break;

                    // Added code for Short Pick Event for PWMS-756
                    case (Int32)WMS.Logic.WMSEvents.EventType.SHORTPICK:
                        #region Short Pick
                        location = qSender.Values["FROMLOC"];
                        warehouseArea = string.IsNullOrEmpty(qSender.Values["TOWAREHOUSEAREA"]) ? qSender.Values["WAREHOUSE"] : qSender.Values["TOWAREHOUSEAREA"];
                        consignee = qSender.Values["CONSIGNEE"];
                        sku = qSender.Values["SKU"];
                        oRepLogger.Write("FROMLOC " + location);
                        oRepLogger.Write("TOWAREHOUSEAREA " + warehouseArea);
                        oRepLogger.Write("CONSIGNEE " + consignee);
                        oRepLogger.Write("SKU " + sku);
                        lc = new Location(location, warehouseArea, true);
                        //repl.Replenishment(lc, consignee, sku, oRepLogger);
                         string pcklistId = qSender.Values["DOCUMENT"];
                        //RWMS-2606 START - Included picklistline as DOCUMENTLINE
                        string pcklistline = qSender.Values["DOCUMENTLINE"];
                        //RWMS-2606 END
                        oRepLogger.Write("Triggering User Pick Short");
                        oRepLogger.Write("PICKLIST =" + pcklistId);
                        repl.EstimateDueDateOnCreateType = Logic.Replenishment.DueDateEstimateType.Normal;
                        //RWMS-2606 START
                        oRepLogger.Write("PICKLISTLINE =" + pcklistline);
                        //RWMS-2606 END
                        //RWMS-2606 - Commented
                        //repl.ProcessUserPickShort(pcklistId, oRepLogger, true);
                        //RWMS-2606 - Commented END
                        //RWMS-2606 - START
                        repl.ProcessUserPickShort(pcklistId, pcklistline, oRepLogger, true);
                        //RWMS-2606 - END
                        #endregion
                        break;
                    // Ended code for Short Pick Event for PWMS-756
                }
            }
            catch (Made4Net.Shared.M4NException ex)
            {
                if (LoggingEnabled == "1")
                {
                    oRepLogger.Write("Error While working on Replenishment Special Event...");
                    oRepLogger.Write(ex.GetErrMessage(0));
                }
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == "1")
                {
                    oRepLogger.Write("Error While working on Replenishment Special Event...");
                    oRepLogger.Write(ex.ToString());
                }
            }
        }

        private void OrdinalReplanishmentMessageHandling(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, int EventType)
        {
            string Location = qSender.Values["LOCATION"];
            string WarehouseArea = qSender.Values["WAREHOUSEAREA"];
            string PickRegion = qSender.Values["PICKREGION"];
            string All = qSender.Values["ALL"];
            string Sku = qSender.Values["SKU"];
            string Consignee = qSender.Values["CONSIGNEE"];
            decimal minQtyOverride = Convert.ToDecimal(qSender.Values["MINQTYOVERRIDE"]);
            decimal TaskTimeLimit = Convert.ToDecimal(qSender.Values["TASKSTIMELIMIT"]);

            //Added for RWMS-1867 Start
            string logInUserId = "";
            if (EventType == 77)
            {
                logInUserId = qSender.Values["LOGINUSERID"];

            }
            //Added for RWMS-1867 End
            try
            {
                // After MSG received create new logger
                WMS.Logic.LogHandler oRepLogger = null;
                String LoggingEnabledStr;
                bool LoggingEnabled = false;
                string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                //RWMS-1887 Start Comment
                //dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceLogDirectory;
                //RWMS-1887 End Comment


                LoggingEnabledStr = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceSection,
                                                  Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceUseLogs);

                if (LoggingEnabledStr == "1")
                    LoggingEnabled = true;

                // Checking if logging is enabled and creating new log file
                if (LoggingEnabled)
                {
                  //  string dirpath = Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", null);
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceLogDirectory;
                    oRepLogger = new LogHandler(dirpath, "RP_" + EventType + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt");
                    oRepLogger.WriteTimeStamp = true;
                    oRepLogger.StartWrite();
                    oRepLogger.Write("Received Ordinal Replenishment Event");
                    // Try to set connection by one that arrived in the message
                    try
                    {
                        Made4Net.Shared.Warehouse.setCurrentWarehouse(qSender.Values["WAREHOUSE"]);
                        oRepLogger.Write("Processing message from [" + qSender.Values["WAREHOUSE"] + "] warehouse");
                        Made4Net.DataAccess.DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
                        oRepLogger.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                    }
                    catch
                    { }
                    oRepLogger.Write("Got Repl task. Task details:");
                    oRepLogger.Write("Location: " + Location);
                    oRepLogger.Write("Warehousearea: " + WarehouseArea);
                    oRepLogger.Write("PickRegion: " + PickRegion);
                    oRepLogger.Write("Consignee: " + Consignee);
                    oRepLogger.Write("Sku: " + Sku);
                    oRepLogger.Write("Minimum qty override factor: " + minQtyOverride);
                    oRepLogger.Write("Time Limit for task: " + TaskTimeLimit);
                    oRepLogger.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                }

                switch (EventType)
                {
                    case (Int32)WMS.Logic.WMSEvents.EventType.NormalReplenishment:
                        #region Normal Replenishment
                        // If ALL = 1 then replenishment for all location's
                        // If ALL = 0 then replenishment only for the selected locations
                        try
                        {
                            string sql;
                            if (minQtyOverride > 0)
                            {
                                if (LoggingEnabled)
                                    oRepLogger.Write("Minimum qty received. updating new minimum quantity parameter.");
                                WMS.Logic.WarehouseParams.Edit("NormalReplMinFactor", null, minQtyOverride.ToString(), WMS.Lib.USERS.SYSTEMUSER);
                                //sql = string.Format("update warehouseparams set paramvalue = '{0}' where paramname = 'NormalReplMinFactor'", minQtyOverride);
                                //Made4Net.DataAccess.DataInterface.RunSQL(sql);
                            }
                            if (WMS.Logic.PickLoc.IsBatchPickLocation(Consignee, Location, oRepLogger))
                            {
                                break;
                            }
                            WMS.Logic.Replenishment repl = new WMS.Logic.Replenishment();
                            //Added for RWMS-1840
                            string ReplPolicyMethod = WMS.Logic.Replenishment.GetPolicyID();
                          //Ended for RWMS-1840

                            if (All == "0")
                                repl.LocReplenish(Location, WarehouseArea, Consignee, Sku, ReplPolicyMethod, WMS.Lib.USERS.SYSTEMUSER, oRepLogger, 0);
                            else
                                repl.NormalReplenish(Location, WarehouseArea, PickRegion, Sku, oRepLogger,0);

                            if (minQtyOverride > 0)
                            {
                                sql = string.Format("select paramvalue from warehouseparams where paramname = 'DelReplMinFactor'");
                                if (Convert.ToBoolean(Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))))
                                {
                                    WMS.Logic.WarehouseParams.Edit("NormalReplMinFactor", null, (-1).ToString(), WMS.Lib.USERS.SYSTEMUSER);
                                    //sql = string.Format("update warehouseparams set paramvalue = '{0}' where paramname = 'NormalReplMinFactor'", -1);
                                    //Made4Net.DataAccess.DataInterface.RunSQL(sql);
                                }
                            }
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.Replenishment:
                        #region  Replenishment
                        try
                        {
                            string waveId = qSender.Values["WAVE"];
                            WMS.Logic.Replenishment repl = new WMS.Logic.Replenishment();
                            repl.Replenishment(waveId, oRepLogger);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.ZoneReplenishment:
                        #region Zone Replenishment
                        try
                        {
                            WMS.Logic.ZoneReplenish zonerepl = new WMS.Logic.ZoneReplenish();
                            //Added for RWMS-1840
                             string ReplPolicyMethod = WMS.Logic.Replenishment.GetPolicyID();
                             //Ended for RWMS-1840
                             zonerepl.ZoneReplenishment(PickRegion, Sku, ReplPolicyMethod, "SYSTEM", oRepLogger, null, false);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Zone Replenishment Tasks:");
                                oRepLogger.Write("Error Description: " + ex.Description);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.OpportunityReplenishment:
                        #region Opportunity Replenishment
                        try
                        {
                            string sLoc = null;
                            string sWarehouseArea = null;
                            string LoadId = qSender.Values["LOADID"];
                            bool OnConatiner = Convert.ToBoolean(qSender.Values["ONCONTAINER"]);
                            WMS.Logic.Replenishment repl = new WMS.Logic.Replenishment();
                            repl.OpportunityReplenish(ref sLoc,ref sWarehouseArea, LoadId, OnConatiner, "SYSTEM", oRepLogger);
                            if (qMsg.ResponseQueue != null)
                            {
                                MessageQueue newmq = qMsg.ResponseQueue;
                                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                                qs.Add("LOCATION", sLoc);
                                qs.Add("WAREHOUSEAREA", sWarehouseArea);
                                qs.Send(newmq, LoadId, qMsg.Id, System.Messaging.MessagePriority.Normal);
                            }
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks for Putaway...");
                                oRepLogger.Write(ex.Description);
                                if (qMsg.ResponseQueue != null)
                                {
                                    MessageQueue newmq = qMsg.ResponseQueue;
                                    Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                                    qs.Add("LOCATION", "");
                                    qs.Add("WAREHOUSEAREA", "");
                                    qs.Send(newmq, qSender.Values["LOADID"], qMsg.Id, System.Messaging.MessagePriority.Normal);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks for Putaway...");
                                oRepLogger.Write(ex.ToString());
                                if (qMsg.ResponseQueue != null)
                                {
                                    MessageQueue newmq = qMsg.ResponseQueue;
                                    Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                                    qs.Add("LOCATION", "");
                                    qs.Add("WAREHOUSEAREA", "");
                                    qs.Send(newmq, qSender.Values["LOADID"], qMsg.Id, System.Messaging.MessagePriority.Normal);
                                }
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.ManualReplenishment:
                        #region Manual Replenishment
                        try
                        {
                            if (Location != string.Empty)
                            {
                                if (WMS.Logic.PickLoc.IsBatchPickLocation(Consignee, Location, oRepLogger))
                                {
                                    break;
                                }
                                WMS.Logic.Replenishment repl = new WMS.Logic.Replenishment();
                                string taskID = string.Empty;
                                string replID = string.Empty;
                                if (repl.ReplenishmentExists(Sku, Location, Consignee, WarehouseArea, ref taskID, ref replID, oRepLogger))
                                {
                                    Task replTask = new WMS.Logic.Task(taskID);
                                    replTask.AssignUserManualReplenish(logInUserId);
                                }
                                else
                                {
                                    repl.LocReplenish(Location, WarehouseArea, Consignee, Sku, WMS.Logic.Replenishment.ReplenishmentMethods.ManualReplenishment, "SYSTEM", oRepLogger, 0, 0, false, logInUserId);
                                }
                            }
                            else if (PickRegion != string.Empty && Sku != string.Empty)
                            {
                                WMS.Logic.ZoneReplenish zonerepl = new WMS.Logic.ZoneReplenish();
                                zonerepl.ZoneReplenishment(PickRegion, Sku, WMS.Logic.Replenishment.ReplenishmentMethods.ManualReplenishment, "SYSTEM", oRepLogger, null, false);
                            }
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled)
                            {
                                oRepLogger.Write("Error While creating Replenishment Tasks...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                }

                if (LoggingEnabled)
                {
                    oRepLogger.Write(EventType + " Task Finished...");
                    oRepLogger.EndWrite();
                }
            }
            catch
            {
            }
        }

        #region Utilities methods

        private bool IsOrdinalReplenishment(int EventType )
        {
            if (EventType == 70 || EventType == 71 || EventType == 72 || EventType == 75 || EventType == 77)
                return true;
            else
                return false;
        }

        #endregion
    }
}
using System;
using WMS.Logic;
using WMS.Lib;
using Made4Net.Shared;
using System.Messaging;
using System.Data;

namespace WMS.BatchReplenishment
{
    /// <summary>
    /// Summary description for BatchReplenishment.
    /// </summary>
    public class BatchReplenishment : Made4Net.Shared.QHandler
    {
        public BatchReplenishment() : base("BatchReplenishment", false)
        {
        }
        protected override void ProcessQueue(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
                int EventType = System.Convert.ToInt32(qSender.Values["EVENT"]);
                BatchReplenishmentSpecialEventHandling(qMsg, qSender, EventType);
            }
            catch { }
        }

        private void BatchReplenishmentSpecialEventHandling(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, int EventType)
        {
            // After MSG received create new logger
            WMS.Logic.LogHandler oRepLogger = null;
            string LoggingEnabled;
            LoggingEnabled = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceUseLogs);
            // Checking if logging is enabled and creating new log file
            try
            {
                if (LoggingEnabled == "1")
                {
                    //  string dirpath = Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", null);
                    string dirpath = Made4Net.DataAccess.Util.GetInstancePath();
                    dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.ReplanishmnetServiceLogDirectory;
                    oRepLogger = new LogHandler(dirpath, "BRP_" + EventType + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt");
                    oRepLogger.WriteTimeStamp = true;
                    oRepLogger.StartWrite();
                    oRepLogger.Write("Received Batch Replenishment Event");
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
                }

                switch (EventType)
                {
                    case (Int32)WMS.Logic.WMSEvents.EventType.BReplenScheduled:
                        #region Batch Replen Scheduled
                        try
                        {
                            BatchReplenHeader batchreplheader = new BatchReplenHeader();
                            batchreplheader.PICKREGION = qSender.Values["PICKREGION"];
                            batchreplheader.WAREHOUSEAREA = qSender.Values["TOWAREHOUSEAREA"];
                            batchreplheader.CONSIGNEE = qSender.Values["CONSIGNEE"];
                            batchreplheader.REPLENPOLICY = qSender.Values["REPLPOLICY"];
                            batchreplheader.TARGETLOCATION = BatchReplenHeader.GetTargetLocation(batchreplheader.PICKREGION);
                            oRepLogger.Write("PICKREGION " + batchreplheader.PICKREGION);
                            oRepLogger.Write("TOWAREHOUSEAREA " + batchreplheader.WAREHOUSEAREA);
                            oRepLogger.Write("CONSIGNEE " + batchreplheader.CONSIGNEE);
                            oRepLogger.Write("REPLPOLICY " + batchreplheader.REPLENPOLICY);
                            oRepLogger.Write("TARGETLOCATION " + batchreplheader.TARGETLOCATION);

                            WMS.Logic.BatchReplenPlanner breplplanner = new WMS.Logic.BatchReplenPlanner(batchreplheader);
                            breplplanner.Plan(qSender.Values["USERID"], oRepLogger);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Planning Batch Replenishment...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Planning Batch Replenishment...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.BReplenLineUnload:
                        #region Batch Replen Detail Line Unload
                        try
                        {
                            String batchReplId = qSender.Values["BATCHREPLID"];
                            if (batchReplId == null)
                            {
                                break;
                            }
                            decimal unloadQty = Decimal.Parse(qSender.Values["UNLOADQTY"]);
                            WMS.Logic.BatchReplenDetail brd = new WMS.Logic.BatchReplenDetail(batchReplId, Int32.Parse(qSender.Values["REPLLINE"]));
                            brd.UpdateUnloadQuantityAndStatus(unloadQty, WMS.Lib.Statuses.BatchReplenDetail.UNLOAD, qSender.Values["USER"]);
                            WMS.Logic.Load load = new WMS.Logic.Load(brd.TOLOAD);
                            load.LOCATION = brd.TOLOCATION;
                            load.STATUS = WMS.Lib.Statuses.LoadStatus.AVAILABLE;
                            SKU sku = new SKU(brd.CONSIGNEE, brd.TOSKU);
                            decimal newldQuantity = sku.ConvertParentLowestUomUnitsToMyLowestUomUnits(unloadQty);
                            load.SKU = brd.TOSKU;
                            load.LOADUOM = brd.TOSKUUOM;
                            load.UNITS = newldQuantity;
                            load.UpdateLocationAndStatus();
                            Load sourceLoadObj = WMS.Logic.Merge.Put(load, brd.TOLOCATION, brd.TOWAREHOUSEAREA);
                            if (!string.IsNullOrEmpty(sku.PARENTSKU))
                            {
                                Load fromLoad = new Load(brd.FROMLOAD);
                                fromLoad.GenerateInvAdj(WMS.Lib.INVENTORY.SUBQTY, fromLoad.UNITS + unloadQty, fromLoad.UNITS, WMS.Lib.Statuses.BatchReplenDetail.UNLOAD, qSender.Values["USER"]);
                                if (sourceLoadObj is null)
                                {
                                    load.GenerateInvAdj(WMS.Lib.INVENTORY.ADDQTY, 0, newldQuantity, WMS.Lib.Statuses.BatchReplenDetail.UNLOAD, qSender.Values["USER"]);
                                }
                                else
                                {
                                    sourceLoadObj.GenerateInvAdj(WMS.Lib.INVENTORY.ADDQTY, sourceLoadObj.UNITS - load.UNITS, sourceLoadObj.UNITS, WMS.Lib.Statuses.BatchReplenDetail.UNLOAD, qSender.Values["USER"]);
                                }
                            }
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Unloading Replenishment Detail...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Unloading Replenishment Detail...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.BReplenLineLetdown:
                        #region Batch Replen Detail Line Letdown
                        try
                        {
                            String batchReplId = qSender.Values["BATCHREPLID"];
                            if (batchReplId == null) {
                                break;
                            }
                            WMS.Logic.BatchReplenDetail brd = new WMS.Logic.BatchReplenDetail(batchReplId, Int32.Parse(qSender.Values["REPLLINE"]));
                            string toLoad = Load.GenerateLoadId();
                            brd.TOLOAD = toLoad;
                            brd.LETDOWNQTY = Decimal.Parse(qSender.Values["LETDOWNQTY"]);
                            brd.STATUS = WMS.Lib.Statuses.BatchReplenDetail.LETDOWN;
                            brd.UpdateLetdownDetails(qSender.Values["USER"]);
                            Load fromLoad = new Load(brd.FROMLOAD);
                            fromLoad.unAllocate(brd.FROMQTY, qSender.Values["USER"]);
                            //SKU sku = new SKU(brd.CONSIGNEE, brd.TOSKU);
                            //decimal newldQuantity = sku.ConvertParentLowestUomUnitsToMyLowestUomUnits(brd.LETDOWNQTY);
                            fromLoad.AdjustLoadQty(WMS.Lib.INVENTORY.SUBQTY, brd.LETDOWNQTY, qSender.Values["USER"]);
                            Load newLoad = new Load();
                            newLoad.CreateLoad(toLoad, brd.CONSIGNEE, brd.FROMSKU , fromLoad.LOADUOM , brd.FROMLOCATION, brd.TOWAREHOUSEAREA, WMS.Lib.Statuses.LoadStatus.AVAILABLE, String.Empty, brd.LETDOWNQTY, null, 0, string.Empty, null, qSender.Values["USER"], oRepLogger);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Letting down Replenishment Detail...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Letting donw Replenishment Detail...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;

                    case (Int32)WMS.Logic.WMSEvents.EventType.BReplenLetdown:
                        #region Batch Replen Header LetDown
                        try
                        {
                            String batchReplId = qSender.Values["BATCHREPLID"];
                            if (batchReplId == null)
                            {
                                break;
                            }
                            WMS.Logic.BatchReplenHeader brh = new WMS.Logic.BatchReplenHeader(batchReplId);
                            brh.UpdateToLoadTargetLocationofBatchReplen();
                            TaskManager tm = new TaskManager();
                            tm.CreateBatchReplenUnloadTask(brh);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Letting down Replenishment...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Letting down Replenishment...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                    case (Int32)WMS.Logic.WMSEvents.EventType.BReplenUnload:
                        #region Batch Replen Header Unload
                        try
                        {
                            String batchReplId = qSender.Values["BATCHREPLID"];
                            if (batchReplId == null)
                            {
                                break;
                            }
                            WMS.Logic.BatchReplenHeader brh = new WMS.Logic.BatchReplenHeader(batchReplId);
                            brh.UpdateStatus(WMS.Lib.Statuses.BatchReplenHeader.UNLOAD, qSender.Values["USER"]);
                        }
                        catch (Made4Net.Shared.M4NException ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Unloading Replenishment...");
                                oRepLogger.Write(ex.GetErrMessage(0));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoggingEnabled == "1")
                            {
                                oRepLogger.Write("Error While Unloading Replenishment...");
                                oRepLogger.Write(ex.ToString());
                            }
                        }
                        #endregion
                        break;
                }
            }
            catch (Made4Net.Shared.M4NException ex)
            {
                if (LoggingEnabled == "1")
                {
                    oRepLogger.Write("Error While working on Batch Replenishment Special Event...");
                    oRepLogger.Write(ex.GetErrMessage(0));
                }
            }
            catch (Exception ex)
            {
                if (LoggingEnabled == "1")
                {
                    oRepLogger.Write("Error While working on Batch Replenishment Special Event...");
                    oRepLogger.Write(ex.ToString());
                }
            }
         
        }

    }
}
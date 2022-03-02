using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Made4Net.DataAccess;
using Made4Net.Shared;
using System.IO;

namespace WMS.SchedulerTasks
{
    public class ExpirationDate
    {
        public ExpirationDate()
        {

        }

        //Commented code for Retrofit task PWMS-477,RWMS-568 Start

        //public void UpdateData()
        //{
        //    Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile("F:\\SCExpert\\Logs\\Scheduler\\", "D_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
        //    _qhlogger.WriteLine("Starting to proccess message", true);
        //    try
        //    {
        //        string SQLAllLot = "SELECT * FROM vLOT WHERE EXPIRATIONDAYS IS NOT NULL";
        //        DataTable dtLots = new DataTable();
        //        DataInterface.FillDataset(SQLAllLot, dtLots, false, "");
        //        _qhlogger.WriteLine(SQLAllLot, true);
        //        foreach (DataRow drLot in dtLots.Rows)
        //        {
        //            // Select All Loads from this CONSIGNEE, SKU, LOT
        //            string SQLLotsLoads = "SELECT * FROM LOADATTRIBUTES WHERE (STATUS<>'RINV' AND STATUS<>'ROUT') AND EXPIRYDATE IS NOT NULL AND CONSIGNEE='002' AND SKU='" + Convert.ToString(drLot["SKU"]) + "' AND LOT='" + Convert.ToString(drLot["LOT"]) + "'";
        //            DataTable dtLoads = new DataTable();
        //            DataInterface.FillDataset(SQLLotsLoads, dtLoads, false, "");
        //            _qhlogger.WriteLine(SQLLotsLoads, true);
        //            foreach (DataRow drLoad in dtLoads.Rows)
        //            {
        //                //_qhlogger.WriteLine("Calculated expiry date : " + Convert.ToString(Convert.ToDateTime(drLoad["EXPIRYDATE"]).Date.AddDays(Convert.ToInt32(drLot["EXPIRATIONDAYS"])).Date) , true);
        //                //_qhlogger.WriteLine(Convert.ToString(DateTime.Now.Date.Subtract(Convert.ToDateTime(drLoad["EXPIRYDATE"]).Date.AddDays(Convert.ToInt32(drLot["EXPIRATIONDAYS"])).Date)), true);
        //                TimeSpan ts = DateTime.Now.Date.Subtract(Convert.ToDateTime(drLoad["EXPIRYDATE"]).Date.AddDays(Convert.ToInt32(drLot["EXPIRATIONDAYS"])).Date);
        //                //_qhlogger.WriteLine(Convert.ToString ( ts.Days ), true);
        //                if (Convert.ToInt64(ts.Days) >= 0)
        //                {
        //                    // Update Load Status to EXPIRED
        //                    DataInterface.RunSQL("UPDATE LOADS SET STATUS='EXPIRED', HOLDRC='" + Convert.ToInt64(ts.Days) + "' WHERE LOADID='" + Convert.ToString(drLoad["LOADID"]) + "'");
        //                    // Send Message to Event Manager
        //                    _qhlogger.WriteLine("Load ID" + Convert.ToString(drLoad["LOADID"]) + " updated", true);

        //                    Made4Net.Shared.QMsgSender oQMsg = new Made4Net.Shared.QMsgSender();

        //                    oQMsg.Add("EVENT", "30".ToString());
        //                    oQMsg.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
        //                    oQMsg.Add("ACTIVITYTIME", "0");
        //                    oQMsg.Add("ACTIVITYTYPE", "SETSTATUS");
        //                    oQMsg.Add("CONSIGNEE", Convert.ToString(drLoad["CONSIGNEE"]));
        //                    oQMsg.Add("DOCUMENT", Convert.ToString(drLoad["RECEIPT"]));
        //                    oQMsg.Add("DOCUMENTLINE", Convert.ToString(drLoad["RECEIPTLINE"]));
        //                    oQMsg.Add("FROMLOAD", Convert.ToString(drLoad["LOADID"]));
        //                    oQMsg.Add("FROMLOC", Convert.ToString(drLoad["LOCATION"]));
        //                    oQMsg.Add("FROMQTY", Convert.ToString(drLoad["UNITS"]));
        //                    oQMsg.Add("FROMSTATUS", Convert.ToString(drLoad["STATUS"]));
        //                    oQMsg.Add("NOTES", "");
        //                    oQMsg.Add("SKU", Convert.ToString(drLot["SKU"]));
        //                    oQMsg.Add("TOLOAD", Convert.ToString(drLoad["LOADID"]));
        //                    oQMsg.Add("TOLOC", Convert.ToString(drLoad["LOCATION"]));
        //                    oQMsg.Add("TOQTY", Convert.ToString(drLoad["UNITS"]));
        //                    oQMsg.Add("TOSTATUS", "EXPIRED");
        //                    oQMsg.Add("USERID", "INTERFACE");
        //                    oQMsg.Add("ADDDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
        //                    oQMsg.Add("LASTMOVEUSER", "INTERFACE");
        //                    oQMsg.Add("LASTSTATUSUSER", "INTERFACE");
        //                    oQMsg.Add("LASTCOUNTUSER", "INTERFACE");
        //                    oQMsg.Add("LASTSTATUSDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
        //                    oQMsg.Add("ADDUSER", "INTERFACE");
        //                    oQMsg.Add("EDITDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
        //                    oQMsg.Add("EDITUSER", "INTERFACE");
        //                    oQMsg.Send("EventManager", "30", "", System.Messaging.MessagePriority.Normal);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _qhlogger.WriteLine(ex.Message, true);
        //    }
        //}

        //Commented code Retrofit task for PWMS-477,RWMS-568 End

        //Added for Retrofit Task PWMS-477,RWMS-568 Start

        public void UpdateData()
        {
            //Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile("F:\\SCExpert\\Logs\\Scheduler\\", "D_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
            //_qhlogger.WriteLine("Starting to proccess message", true);
            //RWMS-2456
            string appLogDir = Made4Net.Shared.Util.GetSystemParameterValue("ApplicationLogDirectory").ToString();
            appLogDir = Made4Net.DataAccess.Util.BuildAndGetFilePath(appLogDir);
            StreamWriter sw = new StreamWriter(appLogDir + new Random().Next().ToString() + ".txt");
            sw.WriteLine("Entered Update Data");
            WMS.Logic.Warehouse.setCurrentWarehouse(Warehouse.CurrentWarehouse);
            try
            {
                string SQLAllLot = "SELECT * FROM vExpiredLoads";
                sw.WriteLine(SQLAllLot);
                DataTable dtLots = new DataTable();
                DataInterface.FillDataset(SQLAllLot, dtLots, false, "");
                //_qhlogger.WriteLine(SQLAllLot, true);
                sw.WriteLine("found results:" + dtLots.Rows.Count.ToString());
                foreach (DataRow drLoad in dtLots.Rows)
                {
                    WMS.Logic.Load ld = new WMS.Logic.Load(Convert.ToString(drLoad["LOADID"]), true);
                    //System.Diagnostics.Debugger.Break();
                    ld.setStatus(Convert.ToString(drLoad["NEWSTATUS"]), "EXP", "SYSTEM");
                    //// Create non specific pickup task if load in picking location
                    //if (Convert.ToInt32(DataInterface.ExecuteScalar("SELECT COUNT(1) FROM PICKLOC WHERE LOCATION='" + ld.LOCATION + "'")) > 0)
                    //{
                    //    DMLS.Logic.Receiving.CreateNonSpecificPickupTask(ld.LOCATION, 1);
                    //}
                }
            }
            catch (Exception ex)
            {
                //_qhlogger.WriteLine(ex.Message, true);
                sw.WriteLine(ex.ToString());
            }
            finally
            {
                sw.Close();
            }

        }
        //Added for Retrofit Task PWMS-477,RWMS-568 End

    }
}
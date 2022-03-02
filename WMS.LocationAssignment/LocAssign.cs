using System;
using System.Data;
using System.Messaging;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;
using System.Collections.Generic;
using System.Text;
using Made4Net.Algorithms.Interfaces;
using Made4Net.Algorithms;
using DataManager.ServiceModel;
using System.Linq;

namespace WMS.LocationAssignment
{
    /// <summary>
    /// Summary description for LocAssign.
    /// </summary>
    public class LocAssign : Made4Net.Shared.QHandler
    {
        private bool useLogs = true;
        private WMS.Logic.LogHandler lg = null;
        // Instantiate so that a single instance is available through out the process
        private IShortestPathProvider _sp = ShortestPath.GetInstance();

        public LocAssign()
            : base("LocAssign", false)
        {
        }

        protected override void ProcessQueue(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            try
            {
               // useLogs = System.Convert.ToBoolean(System.Convert.ToInt32(Made4Net.Shared.AppConfig.Get("CreateLogs", "0")));
                useLogs = System.Convert.ToBoolean(System.Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.LocationAssignmentServiceSection ,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.LocationAssignmentServiceUseLogs)));
                if (useLogs)
                {
                    string DirPath = Made4Net.DataAccess.Util.GetInstancePath();
                    DirPath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.LocationAssignmentServiceLogDirectory;
                    //lg = new Logic.LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory", ""), new Random().Next() + ".txt");
                    lg = new Logic.LogHandler(DirPath, new Random().Next() + ".txt");
                    lg.StartWrite();
                }
                string Action = qSender.Values["ACTION"];
                // Try to set connection by one that arrived in the message
                try
                {
                    Made4Net.Shared.Warehouse.setCurrentWarehouse(qSender.Values["WAREHOUSE"]);
                    lg.Write("Processing message from [" + qSender.Values["WAREHOUSE"] + "] warehouse");
                    DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection;
                    lg.Write("Connection selected [" + Made4Net.Shared.Warehouse.WarehouseConnection + "]");
                }
                catch { }

                switch (Action)
                {
                    case (WMS.Lib.Actions.Services.REQUESTLOCATIONBYLOC):
                        if (useLogs)
                            lg.Write("Start getting new location by location.");
                        getLocByLocation(qMsg, qSender);
                        break;
                    case (WMS.Lib.Actions.Services.REQUESTLOCATIONBYREGION):
                        if (useLogs)
                            lg.Write("Start getting new location by Region.");
                        getLocByRegion(qMsg, qSender);
                        break;
                    case (WMS.Lib.Actions.Services.REQUESTLOCATIONFORLOAD):
                        if (useLogs)
                            lg.Write("Start getting new location for load.");
                        FindLoadDestinationLocation(qMsg, qSender);
                        break;
                    case (WMS.Lib.Actions.Services.REQUESTLOCATIONFORCONTAINER):
                        if (useLogs)
                            lg.Write("Start getting new location for container.");
                        FindContainerDestinationLocation(qMsg, qSender);
                        break;
                    case (WMS.Lib.Actions.Services.REQUESTLOCATIONFORMULTILOAD):
                        if (useLogs)
                            lg.Write("Start getting new location for multiple loads.");
                        FindMultipleLoadDestinationLocation(qMsg, qSender);
                        break;
                }
            }
            catch (Made4Net.Shared.M4NException ex)
            {
                if (useLogs)
                    lg.Write("Error Occurred: " + ex.GetErrMessage(0));
            }
            catch (Exception ex)
            {
                if (useLogs)
                    lg.Write("Error Occurred: " + ex.ToString());
            }
            finally
            {
                if (useLogs)
                    lg.EndWrite();
            }
        }

        #region Methods

        #region License Managment

        private void AcquireLicence()
        {
            try
            {
                if (useLogs)
                {
                    lg.Write("Connecting to Licence Serviece...");
                }
            }
            catch { }

            string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
            string appId = Made4Net.Shared.AppConfig.Get("LicenseAppId", null);
            try
            {
                bool isConnected = true; // Made4Net.DataAccess.DataInterface.Connect(licUser);
                if (isConnected)
                {
                    if (useLogs)
                    {
                        lg.Write("Licence Acquired...");
                    }
                }
                else
                {
                    if (useLogs)
                    {
                        lg.Write("No Licence Acquired...");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ReleaseLicence()
        {
            string licUser = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
            try
            {
                //Made4Net.DataAccess.DataInterface.Disconnect(licUser);
                if (useLogs)
                {
                    lg.Write("Licence released for future use");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion

        #region Location Search for Load Putaway

        #region Multi Pallet Putawy

        /// <summary>
        /// Does a putaway as a single putaway while handling a multiple payload puaway request
        /// </summary>
        /// <param name="qMsg"></param>
        /// <param name="qSender"></param>
        private void FindMultipleLoadDestinationLocationIndividual(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string LoadIDs = "";
            List<LoadsWithSequence> loadsWithSequence = new List<LoadsWithSequence>();
            string User = "";
            String[] loadIdsSplited;
            if (useLogs)
            {
                lg.Write("Single putaway execution started in Multi Payload");
            }
            // Input extraction and validation.
            try
            {
                User = qSender.Values["USERID"];
            }
            catch
            {
            }
            try
            {
                LoadIDs = qSender.Values["LOADID"];
                if (useLogs)
                {
                    lg.Write("Got the Load Ids from the QSender: " + LoadIDs);
                }

                if (LoadIDs == null || LoadIDs == "")
                {
                    ResponseOnErrorForLoads(LoadIDs, 2, qMsg);
                    return;
                }
                loadIdsSplited = Array.ConvertAll(LoadIDs.Split(','), p => p.Trim());
                int sequence = 1;
                foreach (String loadid in loadIdsSplited)
                {
                    LoadsWithSequence lds = new LoadsWithSequence();
                    WMS.Logic.Load ld = new WMS.Logic.Load(loadid, true);
                    lds.Load = ld;
                    lds.SequenceNo = sequence;
                    sequence++;
                    loadsWithSequence.Add(lds);
                }
            }
            catch (M4NException m4nex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load one of the Load Object.");
                    lg.Write(m4nex.Description);
                }
                ResponseOnErrorForLoads(LoadIDs, 1, qMsg);
                return;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load one of the Load Object.");
                    lg.Write(ex.StackTrace);
                }
                ResponseOnErrorForLoads(LoadIDs, 1, qMsg);
                return;
            }

            try
            {
                String[] result = new string[loadsWithSequence.Count];
                int count = 0;
                string WarehouseareaID = "";
                string prePopulateLocation = "";  //RWMS-1277
                List<LoadsWithSequence> assignedLoads = new List<LoadsWithSequence>();
                foreach (LoadsWithSequence lds in loadsWithSequence)
                {
                    // Start Modified For pallaet height in for single load
                    String foundLocation = FindLoadDestinationLocationForMultiPayloadPutawaOneAtATime(lds, User, ref WarehouseareaID, assignedLoads, ref prePopulateLocation); //RWMS-1277
                    result[count] = lds.Load.LOADID + "=" + foundLocation;
                    lds.Location = foundLocation;
                    assignedLoads.Add(lds);
                    // End Modified For pallaet height in for single load
                    count++;
                }

                // Done.
                // Response for locationID should be in this format:
                // G000000005=4622A1,G000000010=4622A1,G000000004=4622A1
                // Convert here
                if (useLogs)
                {
                    lg.Write("Returning Destination Location :" + result);
                }
                ResponseOnSuccessMulti(loadsWithSequence[0].Load.LOADID, String.Join(",", result), WarehouseareaID, qMsg, prePopulateLocation);//RWMS-1277
                return;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not do multi payload putaway as single putaway.");
                    lg.Write(ex.StackTrace);
                }
                ResponseOnErrorForLoads(loadsWithSequence[0].Load.LOADID, 1, qMsg);
                return;
            }
        }

        private string FindLoadDestinationLocationForMultiPayloadPutawaOneAtATime(LoadsWithSequence lds, String User, ref string WarehouseareaID, List<LoadsWithSequence> assignedLoads, ref string prePopulateLocation)//RWMS-1277
        {
            string LocationID = "";
            string Content = "";
            decimal QtyToPlace = -1;
            WMS.Logic.Load ld = null;
            WMS.Logic.SKU sk = null;
            double operatorsEqpHeight = 0;
            try
            {
                ld = lds.Load;
            }
            catch (M4NException m4nex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Load Object.");
                    lg.Write(m4nex.Description);
                }
                // No Error, place it in same location
                return ld.LOCATION;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Load Object.");
                    lg.Write(ex.StackTrace);
                }
                // No Error, place it in same location
                return ld.LOCATION;
            }
            //fetch operators equipment height
            operatorsEqpHeight = GetOperatorsEquipmentHeight(User);
            //fetch operators equipment height

            try
            {
                sk = new SKU(ld.CONSIGNEE, ld.SKU, true);
                DataTable dtConsStrategy = new DataTable();
                DataTable DTLoadParams = new DataTable();
                Made4Net.DataAccess.DataInterface.FillDataset(string.Format("select * from vLoadsPutaway where loadid = '{0}'", ld.LOADID), DTLoadParams, false, null);
                if (DTLoadParams.Rows.Count == 0)
                {
                    if (useLogs)
                        lg.Write("Error Occured - could not extract Load params from vLoadsPutaway....");
                }
                else
                {
                    if (useLogs)
                        lg.Write("Load Params extracted from vLoadsPutaway....");
                }
                PolicyMatcher oPolicyMatcher = new PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams);
                dtConsStrategy = oPolicyMatcher.FindMatchingPolicies();
                foreach (DataRow drConsStrategy in dtConsStrategy.Rows)
                {
                    if (useLogs)
                    {
                        lg.writeSeperator('*', 80);
                        lg.Write("Trying to find location according to policy: " + drConsStrategy["POLICYID"]);
                    }
                    string SQL = null;
                    System.Collections.ArrayList ProcessedLinesByPriority = new System.Collections.ArrayList();
                    if (true) 
                    {
                        SQL = string.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS,ISNULL(PUTAWAYPOLICYDETAIL.PREPOPULATELOCATION, 0) AS PREPOPULATELOCATION from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy["POLICYID"]); //RWMS-1277
                        DataTable dtRegions = new DataTable();
                        DataInterface.FillDataset(SQL, dtRegions, false, "");
                        foreach (DataRow drRegion in dtRegions.Rows)
                        {
                            int priority = Convert.ToInt32(drRegion["Priority"]);
                            if (!ProcessedLinesByPriority.Contains(priority))
                            {
                                ProcessedLinesByPriority.Add(priority);
                                string sRegion,locStorageType,loccontent = null;
                                if (drRegion["PUTREGION"] == System.DBNull.Value) sRegion = ""; else sRegion = Convert.ToString(drRegion["PUTREGION"]);
                                if (drRegion["LOCSTORAGETYPE"] == System.DBNull.Value) locStorageType = ""; else locStorageType = Convert.ToString(drRegion["LOCSTORAGETYPE"]);
                                if (Content == "")
                                {
                                    if (drRegion["CONTENT"] == System.DBNull.Value) loccontent = ""; else loccontent = Convert.ToString(drRegion["CONTENT"]);
                                }
                                else
                                    loccontent = Content;

                                int iNumPallets = -1;
                                if (drRegion["MAXNUMPALLETS"] == System.DBNull.Value) iNumPallets = -1; else iNumPallets = Convert.ToInt32(drRegion["MAXNUMPALLETS"]);

                                if (useLogs)
                                {
                                    lg.Write("Calling the Location.LocToPlaceMulti function.");
                                }
                                WarehouseareaID = "";
                                LocationID = "";
                                prePopulateLocation = Convert.ToString(drRegion["PREPOPULATELOCATION"]);//RWMS-1277
                                // Start Modified For pallaet height in for single load
                                List<LoadsWithSequence> loads = new List<LoadsWithSequence>();
                                loads.Add(lds);
                                Location.LocToPlaceMulti(ref LocationID, ref WarehouseareaID, Convert.ToString(drRegion["PUTAWAYPOLICY"]), loads, sRegion, Convert.ToBoolean(drRegion["FITBYVOLUME"]), Convert.ToBoolean(drRegion["FITBYWEIGHT"]), Convert.ToBoolean(drRegion["FITBYHEIGHT"]), Convert.ToBoolean(drRegion["FITBYPALLETTYPE"]), iNumPallets, assignedLoads, operatorsEqpHeight, locStorageType, loccontent, QtyToPlace, lg, "", "");
                                // End Modified For pallaet height in for single load
                                if (!string.IsNullOrEmpty(LocationID) & (LocationID != null))
                                {
                                    return LocationID;
                                }
                            }
                        }
                    }
                }
                if (useLogs)
                {
                    if (!string.IsNullOrEmpty(LocationID))
                        lg.Write("Got Location: " + LocationID + " in warehousearea: " + WarehouseareaID);
                    else
                        lg.Write("Could not find location for load.");
                }
                //If no location was found -> return same location where the load is sitting currently, as it happens for single payload putaway which is assigned from the RDT UI Code.
                WarehouseareaID = ld.WAREHOUSEAREA;
                return ld.LOCATION;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - " + ex.ToString());
                }
                // No Error, place it in same location
                return ld.LOCATION;
            }
        }

        /// <summary>
        /// Does a putaway as a multi-payload putaway while handling a multiple payload puaway request
        /// </summary>
        /// <param name="qMsg"></param>
        /// <param name="qSender"></param>
        private void FindMultipleLoadDestinationLocation(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string LoadIDs = "";
            List<LoadsWithSequence> loadsWithSequence = new List<LoadsWithSequence>();
            String[] loadIdsSplited;
            string Region = "";
            string LocationID = "";
            string WarehouseareaID = "";
            string User = "";
            string Content = "";
            Boolean IsSingleGroup = false;
            decimal QtyToPlace = -1;
            double operatorsEqpHeight = 0;
            // Input extraction and validation.
            try
            {
                User = qSender.Values["USERID"];
                //fetch operators equipment height
                operatorsEqpHeight = GetOperatorsEquipmentHeight(User);
                //fetch operators equipment height
            }
            catch
            {
                lg.Write("User Id is not passed in the message, cannot fetch operators equipment height, assuming 0.");
                operatorsEqpHeight = 0;
            }
            try
            {
                LoadIDs = qSender.Values["LOADID"];
                if (useLogs)
                {
                    lg.Write("Got the Load Ids from the QSender: " + LoadIDs);
                }

                if (LoadIDs == null || LoadIDs == "")
                {
                    ResponseOnErrorForLoads(LoadIDs, 2, qMsg);
                    return;
                }
                loadIdsSplited = Array.ConvertAll(LoadIDs.Split(','), p => p.Trim());
                int sequence = 1;
                foreach (String loadid in loadIdsSplited)
                {
                    LoadsWithSequence lds = new LoadsWithSequence();
                    WMS.Logic.Load ld = new WMS.Logic.Load(loadid, true);
                    lds.Load = ld;
                    lds.SequenceNo = sequence;
                    sequence++;
                    loadsWithSequence.Add(lds);
                }
            }
            catch (M4NException m4nex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load one of the Load Object.");
                    lg.Write(m4nex.Description);
                }
                ResponseOnErrorForLoads(LoadIDs, 1, qMsg);
                return;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load one of the Load Object.");
                    lg.Write(ex.StackTrace);
                }
                ResponseOnErrorForLoads(LoadIDs, 1, qMsg);
                return;
            }
            // Processing
            try
            {
                // Chek here for groups. If groups are not in the same, then go for single putawy.

                //List<List<LoadsWithGroup>> groups = MultiPayloadPutawayHelper.GroupLoads(loadsWithSequence,lg);
                IsSingleGroup = MultiPayloadPutawayHelper.GroupLoads(loadsWithSequence, lg);

                // Check if more than one group, if more than once gruop multi -payload putawy cannot be done. Go for single payload putawy.
                if (!IsSingleGroup)
                {
                    FindMultipleLoadDestinationLocationIndividual(qMsg, qSender);
                    return;
                }

                // At this time since all the loads forms a single group. So the load attributes policies for any of the loads can be considered.
                // Taking the first one(Much like distance calculation)

                DataTable dtConsStrategy = new DataTable();
                DataTable DTLoadParams = new DataTable();
                // Consider the first one.
                Made4Net.DataAccess.DataInterface.FillDataset(string.Format("select * from vLoadsPutaway where loadid = '{0}'", loadsWithSequence[0].Load.LOADID), DTLoadParams, false, null);
                if (DTLoadParams.Rows.Count == 0)
                {
                    if (useLogs)
                        lg.Write("Error Occured - could not extract Load params from vLoadsPutaway....");
                }
                else
                {
                    if (useLogs)
                        lg.Write("Load Params extracted from vLoadsPutaway....");
                }
                PolicyMatcher oPolicyMatcher = new PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams);
                dtConsStrategy = oPolicyMatcher.FindMatchingPolicies();
                foreach (DataRow drConsStrategy in dtConsStrategy.Rows)
                {
                    if (useLogs)
                    {
                        lg.writeSeperator('*', 80);
                        lg.Write("Trying to find location according to policy: " + drConsStrategy["POLICYID"]);
                    }
                    string SQL = null;
                    System.Collections.ArrayList ProcessedLinesByPriority = new System.Collections.ArrayList();
                    if (true) //(Convert.ToBoolean(DataInterface.ExecuteScalar(SQL)))
                    {
                        SQL = string.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS,ISNULL(PUTAWAYPOLICYDETAIL.PREPOPULATELOCATION, 0) AS PREPOPULATELOCATION from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy["POLICYID"]);//RWMS-1277
                        DataTable dtRegions = new DataTable();
                        DataInterface.FillDataset(SQL, dtRegions, false, "");
                        foreach (DataRow drRegion in dtRegions.Rows)
                        {
                            int priority = Convert.ToInt32(drRegion["Priority"]);
                            if (!ProcessedLinesByPriority.Contains(priority))
                            {
                                ProcessedLinesByPriority.Add(priority);
                                string sRegion, locStorageType, loccontent = null;
                                if (drRegion["PUTREGION"] == System.DBNull.Value) sRegion = ""; else sRegion = Convert.ToString(drRegion["PUTREGION"]);
                                if (drRegion["LOCSTORAGETYPE"] == System.DBNull.Value) locStorageType = ""; else locStorageType = Convert.ToString(drRegion["LOCSTORAGETYPE"]);
                                if (Content == "")
                                {
                                    if (drRegion["CONTENT"] == System.DBNull.Value) loccontent = ""; else loccontent = Convert.ToString(drRegion["CONTENT"]);
                                }
                                else
                                    loccontent = Content;

                                int iNumPallets = -1;
                                if (drRegion["MAXNUMPALLETS"] == System.DBNull.Value) iNumPallets = -1; else iNumPallets = Convert.ToInt32(drRegion["MAXNUMPALLETS"]);

                                if (useLogs)
                                {
                                    lg.Write("Calling the Location.LocToPlaceMulti function.");
                                }
                                WarehouseareaID = "";
                                LocationID = "";//Convert.ToString(drRegion["PUTAWAYPOLICY"]);
                                //Start RWMS-1277
                                string PrePopulateLocation = "";
                                PrePopulateLocation = Convert.ToString(drRegion["PREPOPULATELOCATION"]);
                                //End RWMS-1277
                                List<LoadsWithSequence> assignedLoads = new List<LoadsWithSequence>();

                                Location.LocToPlaceMulti(ref LocationID, ref WarehouseareaID, Convert.ToString(drRegion["PUTAWAYPOLICY"]), loadsWithSequence, sRegion, Convert.ToBoolean(drRegion["FITBYVOLUME"]), Convert.ToBoolean(drRegion["FITBYWEIGHT"]), Convert.ToBoolean(drRegion["FITBYHEIGHT"]), Convert.ToBoolean(drRegion["FITBYPALLETTYPE"]), iNumPallets, assignedLoads, operatorsEqpHeight, locStorageType, loccontent, QtyToPlace, lg, "", "");
                                if (!string.IsNullOrEmpty(LocationID) & (LocationID != null))
                                {
                                    // Response for locationID should be in this format:
                                    // G000000005=4622A1,G000000010=4622A1,G000000004=4622A1
                                    // Convert here
                                    // Got the Location for which all the loads will fit.
                                    String[] responseStringArray = new String[loadsWithSequence.Count];
                                    int count = 0;
                                    foreach (LoadsWithSequence lds in loadsWithSequence)
                                    {
                                        responseStringArray[count] = (lds.Load.LOADID + "=" + LocationID);
                                        count++;
                                    }

                                    ResponseOnSuccessMulti(LoadIDs, String.Join(",", responseStringArray), WarehouseareaID, qMsg, PrePopulateLocation);//RWMS-1277
                                    return;
                                }
                            }
                        }
                        // If control comes here, that means none of the policy with location were not able to take all the loads. Need to go for single putaway.
                        FindMultipleLoadDestinationLocationIndividual(qMsg, qSender);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - " + ex.ToString());
                }
                ResponseOnError(Region, 1, qMsg);
                return;
            }
        }
        #endregion  Multi Pallet Putawy

        private void FindLoadDestinationLocation(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string LoadID = "";
            string Region = "";
            string LocationID = "";
            string WarehouseareaID = "";
            string User = "";
            string Content = "";
            decimal QtyToPlace = -1;
            WMS.Logic.Load ld = null;
            WMS.Logic.SKU sk = null;
            double operatorsEqpHeight = 0;
            try
            {
                User = qSender.Values["USERID"];
                //fetch operators equipment height
                operatorsEqpHeight = GetOperatorsEquipmentHeight(User);
                //fetch operators equipment height
            }
            catch (Exception)
            {
                lg.Write("User Id is not passed in the message, cannot fetch operators equipment height, assuming 0.");
                operatorsEqpHeight = 0;
            }
            try
            {
                LoadID = qSender.Values["LOADID"];
                if (useLogs)
                {
                    lg.Write("Got the Load Id from the QSender: " + LoadID);
                }

                if (LoadID == null || LoadID == "")
                {
                    ResponseOnError(LoadID, 2, qMsg);
                    return;
                }
                ld = new WMS.Logic.Load(LoadID, true);
            }
            catch (M4NException m4nex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Load Object.");
                    lg.Write(m4nex.Description);
                }
                ResponseOnError(LoadID, 1, qMsg);
                return;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Load Object.");
                    lg.Write(ex.StackTrace);
                }
                ResponseOnError(LoadID, 1, qMsg);
                return;
            }
            try
            {
                sk = new SKU(ld.CONSIGNEE, ld.SKU, true);
                DataTable dtConsStrategy = new DataTable();
                DataTable DTLoadParams = new DataTable();
                Made4Net.DataAccess.DataInterface.FillDataset(string.Format("select * from vLoadsPutaway where loadid = '{0}'", ld.LOADID), DTLoadParams, false, null);
                if (DTLoadParams.Rows.Count == 0)
                {
                    if (useLogs)
                        lg.Write("Error Occured - could not extract Load params from vLoadsPutaway....");
                }
                else
                {
                    if (useLogs)
                        lg.Write("Load Params extracted from vLoadsPutaway....");
                }
                PolicyMatcher oPolicyMatcher = new PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams);
                dtConsStrategy = oPolicyMatcher.FindMatchingPolicies();
                foreach (DataRow drConsStrategy in dtConsStrategy.Rows)
                {
                    if (useLogs)
                    {
                        lg.writeSeperator('*', 80);
                        lg.Write("Trying to find location according to policy: " + drConsStrategy["POLICYID"]);
                    }
                    string SQL = null;
                    System.Collections.ArrayList ProcessedLinesByPriority = new System.Collections.ArrayList();
                    if (true) //(Convert.ToBoolean(DataInterface.ExecuteScalar(SQL)))
                    {
                        SQL = string.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS,ISNULL(PUTAWAYPOLICYDETAIL.PREPOPULATELOCATION, 0) AS PREPOPULATELOCATION from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy["POLICYID"]);//RWMS-1277
                        DataTable dtRegions = new DataTable();
                        DataInterface.FillDataset(SQL, dtRegions, false, "");
                        foreach (DataRow drRegion in dtRegions.Rows)
                        {
                            int priority = Convert.ToInt32(drRegion["Priority"]);
                            if (!ProcessedLinesByPriority.Contains(priority))
                            {
                                ProcessedLinesByPriority.Add(priority);
                                string sRegion, locStorageType, loccontent = null;
                                if (drRegion["PUTREGION"] == System.DBNull.Value) sRegion = ""; else sRegion = Convert.ToString(drRegion["PUTREGION"]);
                                if (drRegion["LOCSTORAGETYPE"] == System.DBNull.Value) locStorageType = ""; else locStorageType = Convert.ToString(drRegion["LOCSTORAGETYPE"]);
                                if (Content == "")
                                {
                                    if (drRegion["CONTENT"] == System.DBNull.Value) loccontent = ""; else loccontent = Convert.ToString(drRegion["CONTENT"]);
                                }
                                else
                                    loccontent = Content;

                                int iNumPallets = -1;
                                if (drRegion["MAXNUMPALLETS"] == System.DBNull.Value) iNumPallets = -1; else iNumPallets = Convert.ToInt32(drRegion["MAXNUMPALLETS"]);

                                if (useLogs)
                                {
                                    lg.Write("Calling the Location.LocToPlace function.");
                                }
                                WarehouseareaID = "";
                                LocationID = "";//Convert.ToString(drRegion["PUTAWAYPOLICY"]);
                                //Start RMWS-1277
                                string PrePopulateLocation = "";
                                PrePopulateLocation = Convert.ToString(drRegion["PREPOPULATELOCATION"]);
                                //End RMWS-1277
                                Location.LocToPlace(ref LocationID, ref WarehouseareaID, Convert.ToString(drRegion["PUTAWAYPOLICY"]), ld, sRegion, Convert.ToBoolean(drRegion["FITBYVOLUME"]), Convert.ToBoolean(drRegion["FITBYWEIGHT"]), Convert.ToBoolean(drRegion["FITBYHEIGHT"]), Convert.ToBoolean(drRegion["FITBYPALLETTYPE"]), iNumPallets, operatorsEqpHeight, locStorageType, loccontent, QtyToPlace, lg, "", "");
                                if (!string.IsNullOrEmpty(LocationID) & (LocationID != null))
                                {
                                    ResponseOnSuccess(LoadID, LocationID, WarehouseareaID, qMsg, PrePopulateLocation);//RWMS-1277
                                    WMS.Logic.Putaway.PutAwayScoring p = new WMS.Logic.Putaway.PutAwayScoring(Convert.ToString(drRegion["PUTAWAYPOLICY"]));
                                    if (p.get_DoesAttributeExistsOrHasValue("DISTANCE"))
                                    {
                                        try
                                        {
                                            //string pickloc = DistanceApplied.GetPickLocationForSKU(ld, lg);
                                            string strPickLocFrontRackLoc = "";
                                            string pickloc = DistanceApplied.GetPickLocAndFrontRackLocForSKU(ld, ref strPickLocFrontRackLoc, lg);
                                            if (!string.IsNullOrEmpty(strPickLocFrontRackLoc))
                                            {
                                                //if pickloc found as frontRacklocation then we need to use the frontracklocation instead of pickloc
                                                pickloc = strPickLocFrontRackLoc;
                                            }
                                            if (!String.IsNullOrEmpty(pickloc))
                                            {
                                                IShortestPathProvider SHPath = ShortestPath.GetInstance();

                                                DataManager.ServiceModel.Path path = new DataManager.ServiceModel.Path();
                                                // Fetch source Location
                                                String sqlLoc = "Select * from LOCATION where LOCATION = '{0}'";
                                                DataTable locationFrom = new DataTable();
                                                DataInterface.FillDataset(String.Format(sqlLoc, pickloc), locationFrom);
                                                if (locationFrom.Rows.Count > 0)
                                                {
                                                    DataRow @from = locationFrom.Rows[0];

                                                    DataTable locationTo = new DataTable();
                                                    DataInterface.FillDataset(String.Format(sqlLoc, LocationID), locationTo);

                                                    if (locationTo.Rows.Count > 0)
                                                    {
                                                        DataRow toLoc = locationTo.Rows[0];

                                                        if (User != string.Empty)
                                                        {
                                                            List<Rules> rules = new List<Rules>();
                                                            Rules rule = new Rules();
                                                            rule.Parameter = Made4Net.Algorithms.Constants.Height;
                                                            rule.Data = GetOperatorsEquipmentHeight(User).ToString();
                                                            rule.Operator = ">";
                                                            rules.Add(rule);

                                                            path = SHPath.GetShortestPathWithContsraints(@from, toLoc, null, "LOADPW", true, rules, Common.GetShortestPathLogger(lg));
                                                        }
                                                        else
                                                        {
                                                            path = SHPath.GetShortestPathWithContsraints(@from, toLoc, null, "LOADPW", true, null, Common.GetShortestPathLogger(lg));
                                                        }
                                                        lg.Write("Node to Node Traversed Distance");
                                                        lg.writeSeperator('-', 80);

                                                        foreach (KeyValuePair<string, double> item in path.TraversedNodes)
                                                        {
                                                            lg.Write(String.Format("Node : {0} -- Distance : {1}", item.Key, item.Value));
                                                        }
                                                        lg.writeSeperator('-', 80);
                                                    }
                                                    else
                                                    {
                                                        lg.Write("ToLocation don't exists in database, cannot print path.");
                                                    }
                                                }
                                                else
                                                {
                                                    lg.Write("FromLocation don't exists in database, cannot print path.");
                                                }
                                            }
                                            else
                                            {
                                                lg.Write("Picklocation don't exists for the given load, cannot print path.");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            lg.Write("Distance calculation threw exception for the selected location found by putaway. Cannot display the walk nodes.");
                                        }
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
                if (useLogs)
                {
                    if (!string.IsNullOrEmpty(LocationID))
                        lg.Write("Got Location: " + LocationID + " in warehousearea: " + WarehouseareaID);
                    else
                        lg.Write("Could not find location for load.");
                }
                //If no location was found -> return empty location
                ResponseOnSuccess(LoadID, "", "", qMsg, "");//RWMS-1277
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occurred - " + ex.ToString());
                }
                ResponseOnError(Region, 1, qMsg);
                return;
            }
        }

        private void getLocByLocation(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string LoadID = "";
            string LocationID = "";
            string WarehouseareaID = "";
            string User = "";
            Load ld = null;
            string StorageType = "";
            string Content = "";
            bool pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType;
            double operatorsEqpHeight = 0;
            try
            {
                User = qSender.Values["USERID"];
                //fetch operators equipment height
                operatorsEqpHeight = GetOperatorsEquipmentHeight(User);
                //fetch operators equipment height
            }
            catch
            {
                lg.Write("User Id is not passed in the message, cannot fetch operators equipment height, assuming 0.");
                operatorsEqpHeight = 0;
            }
            try
            {
                LoadID = qSender.Values["LOADID"];

                if (LoadID == null || LoadID == "")
                {
                    ResponseOnError(LoadID, 2, qMsg);
                    return;
                }

                ld = new Load(LoadID, true);
            }
            catch
            {
                ResponseOnError(LoadID, 1, qMsg);
                return;
            }
            try
            {
                LocationID = qSender.Values["LOCATION"];
                WarehouseareaID = qSender.Values["WAREHOUSEAREA"];

                if (LocationID == null || LocationID == "")
                {
                    ResponseOnError(LoadID, 5, qMsg);
                    return;
                }

                if (WarehouseareaID == null || WarehouseareaID == "")
                {
                    ResponseOnError(LoadID, 5, qMsg);
                    return;
                }
                StorageType = qSender.Values["STORAGETYPE"];
                if (useLogs)
                    lg.Write("Got the storagetype : " + StorageType + " From QSender.");

                Content = qSender.Values["CONTENT"];

                if (useLogs)
                    lg.Write("Calling the Location.LocToPlace function.");

                pFitByVolume = Convert.ToBoolean(qSender.Values["FITBYVOLUME"]);
                pFitByWeight = Convert.ToBoolean(qSender.Values["FITBYWEIGHT"]);
                pFitByHeight = Convert.ToBoolean(qSender.Values["FITBYHEIGHT"]);
                pFitByPalletType = Convert.ToBoolean(qSender.Values["FITBYPALLETTYPE"]);

                int iNumPallets = -1;
                //if (drRegion["MAXNUMPALLETS"] == System.DBNull.Value) iNumPallets = -1; else iNumPallets = Convert.ToInt32(drRegion["MAXNUMPALLETS"]);

                Location.LocToPlace(ref LocationID, ref WarehouseareaID, "", ld, "", pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType, iNumPallets, operatorsEqpHeight, StorageType, Content, -1, lg, LocationID, WarehouseareaID);

                if (useLogs)
                {
                    if (!string.IsNullOrEmpty(LocationID))
                        lg.Write("Got Location: " + LocationID + " in warehousearea: " + WarehouseareaID);
                    else
                        lg.Write("Couldnt find location for load.");
                }
                ResponseOnSuccess(LoadID, LocationID, WarehouseareaID, qMsg, "");//RWMS-1277
            }
            catch
            {
                ResponseOnError(LocationID, 1, qMsg);
                return;
            }
        }

        private void getLocByRegion(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string LoadID = "";
            string Region = "";
            string LocationID = "";
            string WarehouseareaID = "";
            string User = "";
            string StorageType = "";
            string Content = "";
            string Inv = "";
            decimal QtyToPlace = -1;
            Load ld = null;
            string PolicyId;
            bool pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType;
            double operatorsEqpHeight = 0;
            try
            {
                User = qSender.Values["USERID"];
                //fetch operators equipment height
                operatorsEqpHeight = GetOperatorsEquipmentHeight(User);
                //fetch operators equipment height
            }
            catch
            {
            }
            try
            {
                LoadID = qSender.Values["LOADID"];

                try
                {
                    if (useLogs)
                    {
                        lg.Write("Got the Load Id from the QSender: " + LoadID);
                    }
                }
                catch
                {
                    lg.Write("User Id is not passed in the message, cannot fetch operators equipment height, assuming 0.");
                    operatorsEqpHeight = 0;
                }

                if (LoadID == null || LoadID == "")
                {
                    ResponseOnError(LoadID, 2, qMsg);
                    return;
                }
                ld = new Load(LoadID, true);
            }
            catch (M4NException m4nex)
            {
                try
                {
                    if (useLogs)
                    {
                        lg.Write("Error Occured - could not load the Load Object.");
                        lg.Write(m4nex.Description);
                    }
                }
                catch
                {
                }
                ResponseOnError(LoadID, 1, qMsg);
                return;
            }
            catch (Exception ex)
            {
                try
                {
                    if (useLogs)
                    {
                        lg.Write("Error Occured - could not load the Load Object.");
                        lg.Write(ex.Message);
                    }
                }
                catch
                {
                }
                ResponseOnError(LoadID, 1, qMsg);
                return;
            }
            try
            {
                Region = qSender.Values["REGION"];
                StorageType = qSender.Values["STORAGETYPE"];
                Inv = qSender.Values["INV"];

                try
                {
                    if (useLogs)
                    {
                        lg.Write("Got the Region : " + Region + " and storagetype : " + StorageType + " and inv: " + Inv + " From QSender.");
                    }
                }
                catch
                {
                }

                switch (Inv)
                {
                    case "True":
                        Inv = "1";
                        break;
                    case "False":
                        Inv = "0";
                        break;
                }
                Content = qSender.Values["CONTENT"];

                if (Region == null || Region == "")
                {
                    try
                    {
                        if (useLogs)
                        {
                            lg.Write("Region is empty - can't find location.");
                        }
                    }
                    catch
                    {
                    }

                    ResponseOnError(LoadID, 6, qMsg);
                    return;
                }
                try
                {
                    QtyToPlace = Convert.ToDecimal(qSender.Values["QTYTOPLACE"]);
                }
                catch
                {
                    QtyToPlace = -1;
                }
                try
                {
                    if (useLogs)
                    {
                        lg.Write("Calling the Location.LocToPlace function.");
                    }
                }
                catch
                {
                }

                PolicyId = qSender.Values["POLICYID"];
                pFitByVolume = Convert.ToBoolean(qSender.Values["FITBYVOLUME"]);
                pFitByWeight = Convert.ToBoolean(qSender.Values["FITBYWEIGHT"]);
                pFitByHeight = Convert.ToBoolean(qSender.Values["FITBYHEIGHT"]);
                pFitByPalletType = Convert.ToBoolean(qSender.Values["FITBYPALLETTYPE"]);

                int iNumPallets = -1;
                //if (drRegion["MAXNUMPALLETS"] == System.DBNull.Value) iNumPallets = -1; else iNumPallets = Convert.ToInt32(drRegion["MAXNUMPALLETS"]);

                Location.LocToPlace(ref LocationID, ref WarehouseareaID, PolicyId, ld, Region, pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType, iNumPallets, operatorsEqpHeight, StorageType, Content, QtyToPlace, lg, "", "");

                try
                {
                    if (useLogs)
                    {
                        lg.Write("Got Location : " + LocationID + " From the LocToPlace.");
                    }
                }
                catch
                {
                }

                if (LocationID != "")
                {
                    ResponseOnSuccess(LoadID, LocationID, WarehouseareaID, qMsg, "");//RWMS-1277
                }
                else
                {
                    ResponseOnSuccess(LoadID, "", "", qMsg, "");//RWMS-1277
                }
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - " + ex.ToString());
                }
                ResponseOnError(Region, 1, qMsg);
                return;
            }
        }

        #endregion

        #region Location Search for Container Putaway

        private void FindContainerDestinationLocation(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string contId = "";
            string LocationID = "";
            string WarehouseareaID = "";
            string User = "";
            WMS.Logic.Container oCont = null;
            User = qSender.Values["USERID"];
            try
            {
                contId = qSender.Values["CONTAINERID"];
                if (useLogs)
                    lg.Write("Got the Container Id from the QSender: " + contId);
                if (contId == null || contId == "")
                {
                    ResponseOnError(contId, 2, qMsg);
                    return;
                }
                oCont = new WMS.Logic.Container(contId, true);
            }
            catch (M4NException m4nex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Container Object...");
                    lg.Write(m4nex.Description);
                }
                ResponseOnError(contId, 1, qMsg);
                return;
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - could not load the Container Object...");
                    lg.Write(ex.ToString());
                }
                ResponseOnError(contId, 1, qMsg);
                return;
            }
            try
            {
                if (useLogs)
                    lg.Write("Trying to match putaway policy according to container content...");
                string loads = ContainerLoadsString(oCont);
                DataTable dtConsStrategy = new DataTable();
                DataTable DTLoadParams = new DataTable();
                Made4Net.DataAccess.DataInterface.FillDataset(string.Format("select * from vLoadsPutaway where loadid in ({0})", loads), DTLoadParams, false, null);
                if (DTLoadParams.Rows.Count == 0)
                {
                    if (useLogs)
                        lg.Write("Error Occured - could not extract Container Load params from vLoadsPutaway...");
                }
                else
                {
                    if (useLogs)
                        lg.Write("Container Load Params extracted from vLoadsPutaway...");
                }
                GetCommonFiledsTable(ref DTLoadParams);
                PolicyMatcher oPolicyMatcher = new PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams);
                dtConsStrategy = oPolicyMatcher.FindMatchingPolicies();
                if (useLogs)
                    lg.Write(dtConsStrategy.Rows.Count.ToString() + " Policies found after matching policies...");
                foreach (DataRow drConsStrategy in dtConsStrategy.Rows)
                {
                    if (useLogs)
                    {
                        lg.writeSeperator('*', 80);
                        lg.Write("Trying to find location according to policy: " + drConsStrategy["POLICYID"]);
                    }
                    string SQL = null;
                    System.Collections.ArrayList ProcessedLinesByPriority = new System.Collections.ArrayList();
                    SQL = string.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE,ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy["POLICYID"]);
                    DataTable dtRegions = new DataTable();
                    DataInterface.FillDataset(SQL, dtRegions, false, "");
                    foreach (DataRow drRegion in dtRegions.Rows)
                    {
                        int priority = Convert.ToInt32(drRegion["Priority"]);
                        if (!ProcessedLinesByPriority.Contains(priority))
                        {
                            ProcessedLinesByPriority.Add(priority);
                            string sRegion, locStorageType, content = null;
                            if (drRegion["PUTREGION"] == System.DBNull.Value) sRegion = ""; else sRegion = Convert.ToString(drRegion["PUTREGION"]);
                            if (drRegion["LOCSTORAGETYPE"] == System.DBNull.Value) locStorageType = ""; else locStorageType = Convert.ToString(drRegion["LOCSTORAGETYPE"]);
                            if (drRegion["CONTENT"] == System.DBNull.Value) content = ""; else content = Convert.ToString(drRegion["CONTENT"]);

                            if (useLogs)
                            {
                                lg.Write("Calling the Location Search for Container Putaway...");
                            }
                            LocationID = "";
                            Location.LocToPlace(ref LocationID, ref WarehouseareaID, Convert.ToString(drRegion["PUTAWAYPOLICY"]), oCont, sRegion, Convert.ToBoolean(drRegion["FITBYVOLUME"]), Convert.ToBoolean(drRegion["FITBYWEIGHT"]), Convert.ToBoolean(drRegion["FITBYHEIGHT"]), Convert.ToBoolean(drRegion["FITBYPALLETTYPE"]), locStorageType, "", lg);
                            if (!string.IsNullOrEmpty(LocationID) & (LocationID != null))
                            {
                                if (useLogs)
                                    lg.Write("Got Location: " + LocationID + " in warehouse area: " + WarehouseareaID);
                                ResponseOnSuccess(oCont.ContainerId, LocationID, WarehouseareaID, qMsg, "");//RWMS-1277
                                return;
                            }
                        }
                    }
                }
                if (useLogs)
                    lg.Write("No Location Found for receiving the container...");
                //If no location was found -> return empty location
                ResponseOnSuccess(oCont.ContainerId, "", "", qMsg, "");//RWMS-1277
            }
            catch (Exception ex)
            {
                if (useLogs)
                {
                    lg.Write("Error Occured - " + ex.ToString());
                }
                ResponseOnError(oCont.ContainerId, 1, qMsg);
                return;
            }
        }

        private string ContainerLoadsString(WMS.Logic.Container oCont)
        {
            string loads = string.Empty;
            foreach (WMS.Logic.Load oLoad in oCont.Loads)
            {
                loads = loads + "," + "'" + oLoad.LOADID + "'";
            }
            loads = loads.TrimStart(",".ToCharArray());
            return loads;
        }

        private void GetCommonFiledsTable(ref DataTable DTLoadParams)
        {
            //DataTable commonFields = new DataTable();
            DataRow commonRow;

            if (DTLoadParams.Rows.Count <= 1)
                return;

            commonRow = DTLoadParams.NewRow();
            foreach (DataColumn tmpCol in DTLoadParams.Columns)
            {
                if (DataMatched(DTLoadParams, tmpCol.ColumnName))
                    commonRow[tmpCol.ColumnName] = DTLoadParams.Rows[0][tmpCol.ColumnName];
            }
            DTLoadParams.Rows.InsertAt(commonRow, 0);
            for (int i = 1; i < DTLoadParams.Rows.Count - 1; i++)
                DTLoadParams.Rows.RemoveAt(i);
        }

        private Boolean DataMatched(DataTable DTLoadParams, string pColumnName)
        {
            for (int i = 0; i < DTLoadParams.Rows.Count - 1; i++)
            {
                switch (DTLoadParams.Rows[i][pColumnName].GetType().FullName)
                {
                    case "System.String":
                        if (string.Compare(Convert.ToString(DTLoadParams.Rows[i][pColumnName]), Convert.ToString(DTLoadParams.Rows[i + 1][pColumnName]), StringComparison.OrdinalIgnoreCase) != 0)
                            return false;
                        break;
                    case "System.DateTime":
                        if (Convert.ToDateTime(DTLoadParams.Rows[i][pColumnName]) != Convert.ToDateTime(DTLoadParams.Rows[i + 1][pColumnName]))
                            return false;
                        break;
                    case "System.Decimal":
                        if (Convert.ToDecimal(DTLoadParams.Rows[i][pColumnName]) != Convert.ToDecimal(DTLoadParams.Rows[i + 1][pColumnName]))
                            return false;
                        break;
                    case "System.Int32":
                        if (Convert.ToInt32(DTLoadParams.Rows[i][pColumnName]) != Convert.ToInt32(DTLoadParams.Rows[i + 1][pColumnName]))
                            return false;
                        break;
                    case "System.Boolean":
                        if (Convert.ToBoolean(DTLoadParams.Rows[i][pColumnName]) != Convert.ToBoolean(DTLoadParams.Rows[i + 1][pColumnName]))
                            return false;
                        break;
                }
            }
            return true;
        }

        #endregion

        #region Responses

        public override void Dispose()
        {
            base.Dispose();
        }

        public void ResponseOnError(string LoadId, int ErrorType, Message qMsg)
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("ERROR", GetErrorTypeMsg(ErrorType));
                qs.Send(newmq, LoadId, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }

        public void ResponseOnErrorForLoads(string LoadIDs, int ErrorType, Message qMsg)
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("ERROR", GetErrorTypeMsg(ErrorType));
                qs.Send(newmq, LoadIDs, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }


        public void ResponseOnSuccess(string LoadId, string BestLocation, string BestLocationWHArea, Message qMsg, string PrePopulateLocation)//RWMS-1277
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("LOCATION", BestLocation);
                qs.Add("WAREHOUSEAREA", BestLocationWHArea);
                qs.Add("PREPOPULATELOCATION", PrePopulateLocation);//RWMS-1277
                qs.Send(newmq, LoadId, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }

        public void ResponseOnSuccessMulti(string LoadIds, string BestLocations, string BestLocationWHArea, Message qMsg, string PrePopulateLocation)//RWMS-1277
        {
            if (qMsg.ResponseQueue != null)
            {
                MessageQueue newmq = qMsg.ResponseQueue;
                Made4Net.Shared.QMsgSender qs = new Made4Net.Shared.QMsgSender();
                qs.Add("LOCATIONS", BestLocations);
                qs.Add("WAREHOUSEAREA", BestLocationWHArea);
                qs.Add("PREPOPULATELOCATION", PrePopulateLocation);//RWMS-1277
                qs.Send(newmq, LoadIds, qMsg.Id, System.Messaging.MessagePriority.Normal);
            }
        }

        public string GetErrorTypeMsg(int ErrNumber)
        {
            switch (ErrNumber)
            {
                case 0: return "Location Not Found";
                case 1: return "Error getting load info";
                case 2: return "Loadid is empty";
                case 3: return "Error getting sku info";
                case 4: return "Error getting consignee info";
                case 5: return "Location is empty";
                case 6: return "Region is empty";
                case 7: return "No Loc Found";
            }
            return "";
        }

        #endregion

        #region Helpers

        private double GetOperatorsEquipmentHeight(String user)
        {
            String sql = String.Format("select  COALESCE(HANDLINGEQUIPMENT.HEIGHT, 0) as HEIGHT from HANDLINGEQUIPMENT inner join WHACTIVITY on HANDLINGEQUIPMENT.HANDLINGEQUIPMENT = WHACTIVITY.HETYPE where WHACTIVITY.USERID = '{0}'", user);
            DataTable dt = new DataTable();
            DataInterface.FillDataset(sql, dt, false, "");
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["HEIGHT"]);
            }
            return 0;
        }

        #endregion
        #endregion
    }

}
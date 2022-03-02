using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Made4Net.DataAccess;
using Made4Net.Shared;


namespace WMS.ScheduleTasks
{

    public class StagingLaneAssignment
    {

        public StagingLaneAssignment()
        {
        }

        //public bool SetDocumentStagingLane()
        public void Execute()
        {
            SetDocumentStagingLane();
        }

        public void SetDocumentStagingLane()
        {
            //Made4Net.Shared.Logging.LogFile oLogger = null;
            string pDocType = null;
            string pConsignee = null;
            string pDocId = null;
            // WMS.Logic.LogHandler oLogger = new WMS.Logic.LogHandler(Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT param_value FROM sys_param WHERE param_code='ApplicationLogDirectory'", "Made4NetSchema")), "SLAssgn1" + new Random().Next() + ".txt");


            Made4Net.Shared.Logging.LogFile oLogger = new Made4Net.Shared.Logging.LogFile(Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT param_value FROM sys_param WHERE param_code='ApplicationLogDirectory'", "Made4NetSchema")), "SLAssgn1" + new Random().Next() + ".txt", false);
            
            string SQL = string.Format("select consignee, orderid, doctype from DOCUMENTSSLASSIGN ");
            DataTable dt = new DataTable();
            Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt,false,"");


            WMS.Logic.StagingLaneAssignmentSetter SLSetter = new WMS.Logic.StagingLaneAssignmentSetter();
            bool ret = true;
            
            foreach (DataRow dr in dt.Rows)
            {
                pDocType = dr["doctype"].ToString();
                pConsignee = dr["consignee"].ToString();
                pDocId = dr["orderid"].ToString();
                //if (!SLSetter.SetDocumentStagingLane(pDocType, pConsignee, pDocId, oLogger))
                //{
                //    ret = false;
                //}
                if (!SLSetter.SetDocumentStagingLane(pDocType, pConsignee, pDocId, oLogger))
                {
                    ret = false;
                }
            }                     

            if (ret)
            {
                oLogger.WriteLine("Orders assigned to staging lanes",true);
            }
            else
            {
                oLogger.WriteLine("Not all order were assigned to staging lane",true);
            }
            
        }
    }
}
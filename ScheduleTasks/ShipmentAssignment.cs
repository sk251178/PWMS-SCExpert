using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;
using Assignments.AssignmentsGelsons;

namespace ScheduleTasksGelsons
{

    public class ShipmentAssignment
    {

        public ShipmentAssignment()
        {
        }


        #region "Assign To Shipment"

        //public void AssigneOrdersToShipment()
        public void Execute(string pTemplateName)
        {
            string logpath =Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT param_value FROM sys_param WHERE param_code='ApplicationLogDirectory'", "Made4NetSchema"));
            //RWMS-2456
            logpath = Made4Net.DataAccess.Util.BuildAndGetFilePath(logpath);

            WMS.Logic.LogHandler lg = new WMS.Logic.LogHandler(logpath, "ShpAssgn1" + new Random().Next() + ".txt");

            Assignments.AssignmentsGelsons.OrdersAutomationShipmentAssignment sp = new Assignments.AssignmentsGelsons.OrdersAutomationShipmentAssignment(pTemplateName, lg);
            sp.AssigneOrdersToShipment();
        }


        #endregion

    }
}
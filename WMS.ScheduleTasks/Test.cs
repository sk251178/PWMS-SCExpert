using System;
using System.Collections.Generic;
using System.Text;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;

namespace WMS.SchedulerTasks
{
    public class Test
    {
        public Test()
        {

        }

        public void UpdateData()
        {
            DataInterface.RunSQL("UPDATE SKU SET ADDUSER='" + Made4Net.Shared.Warehouse.CurrentWarehouse + DateTime.Now.ToString("ddMMyyyy") + "'");
        }
    }
}

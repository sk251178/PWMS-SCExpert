using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;

namespace WMS.SchedulerTasks
{
    class ZoneReplenishment
    {
        public ZoneReplenishment()
		{
		}
        //Added for RWMS-1840
      string ReplPolicyMethod = WMS.Logic.Replenishment.GetPolicyID();
      //Ended for RWMS-1840

		public void ReplenishAll()
		{
            EventManagerQ em = new EventManagerQ();
            Int32 EventType = (Int32)WMS.Logic.WMSEvents.EventType.ZoneReplenishment;
            em.Add("EVENT", EventType.ToString());
            em.Add("REPLMETHOD", ReplPolicyMethod);
            em.Add("LOCATION", "");
            em.Add("PICKREGION", "");
            em.Add("ALL", "1");
            em.Add("SKU", "");
            em.Send(WMSEvents.get_EventDescription(EventType));
		}

        public void Replenish(string pPutRegion, string pSKU)
        {
            EventManagerQ em = new EventManagerQ();
            Int32 EventType = (Int32)WMS.Logic.WMSEvents.EventType.ZoneReplenishment;
            em.Add("EVENT", EventType.ToString());
            em.Add("REPLMETHOD", ReplPolicyMethod);
            em.Add("LOCATION", "");
            em.Add("PICKREGION", pPutRegion);
            em.Add("ALL", "1");
            em.Add("SKU", pSKU);
            em.Send(WMSEvents.get_EventDescription(EventType));
        }
    }
}
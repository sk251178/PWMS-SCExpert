using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.SchedulerTasks
{
    class LoadsCycleCounts
    {
        public LoadsCycleCounts()
        {

        }
        public void Execute()
        {
            Made4Net.Shared.QMsgSender msgSender = new Made4Net.Shared.QMsgSender();
            int eventType = (int) WMS.Logic.WMSEvents.EventType.LoadCycleCounts;
            msgSender.Add("EVENT",eventType.ToString());
            msgSender.Add("CONSIGNEE","");
            msgSender.Add("SKU","");
            msgSender.Add("SKUGROUP","");
            msgSender.Add("ALL","1");
            msgSender.Send("Counting",WMS.Logic.WMSEvents.get_EventDescription(eventType),"",System.Messaging.MessagePriority.Normal);
        }
       
    }
}

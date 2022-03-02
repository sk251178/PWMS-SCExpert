using System;
using System.Data;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;

namespace WMS.SchedulerTasks
{
	/// <summary>
	/// Summary description for SnapShot.
	/// </summary>
	public class SnapShot
	{
		public SnapShot() 
		{

		}

        public void Execute()
        {
            Made4Net.Shared.QMsgSender oQMsg = new Made4Net.Shared.QMsgSender();
            oQMsg.Add("EVENT", 36.ToString());
            oQMsg.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
            oQMsg.Send("EventManager", "36", "", System.Messaging.MessagePriority.Normal);
        }

        public void ExecuteConsigneeSnapShot(string pConsignee)
        {
            Made4Net.Shared.QMsgSender oQMsg = new Made4Net.Shared.QMsgSender();
            oQMsg.Add("EVENT", 36.ToString());
            oQMsg.Add("CONSIGNEE", pConsignee);
            oQMsg.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now));
            oQMsg.Send("EventManager", "36", "", System.Messaging.MessagePriority.Normal);
        }
	}
}

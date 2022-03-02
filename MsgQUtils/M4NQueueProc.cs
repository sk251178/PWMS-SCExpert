using System;
using Made4Net.Shared;

namespace MsgQUtils
{
	/// <summary>
	/// Summary description for M4NQueueProc.
	/// </summary>
	public class M4NQueueProc : QHandler
	{
		public M4NQueueProc(string qName):base("ExpertSapAddOn",false)
		{
		}

		protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
		{	
			string msgContent = string.Empty;
			for (int i = 0; i < qSender.Values.Count - 1; i++)
			{
				msgContent = msgContent + qSender.Values[i] + "\r\n";
			}
			MsgReaderForm.msgText += msgContent;
		}
	}
}

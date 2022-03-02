using System;
using Made4Net.DataAccess;
using Made4Net.Shared;
using Made4Net.Schema;
using WMS.Logic;
using System.Messaging;
using System.Configuration;
using System.Collections.Specialized;
namespace WMS.EventManager
{
    /// <summary>
    /// Summary description for EventManager.
    /// </summary>
    public class EventManager:Made4Net.Shared.QHandler
    {
        private bool UseLogs = false;
        private string lodDir = "";
        private int failurecount = 0;
        private int maxFailures = 0;
        private LogHandler oLogger = null;

        public EventManager():base("EventManager",false)
        {
            string userlog =Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.EventManagerServiceConfigSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.EventManagerServiceUseLogs);
            UseLogs = Convert.ToBoolean(Convert.ToInt32(userlog));
            lodDir = Made4Net.DataAccess.Util.GetInstancePath() + "\\" + Made4Net.Shared.ConfigurationSettingsConsts.EventManagerServiceLogsDir;
            maxFailures = Convert.ToInt32(Made4Net.Shared.Util.GetAppConfigNameValue("EventManagerServiceConfig", "FailureCount"));
        }

        protected override void ProcessQueue(System.Messaging.Message qMsg,Made4Net.Shared.QMsgSender qSender, System.Messaging.PeekCompletedEventArgs e)
        {
            if(UseLogs)
            {
                oLogger = new LogHandler(lodDir,"EM_" + DateTime.Now.ToString("ddMMyy_hhmmss_") + new Random().Next() + ".txt");
                oLogger.StartWrite();
            }
            string action = qSender.Values["ACTION"];
            string currentWH = qSender.Values["WAREHOUSE"];
            try
            {
                if (IsAudit(action))
                {
                    PushMsg(qMsg,WMS.Lib.MSMQUEUES.AUDIT,qSender);
                    if(UseLogs)
                    {
                        oLogger.Write("Message content :");
                        oLogger.Write(PrintMessageContent(qSender));
                    }
                }
            }
            catch(Exception ex)
            {
                failurecount++;
                if(UseLogs)
                {
                    oLogger.WriteException(ex);
                }
                //RWMS-1428
                //if(failurecount >= maxFailures)
                //{
                //    this.StopQueue();
                //}
                throw new M4NQHandleException(ex.Message,false);
            }

            try
            {
                //Commented for RWMS-1776
                //if(qSender.Values["EVENT"] != null)
                //{
                //    Made4Net.DataAccess.Collections.GenericCollection oc = WMS.Logic.WMSEvents.getRegisteredProcesses(Convert.ToInt32(qSender.Values["EVENT"]));
                //    for(int idx = 0;idx < oc.Count; idx++)
                //    {
                //        WMS.Logic.EVENTREG regq = (WMS.Logic.EVENTREG)oc[idx];
                //        PushMsg(qMsg,regq,qSender);
                //        if(UseLogs)
                //        {
                //            oLogger.Write("Message " + qSender.Values["EVENT"] + " sent to " + regq.REGQUEUENAME + " queue");
                //        }
                //    }
                //}
                //End Commented for RWMS-1776
                //Added for RWMS-1776
                if (qSender.Values["EVENT"] != null)
                {
                    Made4Net.DataAccess.Collections.GenericCollection oc = WMS.Logic.WMSEvents.getRegisteredProcesses(Convert.ToInt32(qSender.Values["EVENT"]));
                    if (oc.Count > 0)
                    {
                        for (int idx = 0; idx < oc.Count; idx++)
                        {
                            WMS.Logic.EVENTREG regq = (WMS.Logic.EVENTREG)oc[idx];
                            PushMsg(qMsg, regq, qSender);
                            if (UseLogs)
                            {
                                oLogger.Write("Message " + qSender.Values["EVENT"] + " sent to " + regq.REGQUEUENAME + " queue");
                            }
                        }
                    }
                }
                else
                {
                    //RWMS-2620 START
                    if (!IsAudit(action))
                    {
                        PushMsgToDeadLetterQueue(qMsg, qSender);
                    }
                    //RWMS-2620 END

                    //RWMS-2620 Comment
                    ////send msg to deadletter queue
                    //PushMsgToDeadLetterQueue(qMsg, qSender);
                    //RWMS-2620 Comment END
                }
                //End Added for RWMS-1776
            }
            catch (Exception ex)
            {
                failurecount++;
                if (UseLogs)
                {
                    oLogger.WriteException(ex);
                }
                //RWMS-1428
                //if(failurecount >= maxFailures)
                //{
                //    this.StopQueue();
                //}

                //Added for RWMS-1776 - send msg to deadletter queue
                PushMsgToDeadLetterQueue(qMsg, qSender);
                //End Added for RWMS-1776

                throw new M4NQHandleException(ex.Message, false);
            }

            if(UseLogs)
            {
                oLogger.Write("Message was proccessed successfully....");
                oLogger.EndWrite();
            }
        }

        //Added for RWMS-1776 - send msg to deadletter queue
        private void PushMsgToDeadLetterQueue(System.Messaging.Message qMsg, Made4Net.Shared.QMsgSender qSender)
        {
            string QName = "DeadLetterQueue";
            if (UseLogs)
                oLogger.Write(PrintMessageContent(qSender));

            try
            {
                qSender.Send(QName, qMsg.Label, "", System.Messaging.MessagePriority.Normal);
            }
            catch (Exception ex)
            {
                if (UseLogs)
                    oLogger.Write(ex.Message.ToString());

                throw new M4NQHandleException(ex.Message.ToString(), false);
            }
        }
        //End Added for RWMS-1776

        private void PushMsg(System.Messaging.Message qMsg,string QName,Made4Net.Shared.QMsgSender qSender)
        {

           if (UseLogs)
                oLogger.Write(PrintMessageContent(qSender));

            try
            {
                qSender.Send(QName, qMsg.Label, "", System.Messaging.MessagePriority.Normal);
            }
            catch (Exception ex)
            {
                if (UseLogs)
                    oLogger.Write(ex.Message.ToString());

                throw new M4NQHandleException(ex.Message.ToString(),false );
            }
        }

        private void PushMsg(System.Messaging.Message qMsg,WMS.Logic.EVENTREG evreg,Made4Net.Shared.QMsgSender qSender)
        {

            if (UseLogs)
                oLogger.Write(PrintMessageContent(qSender));

            try
            {
                if(!evreg.SYNCHRONIZEDQUEUE)
                {
                    qSender.Send(evreg.REGQUEUENAME, qMsg.Label, "", System.Messaging.MessagePriority.Normal);
                }
                else
                {
                    Made4Net.Shared.SyncQMsgSender synSender = new SyncQMsgSender();
                    synSender.Values.Add(qSender.Values);
                    synSender.Send(evreg.REGQUEUENAME, qMsg.Label, System.Messaging.MessagePriority.Normal);
                }

            }
            catch(Exception ex)
            {
                if (UseLogs)
                    oLogger.Write(ex.Message.ToString() );

                throw new M4NQHandleException(ex.Message.ToString(),false );
            }
        }

        private bool IsAudit(string pAction)
        {
            if(String.IsNullOrEmpty(pAction))
            {
                return false;
            }

            bool ret = false;
            if (pAction.Equals(WMS.Lib.Actions.Audit.REQUESTPICKUP))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.CANCELPICK))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.CANPCKLIST))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.UNALLOCATEPICK))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.PCKPCKLIST))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.RELEASEWAVE))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.RECEIPTCLOSE))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SHPSHIP))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SETSTATUS))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.MOVELOAD))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.DELETELOAD))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.ORDERSHIP))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.COMPLETEWAVE))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.COMPLETEORDER))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.COMPLETESHIP))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SPLITCONTAINER))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SPLITPICK))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.CHANGEUOM))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SETORDSTAT))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.SETUOM))
            {
                ret = true;
            }
            else if (pAction.Equals(WMS.Lib.Actions.Audit.CANSHIP))
            {
                ret = true;
            }
            return ret;
        }

        protected new string PrintMessageContent(QMsgSender qSender)
        {
            // RWMS-1419 start
            string msgContent = "******************** MESSAGE CONTENT *****************";
            msgContent = msgContent + "\r\n";
            for (int i = 0; i < qSender.Values.Count - 1; i++)
            {
                msgContent = msgContent + "     " + qSender.Values[i] + "\r\n";
            }
            msgContent = msgContent + "******************** END OF MESSAGE CONTENT *****************" + "\r\n";
            return msgContent;
            // RWMS-1419 End
        }
    }
}
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Made4Net.DataAccess;
using Made4Net.Shared;
using WMS.Logic;
using System.Messaging;

namespace WMS.ScheduleTasks
{
    public class RePlanExceptions
    {
        public RePlanExceptions()
        {

        }

        public void RePlan()
        {
            Random random = new Random();
            Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile("d:\\SCEXPERT\\Logs\\Replan\\", "WSRP_" + random.Next(100000).ToString() + ".txt", true);
            _qhlogger.WriteSeperator();
            try
            {
                Made4Net.Shared.QMsgSender eq = new QMsgSender();
                eq.Values.Add("EVENT", "900");
                eq.Send("DispatchingControlQ", "Replan Waves","",System.Messaging.MessagePriority.Normal);
            }
            catch (M4NException m4nex)
            {
                _qhlogger.WriteLine("M4N Ex : " + m4nex.Description, true);
            }
            catch (Exception ex)
            {
                _qhlogger.WriteLine("SYS Ex : " + ex.Message, true);
            }
            _qhlogger.WriteSeperator();
            _qhlogger.Close();



            //Random random = new Random();
            //Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile("d:\\SCEXPERT\\Logs\\Replan\\", "WSRP_" + random.Next(100000).ToString() + ".txt", true);
            //_qhlogger.WriteSeperator();

            //try
            //{
            //    string waves = "SELECT DISTINCT WAVE FROM WAVEEXCEPTION WHERE  wave <>''";
            //    _qhlogger.WriteLine(waves, true);
            //    DataTable dt = new DataTable();
            //    DataInterface.FillDataset(waves, dt, false, "");
            //    _qhlogger.WriteLine("Found " + dt.Rows.Count + " waves with exception", true);
            //    foreach (DataRow wdr in dt.Rows)
            //    {
            //        Wave ws = new Wave(Convert.ToString(wdr["WAVE"]), true);
            //        if (ws.STATUS != "PLANNING")
            //        {
            //            _qhlogger.WriteLine("Starting to replan wave : " + ws.WAVE, true);
            //            ws.Plan(true, "SCHEDULER", false);
            //            _qhlogger.WriteLine("Finished to replan wave : " + ws.WAVE, true);
            //        }
            //    }
            //}
            //catch (M4NException m4nex)
            //{
            //    _qhlogger.WriteLine("M4N Ex : " + m4nex.Description, true);
            //}
            //catch (Exception ex)
            //{
            //    _qhlogger.WriteLine("SYS Ex : " + ex.Message , true);
            //}
            //_qhlogger.WriteSeperator();
            //_qhlogger.Stream.Close();
        }
    }
}
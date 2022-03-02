using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using Made4Net.Shared;
using WMS.Logic;
using System.Data;

namespace PicklistBreak
{
   public class PicklistBreakHandler
    {
        //PicklistBreakHandler() { }


        public void PicklistBreak(string wave, string USERID)
        {
            string picklist, newpl;
            decimal breakVoluem, lineVol, picklistline, curVol;
            string vName = Made4Net.Shared.AppConfig.Get("viewPicklistBreakHeaderName", null);
            //
            string sql = "select picklist, breakVoluem from {1} where wave ='{0}' order by picklist";

            UpdateWaveStatus(wave, USERID, WMS.Lib.Statuses.Wave.PLANNING.ToString());

            DataTable dt = new DataTable();
         
            sql = string.Format(sql, wave, vName);
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, false, "");
            vName = Made4Net.Shared.AppConfig.Get("viewPicklistBreakDetName", null);

            foreach (DataRow dr in dt.Rows)
            {
                sql = "select picklistline, lineVoluem from {1} where picklist ='{0}' order by picklistline";
                picklist = dr["picklist"].ToString();
                breakVoluem = decimal.Parse(dr["breakVoluem"].ToString());
                DataTable dtPL = new DataTable();

                curVol = 0;
                newpl = "";
                sql = string.Format(sql, picklist, vName);
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dtPL, false, "");
                foreach (DataRow drPL in dtPL.Rows)
                {
                    picklistline = decimal.Parse(drPL["picklistline"].ToString());
                    lineVol = decimal.Parse(drPL["lineVoluem"].ToString());
                    SplitePicklist(picklist, picklistline, breakVoluem, lineVol, ref curVol, ref newpl, USERID);
                }
            }

            UpdateWaveStatus(wave, USERID, WMS.Lib.Statuses.Wave.PLANNED);
        }


        private void SplitePicklist(string pl, decimal line, decimal breakVoluem, decimal lineVol, ref decimal curVol, ref string newpl, string user)
        {
            if (curVol == 0)//then first line of pl, allways stay
            {
                curVol = lineVol;
                return;
            }
            if (curVol + lineVol > breakVoluem)
            {
                curVol = lineVol;
                newpl = Made4Net.Shared.Util.getNextCounter("PICKLIST");//set new PL
                ClonePicklistHeader(pl, newpl, user);
            }
            else curVol = curVol + lineVol;

            if (newpl != "")//move line to new PL
            {
                MovePicklistLine(pl, line, newpl, user);
            }//else -> line stay in source pl

        }

        private void MovePicklistLine(string fromPl, decimal fromLine, string toPl, string user)
        {
            decimal toLine = getNextLine(toPl);
           
            MovePicklistDetail(fromPl, fromLine, toPl, toLine, user);
        }

        private void MovePicklistDetail(string fromPl, decimal fromLine, string toPl, decimal toLine, string user)
        {
            string sql = string.Format("update PICKDETAIL set  PICKLIST='{0}', PICKLISTLINE = '{1}', EDITDATE=GETDATE(),EDITUSER='{2}'  where PICKLIST='{3}' and PICKLISTLINE = '{4}'", toPl, toLine, user, fromPl, fromLine);

            Made4Net.DataAccess.DataInterface.RunSQL(sql);
        }

        private void ClonePicklistHeader(string fromPl, string toPl, string user)
        {
            string sql = string.Format("insert into PICKHEADER SELECT '{0}', PICKTYPE, PICKMETHOD, STRATEGYID, CREATEDATE, PLANDATE, RELEASEDATE, ASSIGNEDDATE, COMPLETEDDATE, STATUS, WAVE, HANDELINGUNITTYPE, ADDDATE, ADDUSER, GETDATE(), '{1}' FROM PICKHEADER  where PICKLIST='{2}'", toPl, user, fromPl);

            Made4Net.DataAccess.DataInterface.RunSQL(sql);
        }


        private decimal getNextLine(string Pl)
        {
            string sql = string.Format("select count(1)+1 from PICKDETAIL where picklist='{0}'", Pl);
            return decimal.Parse(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql).ToString());
        }

        private void UpdateWaveStatus(string wave, string userid, string status)
        {
            string sql = "update wave set status = '{0}', edituser = '{1}', editdate = getdate() where wave = '{2}'";
            sql = string.Format(sql, status, userid, wave);
            try
            {
                Made4Net.DataAccess.DataInterface.RunSQL(sql);
            }
            catch { }
        }

      
    }
}

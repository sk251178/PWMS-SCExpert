using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Made4Net.DataAccess;
using Made4Net.Net.TCPIP;
using Made4Net.Shared;

namespace EmployeeInfoViewer
{
    public partial class EmployeeViewer : Form
    {
        private string _userID;
        private string _warehouse;
        private string _defaultShift;
        private string _shiftID;
        private string _clockInOut;
        private string _shiftStatus;
        private string _shiftDecription;
        private Timer _logoutTimer;
        private string _employeeName;
        private string _mheID;
        private string _heType;
        private BindingList<WMS.Logic.Task> _tasksList;
        private string _dbUserID;
        private bool _connected;
        
        
        public EmployeeViewer()
        {
            InitializeComponent();
            try
            {
                _dbUserID = System.Configuration.ConfigurationManager.AppSettings.Get("LicenseUserId"); 

                if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ShowPasswordField")))
                {
                    labelPassword.Visible = false;
                    textBoxPassword.Visible = false;
                }
                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("PrefGridIsDisplayed")))
                    dataGridViewSummaryPref.Visible = true;

                if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("PrefReportButtonEnabled")))
                    buttonPerfReport.Enabled = false;

                if (!bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("AllowCompleteTasks")))
                    buttonCompleteTasks.Enabled = false;
                disableControls();
                disableTaskControls();

                //if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ConnectToDB")))
                //{
                //    //_connected = Connect();
                //    //if (!_connected)
                //    //{
                //    //    //updatePreformanceSummaryGrid();

                //    //    MessageBox.Show("Could not establish connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //}
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                buttonLogin.Enabled = false;

            }
            
            _logoutTimer = new Timer();
            _logoutTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Timeout")) * 1000;
            _logoutTimer.Tick += new EventHandler(_logoutTimer_Tick);
            _tasksList = new BindingList<WMS.Logic.Task>();
            dataGridViewTasks.DataSource = _tasksList;

            dataGridViewPreformance.Visible = false;
            groupBoxTasks.Visible = true;
            ActiveControl = textBoxEmployeeID;
            this.AcceptButton = this.buttonLogin;
        }
        
        void _logoutTimer_Tick(object sender, EventArgs e)
        {
            logout();
        }

        private void clockIn()
        {
            if (_clockInOut.ToLower() == "in")
                return;
            try
            {
                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("AddUserToWHActivity")))
                    WMS.Logic.ShiftInstance.ClockUser(_userID, WMS.Lib.Shift.ClockStatus.IN, System.Configuration.ConfigurationManager.AppSettings.Get("Location"), "");
                else
                    WMS.Logic.ShiftInstance.ClockUser(_userID, WMS.Lib.Shift.ClockStatus.IN, "", "");

                updateClockInShiftStatus();
                if (_userID != string.Empty)
                {
                    buttonClockIn.Enabled = false;
                    buttonClockOut.Enabled = true;
                    buttonClockOutCompleteTasks.Enabled = true;
                    textBoxTaskID.Focus();
                }

                postToWHActivity();
                updateHeaderPanel();
                enableTaskControls();
                buttonClockIn.Enabled = false;
                buttonClockOut.Enabled = true;
                if (System.Configuration.ConfigurationManager.AppSettings.Get("ClockInMiscTaskSubType") != string.Empty)
                    createClockInTask();
                textBoxTaskID.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createClockInTask()
        {
            try
            {
                WMS.Logic.Task oTsk = new WMS.Logic.Task();

                oTsk.TASKTYPE = WMS.Lib.TASKTYPE.MISC;
                oTsk.TASKSUBTYPE = System.Configuration.ConfigurationManager.AppSettings.Get("ClockInMiscTaskSubType");
                oTsk.Create();
                oTsk.AssignUser(_userID, WMS.Lib.TASKASSIGNTYPE.MANUAL, "", -1);
                oTsk.Complete("");
            }
            catch { }
        }



        private void clockOut()
        {
            if (_shiftID != string.Empty)
            {
                try
                {
                    WMS.Logic.ShiftInstance.ClockUser(_userID, WMS.Lib.Shift.ClockStatus.OUT, "", "");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            updateClockInShiftStatus();
            updateHeaderPanel();
            buttonClockOut.Enabled = false;
            buttonClockOutCompleteTasks.Enabled = false;
            buttonClockIn.Enabled = true;
            disableTaskControls();
            buttonClockIn.Enabled = true;
            buttonClockOut.Enabled = false;
            if (System.Configuration.ConfigurationManager.AppSettings.Get("ClockOutMiscTaskSubType") != string.Empty)
                createClockOutTask();
        }

        private void createClockOutTask()
        {
            try
            {
                WMS.Logic.Task oTsk = new WMS.Logic.Task();

                oTsk.TASKTYPE = WMS.Lib.TASKTYPE.MISC;
                oTsk.TASKSUBTYPE = System.Configuration.ConfigurationManager.AppSettings.Get("ClockOutMiscTaskSubType");
                oTsk.Create();
                oTsk.AssignUser(_userID, WMS.Lib.TASKASSIGNTYPE.MANUAL, "", -1);
                oTsk.Complete("");
            }
            catch { }
        }

        private void enableTaskControls()
        {
            textBoxTaskID.Enabled = true;
            textBoxTaskID.Text = "";
            buttonClearTasks.Enabled = true;
            buttonAssignTasks.Enabled = true;
            buttonUnAssignTasks.Enabled = true;
        }

        private void disableTaskControls()
        {
            textBoxTaskID.Enabled = false;
            buttonClearTasks.Enabled = false;
            buttonAssignTasks.Enabled = false;
            buttonUnAssignTasks.Enabled = false;
        }

        private void disableControls()
        {
            buttonClockIn.Enabled = false;
            buttonClockOut.Enabled = false;
            buttonClockOutCompleteTasks.Enabled = false;
            buttonPerfReport.Enabled = false;
            buttonShowTaskAssignment.Enabled = false;
            panelLoggedInControls.Visible = false;
        }

        private void enableControls()
        {
            
            buttonPerfReport.Enabled = true;
            buttonShowTaskAssignment.Enabled = true;
            panelLoggedInControls.Visible = true;

        }

        private void getUserDetails()
        {
            if (textBoxEmployeeID.Text != string.Empty)
            {
                string sql = string.Format("select top(1)userid,defaultshift,warehouse from userwarehouse where userid='{0}'", textBoxEmployeeID.Text);
                DataTable dt = (DataTable)queryDB(sql, "Made4NetSchema");
                if (dt == null || dt.Rows.Count == 0)
                    return;

                _userID = dt.Rows[0]["UserID"].ToString();
                _warehouse = dt.Rows[0]["Warehouse"].ToString();
                _defaultShift = dt.Rows[0]["DefaultShift"].ToString();
                WMS.Logic.Warehouse.setCurrentWarehouse(_warehouse);
            }
        }

        private void getHEIDType()
        {
            string sql = string.Format("Select * from vUsersClockIn where userID='{0}'", textBoxEmployeeID.Text);
            DataTable dt = (DataTable)queryDB(sql, "default");
            if (dt.Rows.Count > 0)
            {
                                _mheID = dt.Rows[0]["MHEID"].ToString();
                _heType = dt.Rows[0]["HETYPE"].ToString();

            }
        }

        private void updateClockInShiftStatus()
        {
            string sql = string.Format("select top(1)shiftid,case clockinout when 1 then 'In' else 'Out' end as clockinout from SHIFTUSERCLOCKS where userid='{0}' order by clocktime desc", _userID);
            DataTable dt = (DataTable)queryDB(sql, "default");
            if (dt.Rows.Count == 0)
            {
                _clockInOut = "out";
                return;
            }

            _shiftID = WMS.Logic.ShiftInstance.getShihtIDbyUserID(_userID).ToString();
            _clockInOut = dt.Rows[0]["clockinout"].ToString();
             
            if (_shiftID != string.Empty)
            {
                sql = String.Format("select shiftdescription,status from shiftmaster inner join shift on shiftmaster.shiftcode = shift.shiftcode where shiftid='{0}'", _shiftID);
                dt = (DataTable) queryDB(sql, "default");
                if (dt.Rows.Count != 0)
                {
                    _shiftDecription = dt.Rows[0]["shiftdescription"].ToString();
                    _shiftStatus = dt.Rows[0]["status"].ToString();
                }
                else
                {
                    _shiftDecription = "";
                    _shiftStatus = "";
                }
            }

        }

        private void login()
        {
            if (_clockInOut.ToLower() == "in")
            {
                postToWHActivity();

                buttonClockIn.Enabled = false; 
                buttonClockOut.Enabled = true;
                buttonClockOutCompleteTasks.Enabled = true;
                enableTaskControls();
            }
            else
            {
                buttonClockOut.Enabled = false;
                buttonClockOutCompleteTasks.Enabled = false;
                buttonClockIn.Enabled = true;
                disableTaskControls();
            }

            updateHeaderPanel();
            updatePreformanceSummaryGrid();
            updateTasksGrid();

            panelLogin.Visible = false;
            groupBox1.Visible = true;
            dataGridViewPreformance.Visible = false;
            groupBoxTasks.Visible = true;
            enableControls();
            
            _logoutTimer.Start();
            this.AcceptButton = this.buttonAssignTasks;
            textBoxTaskID.Focus();
        }

        private void postToWHActivity()
        {
            WMS.Logic.WHActivity oWHActivity = new WMS.Logic.WHActivity();
            oWHActivity.ACTIVITY = "Login";
            oWHActivity.WAREHOUSEAREA = _warehouse;
            oWHActivity.USERID = _userID;
            oWHActivity.ACTIVITYTIME = DateTime.Now;
            oWHActivity.ADDDATE = DateTime.Now;
            oWHActivity.EDITDATE = DateTime.Now;
            oWHActivity.ADDUSER = _userID;
            oWHActivity.EDITUSER = _userID;
            oWHActivity.HANDLINGEQUIPMENTID = _mheID;

            //oWHActivity.LOCATION =;

            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("AddUserToWHActivity")))
                oWHActivity.LOCATION = System.Configuration.ConfigurationManager.AppSettings.Get("Location");
            
            oWHActivity.WAREHOUSEAREA = System.Configuration.ConfigurationManager.AppSettings.Get("WarehouseArea");
            //try
            //{
            //    WMS.Logic.WHActivity.SetShift(_userID, _shiftID, WMS.Lib.Shift.ClockStatus.IN, System.Configuration.ConfigurationManager.AppSettings.Get("Location"));
            //}
            //catch { }

            oWHActivity.Post();
        }

        private void updateHeaderPanel()
        {
            labelValueClockStatus.Text = _clockInOut;
            labelValueEmployeeID.Text = _userID;

            labelValueHEID.Text = _mheID;
            labeValueHEType.Text = _heType;
            
            if (_shiftID != string.Empty)
            {
                labelValueShiftCode.Text = _shiftDecription;
                labelValueShiftStatus.Text = _shiftStatus;
                
            }
            else
                labelValueShiftCode.Text = "";

            if (_employeeName == string.Empty || _employeeName == null)
            {
                string sql = string.Format("select firstname +' '+lastname from userprofile where userid='{0}'", _userID);
                object employeeName = DataInterface.ExecuteScalar(sql, Made4Net.Schema.Constants.CONNECTION_NAME );
                if (employeeName != null)
                {
                    _employeeName = employeeName.ToString();
                    labelValueEmployeeName.Text = _employeeName.ToString();
                }
            }
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("AddUserToWHActivity")))
            {
                labelLocation.Visible = true;
                object location = DataInterface.ExecuteScalar(string.Format("Select location from whactivity where userid='{0}'", _userID));
                if (location != null)
                    labelValueLocation.Text = location.ToString();
            }
        }

        private void logout()
        {
            panelLogin.Visible = true;
            groupBox1.Visible = false;

            

            _userID = "";
            updatePreformanceSummaryGrid();
            labelValueClockStatus.Text = "";
            labelValueEmployeeID.Text = "";
            labelValueHEID.Text = "";
            labelValueShiftCode.Text = "";
            labelValueShiftStatus.Text = "";
            labeValueHEType.Text = "";

            textBoxEmployeeID.Text = "";
            textBoxEmployeeID.Focus();
            disableControls();
            disableTaskControls();
            _logoutTimer.Stop();
            this.AcceptButton = null;
            dataGridViewSummaryPref.DataSource = null;
            _tasksList.Clear();
            dataGridViewTasks.DataSource = _tasksList;

            dataGridViewPreformance.Visible = false;
            groupBoxTasks.Visible = true;

            labelTasksErrors.Text = "";
            this.AcceptButton = this.buttonLogin;
        }

        private void assignTasks()
        {
            foreach (DataGridViewRow dr in dataGridViewTasks.Rows)
            {
                string task = dr.Cells["Task"].Value.ToString();
                WMS.Logic.Task taskObj = new WMS.Logic.Task(task, true);
                if (taskObj.STATUS.ToUpper() == WMS.Lib.Statuses.Task.AVAILABLE)
                    taskObj.AssignUser(_userID,WMS.Lib.TASKASSIGNTYPE.MANUAL, "", -1);
                //else if (taskObj.STATUS == WMS.Lib.Statuses.Task.ASSIGNED && taskObj.USERID != _userID)
                    //throw new System.Exception();
            }
        }

        private void unassignTasks()
        {
            foreach (DataGridViewRow dr in dataGridViewTasks.Rows)
            {
                string task = dr.Cells["Task"].Value.ToString();
                WMS.Logic.Task taskObj = new WMS.Logic.Task(task, true);
                if (taskObj.STATUS == WMS.Lib.Statuses.Task.ASSIGNED)
                    taskObj.DeAssignUser();
            }
        }

        private void completeDataGridTasks()
        {
            foreach (DataGridViewRow dr in dataGridViewTasks.Rows)
            {
                string task = dr.Cells["Task"].Value.ToString();
                WMS.Logic.Task taskObj = new WMS.Logic.Task(task, true);
                if (taskObj.STATUS == WMS.Lib.Statuses.Task.ASSIGNED)
                    taskObj.Complete("");
            }
            dataGridViewTasks.Rows.Clear();
        
        }

        private void completeUserAssignedTasks()
        {
            string sql = string.Format("Select task from tasks where status='Assigned' and UserID='{0}'", _userID);
            DataTable ds = (DataTable)queryDB(sql, "default");
            foreach (DataRow dr in ds.Rows)
            {
                string task = dr["Task"].ToString();
                WMS.Logic.Task taskObj = new WMS.Logic.Task(task, true);
                if (taskObj.STATUS == WMS.Lib.Statuses.Task.ASSIGNED)
                    taskObj.Complete("");
            }
            dataGridViewTasks.Rows.Clear();
        }

        private void textBoxEmployeeID_TextChanged(object sender, EventArgs e)
        {
            getUserDetails();
            if (_userID != "" && _userID != null)
            {
                updateClockInShiftStatus();
                getHEIDType();
                login();
            }
        }

        private void EmployeeViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
        }

        private void textBoxTaskID_TextChanged(object sender, EventArgs e)
        {
            _logoutTimer.Stop();
            _logoutTimer.Start();
            
            if (textBoxTaskID.Text != "")
            {
                labelTasksErrors.Text = "";
                WMS.Logic.Task tmpTask;
                try
                {
                    tmpTask = new WMS.Logic.Task(textBoxTaskID.Text, true);
                    WMS.Logic.Task a = new WMS.Logic.Task();
                    if (tmpTask.STATUS == WMS.Lib.Statuses.Task.ASSIGNED && tmpTask.USERID != _userID)
                    {
                        labelTasksErrors.Text = "Task Assigned To Another User.";
                        labelTasksErrors.ForeColor = Color.Red;
                        textBoxTaskID.Clear();
                        return;
                    }
                    if (tmpTask.STATUS == WMS.Lib.Statuses.Task.COMPLETE)
                    {
                        labelTasksErrors.Text = "Task was already completed.";
                        labelTasksErrors.ForeColor = Color.Red;
                        textBoxTaskID.Clear();
                        return;
                    }
                    if (tmpTask.STATUS == WMS.Lib.Statuses.Task.CANCELED)
                    {
                        labelTasksErrors.Text = "Task was cancelled.";
                        labelTasksErrors.ForeColor = Color.Red;
                        textBoxTaskID.Clear();
                        return;
                    }
                    foreach (WMS.Logic.Task tsk in _tasksList)
                    {
                        if (tsk.TASK == tmpTask.TASK)
                        {
                            labelTasksErrors.Text = "Task Already Exists.";
                            labelTasksErrors.ForeColor = Color.Red;
                            textBoxTaskID.Clear();
                            return;
                        }
                    }
                    _tasksList.Add(tmpTask);
                    dataGridViewTasks.DataSource = _tasksList;
                }
                catch
                {
                    labelTasksErrors.Text = "Task Doesn't Exist.";
                    return;
                }
                textBoxTaskID.Clear();
            }
        }
        
        private void updatePreformanceSummaryGrid()
        {
            string sql = String.Format("select CASE indirectflag WHEN 0 THEN 'Direct' ELSE 'Indirect' END AS DirectInDirect, AVG(shiftPerformance) AS ShiftPerformance, SUM(StdTime) AS StdTime, SUM(TaskActualTime) AS ActualTime, SUM(TaskDelaysTime) AS DelayTime from vShiftTaskPerformance where userid='{0}' and shiftid='{1}' group by indirectflag", _userID, _shiftID);
            DataTable dt = (DataTable)queryDB(sql, "default");
            if (_userID != null && _userID != string.Empty && dt.Rows.Count == 0)
            {
                dt.Rows.Add("Indirect", 0, 0, 0, 0);
                dt.Rows.Add("Direct", 0, 0, 0, 0);
            }
            if (dt != null && dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["DirectIndirect"].ToString() == "Direct")
                    dt.Rows.Add("Indirect", 0, 0, 0, 0);
                else
                    dt.Rows.Add("Direct", 0, 0, 0, 0);
            }
            dataGridViewSummaryPref.DataSource = dt;
        }

        private void updatePreformanceGrid()
        {
            dataGridViewPreformance.Visible = true;
            groupBoxTasks.Visible = false;
            string sql = String.Format("select CASE indirectflag WHEN 0 THEN 'Direct' ELSE 'Indirect' END AS DirectInDirect, shiftPerformance AS ShiftPerformance, StdTime AS StdTime, TaskActualTime AS ActualTime, TaskDelaysTime AS DelayTime from vShiftTaskPerformance where userid='{0}' and shiftid='{1}'", _userID, _shiftID);
            DataTable dt = (DataTable)queryDB(sql, "default");
            dataGridViewPreformance.DataSource = dt;
        }

        private void updateTasksGrid()
        {
            string sql = string.Format("select task from tasks where userid='{0}' and status='assigned'",_userID );
            DataTable dt = (DataTable)queryDB(sql, "default");

            WMS.Logic.Task tmpTask;
            foreach (DataRow row in dt.Rows )
            {
                tmpTask = new WMS.Logic.Task(row["Task"].ToString(), true);
                bool existsInTaskList = false;
                foreach (WMS.Logic.Task tsk in _tasksList)
                {
                    if (tsk.TASK == tmpTask.TASK)
                    {
                        existsInTaskList = true;
                        break;
                    }
                }
                if (!existsInTaskList)
                    _tasksList.Add(tmpTask);
            }


        }

        #region "Buttons Clicks"
        
        private void buttonClearTasks_Click(object sender, EventArgs e)
        {
            _logoutTimer.Stop();
            _logoutTimer.Start();
            if (dataGridViewTasks.Rows.Count == 0)
            {
                labelTasksErrors.ForeColor = Color.Red;
                labelTasksErrors.Text = "Tasks Table Is Empty.";
                return;
            }
           
            dataGridViewTasks.Rows.Clear();
            labelTasksErrors.ForeColor = Color.Black;
            labelTasksErrors.Text = "Cleared Tasks Table.";
            textBoxTaskID.Text = "";
        }

        private void buttonPerfReport_Click(object sender, EventArgs e)
        {
            updatePreformanceGrid();
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonClockOut_Click(object sender, EventArgs e)
        {
            clockOut();
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonClockOutCompleteTasks_Click(object sender, EventArgs e)
        {
            completeUserAssignedTasks();
            clockOut();
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonClockIn_Click(object sender, EventArgs e)
        {
            clockIn();
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CompleteTasksUponClockIn")))
                completeUserAssignedTasks();
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonAssignTasks_Click(object sender, EventArgs e)
        {
            _logoutTimer.Stop();
            _logoutTimer.Start();
            if (dataGridViewTasks.Rows.Count == 0)
            {
                labelTasksErrors.ForeColor = Color.Red;
                labelTasksErrors.Text = "No Tasks To Assign.";
                return;
            }
            try
            {
                assignTasks();
            }
            catch
            {
                labelTasksErrors.ForeColor = Color.Red;
                labelTasksErrors.Text = "An Entered Task Was Already Assigned.";
            }
            labelTasksErrors.ForeColor = Color.Black;
            labelTasksErrors.Text = "Entered Tasks Were Successfully Assigned.";

        }

        private void buttonUnAssignTasks_Click(object sender, EventArgs e)
        {
            unassignTasks();
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonCompleteTasks_Click(object sender, EventArgs e)
        {
            _logoutTimer.Stop();
            _logoutTimer.Start();
            if (dataGridViewTasks.Rows.Count == 0)
            {
                labelTasksErrors.ForeColor = Color.Red;
                labelTasksErrors.Text = "No Tasks To Complete.";
                return;
            }
            completeDataGridTasks();
            labelTasksErrors.ForeColor = Color.Black;
            labelTasksErrors.Text = "Entered Tasks Were Completed.";

        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void buttonShowTaskAssignment_Click(object sender, EventArgs e)
        {
            dataGridViewPreformance.Visible = false;
            groupBoxTasks.Visible = true;
            _logoutTimer.Stop();
            _logoutTimer.Start();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (!_connected)
                _connected = Connect();

            if (_connected)
            {
                getUserDetails();
                if (_userID != "" && _userID != null)
                {
                    this.AcceptButton = null;
                    try
                    {
                        updateClockInShiftStatus();
                        getHEIDType();
                        login();
                    }
                    catch
                    {
                        MessageBox.Show("Could not login. Please check the database tables.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Could not login. User does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("Could not establish connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region "DB"
        protected bool Connect()
        {
            bool res = false;
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ConnectToDB")))
            {
                string LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
                try
                {
                    res = Made4Net.DataAccess.DataInterface.Connect(LicenseUserId);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return res;
            
        }

        protected bool Disconnect()
        {
            try
            {
                string LicenseUserId = Made4Net.Shared.AppConfig.Get("LicenseUserId", null);
                Made4Net.DataAccess.DataInterface.Disconnect(LicenseUserId);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        private object queryDB(string pSql, string db)
        {
            try
            {
                DataTable ds = new DataTable();
                DataInterface.FillDataset(pSql, ds, false, db);
                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

#endregion

        private void dataGridViewTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }








    }
}
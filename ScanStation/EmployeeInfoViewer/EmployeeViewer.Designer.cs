namespace EmployeeInfoViewer
{
    partial class EmployeeViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeViewer));
            this.panelLogin = new System.Windows.Forms.Panel();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxEmployeeID = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelEmployeeID = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelValueLocation = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.labelClockStatusDetails = new System.Windows.Forms.Label();
            this.labelHandlingEquipmentIDDetails = new System.Windows.Forms.Label();
            this.labelHandlingEquipmentTypeDetails = new System.Windows.Forms.Label();
            this.labeValueHEType = new System.Windows.Forms.Label();
            this.labelValueHEID = new System.Windows.Forms.Label();
            this.labelValueClockStatus = new System.Windows.Forms.Label();
            this.labelValueShiftStatus = new System.Windows.Forms.Label();
            this.labelValueShiftCode = new System.Windows.Forms.Label();
            this.labelShiftCode = new System.Windows.Forms.Label();
            this.labelShiftStatusDetails = new System.Windows.Forms.Label();
            this.labelValueEmployeeName = new System.Windows.Forms.Label();
            this.labelValueEmployeeID = new System.Windows.Forms.Label();
            this.labelEmployeeName = new System.Windows.Forms.Label();
            this.labelEmployeeIDDetails = new System.Windows.Forms.Label();
            this.dataGridViewSummaryPref = new System.Windows.Forms.DataGridView();
            this.buttonClockOutCompleteTasks = new System.Windows.Forms.Button();
            this.buttonClockOut = new System.Windows.Forms.Button();
            this.buttonClockIn = new System.Windows.Forms.Button();
            this.dataGridViewTasks = new System.Windows.Forms.DataGridView();
            this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskSubType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromWaehouseArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StdTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelTaskButtons = new System.Windows.Forms.Panel();
            this.labelTasksErrors = new System.Windows.Forms.Label();
            this.labelTaskID = new System.Windows.Forms.Label();
            this.textBoxTaskID = new System.Windows.Forms.TextBox();
            this.buttonCompleteTasks = new System.Windows.Forms.Button();
            this.buttonAssignTasks = new System.Windows.Forms.Button();
            this.buttonClearTasks = new System.Windows.Forms.Button();
            this.buttonUnAssignTasks = new System.Windows.Forms.Button();
            this.groupBoxTasks = new System.Windows.Forms.GroupBox();
            this.dataGridViewPreformance = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonShowTaskAssignment = new System.Windows.Forms.Button();
            this.buttonLogOut = new System.Windows.Forms.Button();
            this.buttonPerfReport = new System.Windows.Forms.Button();
            this.panelLoggedInControls = new System.Windows.Forms.Panel();
            this.panelLogin.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummaryPref)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taskBindingSource)).BeginInit();
            this.panelTaskButtons.SuspendLayout();
            this.groupBoxTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreformance)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelLoggedInControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.textBoxPassword);
            this.panelLogin.Controls.Add(this.buttonLogin);
            this.panelLogin.Controls.Add(this.textBoxEmployeeID);
            this.panelLogin.Controls.Add(this.labelPassword);
            this.panelLogin.Controls.Add(this.labelEmployeeID);
            this.panelLogin.Location = new System.Drawing.Point(145, 11);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(696, 191);
            this.panelLogin.TabIndex = 0;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(380, 79);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(123, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonLogin.Location = new System.Drawing.Point(287, 120);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(117, 54);
            this.buttonLogin.TabIndex = 2;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxEmployeeID
            // 
            this.textBoxEmployeeID.Location = new System.Drawing.Point(380, 36);
            this.textBoxEmployeeID.Name = "textBoxEmployeeID";
            this.textBoxEmployeeID.Size = new System.Drawing.Size(123, 20);
            this.textBoxEmployeeID.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPassword.Location = new System.Drawing.Point(221, 79);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(92, 24);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "Password";
            // 
            // labelEmployeeID
            // 
            this.labelEmployeeID.AutoSize = true;
            this.labelEmployeeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelEmployeeID.Location = new System.Drawing.Point(195, 32);
            this.labelEmployeeID.Name = "labelEmployeeID";
            this.labelEmployeeID.Size = new System.Drawing.Size(118, 24);
            this.labelEmployeeID.TabIndex = 0;
            this.labelEmployeeID.Text = "Employee ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelValueLocation);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Controls.Add(this.labelClockStatusDetails);
            this.groupBox1.Controls.Add(this.labelHandlingEquipmentIDDetails);
            this.groupBox1.Controls.Add(this.labelHandlingEquipmentTypeDetails);
            this.groupBox1.Controls.Add(this.labeValueHEType);
            this.groupBox1.Controls.Add(this.labelValueHEID);
            this.groupBox1.Controls.Add(this.labelValueClockStatus);
            this.groupBox1.Controls.Add(this.labelValueShiftStatus);
            this.groupBox1.Controls.Add(this.labelValueShiftCode);
            this.groupBox1.Controls.Add(this.labelShiftCode);
            this.groupBox1.Controls.Add(this.labelShiftStatusDetails);
            this.groupBox1.Controls.Add(this.labelValueEmployeeName);
            this.groupBox1.Controls.Add(this.labelValueEmployeeID);
            this.groupBox1.Controls.Add(this.labelEmployeeName);
            this.groupBox1.Controls.Add(this.labelEmployeeIDDetails);
            this.groupBox1.Location = new System.Drawing.Point(78, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(836, 156);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // labelValueLocation
            // 
            this.labelValueLocation.AutoSize = true;
            this.labelValueLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueLocation.Location = new System.Drawing.Point(568, 113);
            this.labelValueLocation.Name = "labelValueLocation";
            this.labelValueLocation.Size = new System.Drawing.Size(0, 20);
            this.labelValueLocation.TabIndex = 2;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelLocation.Location = new System.Drawing.Point(438, 110);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(95, 24);
            this.labelLocation.TabIndex = 1;
            this.labelLocation.Text = "Location:";
            this.labelLocation.Visible = false;
            // 
            // labelClockStatusDetails
            // 
            this.labelClockStatusDetails.AutoSize = true;
            this.labelClockStatusDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelClockStatusDetails.Location = new System.Drawing.Point(438, 84);
            this.labelClockStatusDetails.Name = "labelClockStatusDetails";
            this.labelClockStatusDetails.Size = new System.Drawing.Size(130, 24);
            this.labelClockStatusDetails.TabIndex = 0;
            this.labelClockStatusDetails.Text = "Clock Status:";
            // 
            // labelHandlingEquipmentIDDetails
            // 
            this.labelHandlingEquipmentIDDetails.AutoSize = true;
            this.labelHandlingEquipmentIDDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelHandlingEquipmentIDDetails.Location = new System.Drawing.Point(438, 54);
            this.labelHandlingEquipmentIDDetails.Name = "labelHandlingEquipmentIDDetails";
            this.labelHandlingEquipmentIDDetails.Size = new System.Drawing.Size(180, 24);
            this.labelHandlingEquipmentIDDetails.TabIndex = 0;
            this.labelHandlingEquipmentIDDetails.Text = "Handling Equi. ID:";
            // 
            // labelHandlingEquipmentTypeDetails
            // 
            this.labelHandlingEquipmentTypeDetails.AutoSize = true;
            this.labelHandlingEquipmentTypeDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelHandlingEquipmentTypeDetails.Location = new System.Drawing.Point(438, 23);
            this.labelHandlingEquipmentTypeDetails.Name = "labelHandlingEquipmentTypeDetails";
            this.labelHandlingEquipmentTypeDetails.Size = new System.Drawing.Size(208, 24);
            this.labelHandlingEquipmentTypeDetails.TabIndex = 0;
            this.labelHandlingEquipmentTypeDetails.Text = "Handling Equi. Type:";
            // 
            // labeValueHEType
            // 
            this.labeValueHEType.AutoSize = true;
            this.labeValueHEType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labeValueHEType.Location = new System.Drawing.Point(649, 27);
            this.labeValueHEType.Name = "labeValueHEType";
            this.labeValueHEType.Size = new System.Drawing.Size(0, 20);
            this.labeValueHEType.TabIndex = 0;
            // 
            // labelValueHEID
            // 
            this.labelValueHEID.AutoSize = true;
            this.labelValueHEID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueHEID.Location = new System.Drawing.Point(632, 54);
            this.labelValueHEID.Name = "labelValueHEID";
            this.labelValueHEID.Size = new System.Drawing.Size(0, 20);
            this.labelValueHEID.TabIndex = 0;
            // 
            // labelValueClockStatus
            // 
            this.labelValueClockStatus.AutoSize = true;
            this.labelValueClockStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueClockStatus.Location = new System.Drawing.Point(632, 87);
            this.labelValueClockStatus.Name = "labelValueClockStatus";
            this.labelValueClockStatus.Size = new System.Drawing.Size(0, 20);
            this.labelValueClockStatus.TabIndex = 0;
            // 
            // labelValueShiftStatus
            // 
            this.labelValueShiftStatus.AutoSize = true;
            this.labelValueShiftStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueShiftStatus.Location = new System.Drawing.Point(177, 110);
            this.labelValueShiftStatus.Name = "labelValueShiftStatus";
            this.labelValueShiftStatus.Size = new System.Drawing.Size(0, 20);
            this.labelValueShiftStatus.TabIndex = 0;
            // 
            // labelValueShiftCode
            // 
            this.labelValueShiftCode.AutoSize = true;
            this.labelValueShiftCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueShiftCode.Location = new System.Drawing.Point(177, 82);
            this.labelValueShiftCode.Name = "labelValueShiftCode";
            this.labelValueShiftCode.Size = new System.Drawing.Size(0, 20);
            this.labelValueShiftCode.TabIndex = 0;
            // 
            // labelShiftCode
            // 
            this.labelShiftCode.AutoSize = true;
            this.labelShiftCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelShiftCode.Location = new System.Drawing.Point(39, 79);
            this.labelShiftCode.Name = "labelShiftCode";
            this.labelShiftCode.Size = new System.Drawing.Size(112, 24);
            this.labelShiftCode.TabIndex = 0;
            this.labelShiftCode.Text = "Shift Code:";
            // 
            // labelShiftStatusDetails
            // 
            this.labelShiftStatusDetails.AutoSize = true;
            this.labelShiftStatusDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelShiftStatusDetails.Location = new System.Drawing.Point(39, 106);
            this.labelShiftStatusDetails.Name = "labelShiftStatusDetails";
            this.labelShiftStatusDetails.Size = new System.Drawing.Size(118, 24);
            this.labelShiftStatusDetails.TabIndex = 0;
            this.labelShiftStatusDetails.Text = "Shift Status:";
            // 
            // labelValueEmployeeName
            // 
            this.labelValueEmployeeName.AutoSize = true;
            this.labelValueEmployeeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueEmployeeName.Location = new System.Drawing.Point(210, 55);
            this.labelValueEmployeeName.Name = "labelValueEmployeeName";
            this.labelValueEmployeeName.Size = new System.Drawing.Size(0, 20);
            this.labelValueEmployeeName.TabIndex = 0;
            // 
            // labelValueEmployeeID
            // 
            this.labelValueEmployeeID.AutoSize = true;
            this.labelValueEmployeeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelValueEmployeeID.Location = new System.Drawing.Point(177, 23);
            this.labelValueEmployeeID.Name = "labelValueEmployeeID";
            this.labelValueEmployeeID.Size = new System.Drawing.Size(0, 20);
            this.labelValueEmployeeID.TabIndex = 0;
            // 
            // labelEmployeeName
            // 
            this.labelEmployeeName.AutoSize = true;
            this.labelEmployeeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelEmployeeName.Location = new System.Drawing.Point(39, 52);
            this.labelEmployeeName.Name = "labelEmployeeName";
            this.labelEmployeeName.Size = new System.Drawing.Size(171, 24);
            this.labelEmployeeName.TabIndex = 0;
            this.labelEmployeeName.Text = "Employee Name:";
            // 
            // labelEmployeeIDDetails
            // 
            this.labelEmployeeIDDetails.AutoSize = true;
            this.labelEmployeeIDDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelEmployeeIDDetails.Location = new System.Drawing.Point(39, 20);
            this.labelEmployeeIDDetails.Name = "labelEmployeeIDDetails";
            this.labelEmployeeIDDetails.Size = new System.Drawing.Size(135, 24);
            this.labelEmployeeIDDetails.TabIndex = 0;
            this.labelEmployeeIDDetails.Text = "Employee ID:";
            // 
            // dataGridViewSummaryPref
            // 
            this.dataGridViewSummaryPref.AllowUserToAddRows = false;
            this.dataGridViewSummaryPref.AllowUserToDeleteRows = false;
            this.dataGridViewSummaryPref.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSummaryPref.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSummaryPref.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSummaryPref.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSummaryPref.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewSummaryPref.Location = new System.Drawing.Point(66, 3);
            this.dataGridViewSummaryPref.Name = "dataGridViewSummaryPref";
            this.dataGridViewSummaryPref.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSummaryPref.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSummaryPref.RowHeadersVisible = false;
            this.dataGridViewSummaryPref.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSummaryPref.Size = new System.Drawing.Size(518, 67);
            this.dataGridViewSummaryPref.TabIndex = 0;
            // 
            // buttonClockOutCompleteTasks
            // 
            this.buttonClockOutCompleteTasks.Location = new System.Drawing.Point(227, 12);
            this.buttonClockOutCompleteTasks.Name = "buttonClockOutCompleteTasks";
            this.buttonClockOutCompleteTasks.Size = new System.Drawing.Size(98, 48);
            this.buttonClockOutCompleteTasks.TabIndex = 2;
            this.buttonClockOutCompleteTasks.Text = "Clock Out And Complete Tasks";
            this.buttonClockOutCompleteTasks.UseVisualStyleBackColor = true;
            this.buttonClockOutCompleteTasks.Click += new System.EventHandler(this.buttonClockOutCompleteTasks_Click);
            // 
            // buttonClockOut
            // 
            this.buttonClockOut.Location = new System.Drawing.Point(117, 12);
            this.buttonClockOut.Name = "buttonClockOut";
            this.buttonClockOut.Size = new System.Drawing.Size(98, 48);
            this.buttonClockOut.TabIndex = 1;
            this.buttonClockOut.Text = "Clock Out";
            this.buttonClockOut.UseVisualStyleBackColor = true;
            this.buttonClockOut.Click += new System.EventHandler(this.buttonClockOut_Click);
            // 
            // buttonClockIn
            // 
            this.buttonClockIn.Location = new System.Drawing.Point(6, 12);
            this.buttonClockIn.Name = "buttonClockIn";
            this.buttonClockIn.Size = new System.Drawing.Size(98, 48);
            this.buttonClockIn.TabIndex = 0;
            this.buttonClockIn.Text = "Clock In";
            this.buttonClockIn.UseVisualStyleBackColor = true;
            this.buttonClockIn.Click += new System.EventHandler(this.buttonClockIn_Click);
            // 
            // dataGridViewTasks
            // 
            this.dataGridViewTasks.AllowUserToAddRows = false;
            this.dataGridViewTasks.AllowUserToDeleteRows = false;
            this.dataGridViewTasks.AutoGenerateColumns = false;
            this.dataGridViewTasks.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Task,
            this.TaskType,
            this.TaskSubType,
            this.Priority,
            this.FromWaehouseArea,
            this.FromLocation,
            this.StdTime});
            this.dataGridViewTasks.DataSource = this.taskBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTasks.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTasks.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewTasks.Name = "dataGridViewTasks";
            this.dataGridViewTasks.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTasks.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTasks.RowHeadersVisible = false;
            this.dataGridViewTasks.RowHeadersWidth = 70;
            this.dataGridViewTasks.Size = new System.Drawing.Size(641, 150);
            this.dataGridViewTasks.TabIndex = 1;
            this.dataGridViewTasks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTasks_CellContentClick);
            // 
            // Task
            // 
            this.Task.DataPropertyName = "TASK";
            this.Task.HeaderText = "TASK";
            this.Task.Name = "Task";
            this.Task.ReadOnly = true;
            this.Task.Width = 90;
            // 
            // TaskType
            // 
            this.TaskType.DataPropertyName = "TASKTYPE";
            this.TaskType.HeaderText = "TASKTYPE";
            this.TaskType.Name = "TaskType";
            this.TaskType.ReadOnly = true;
            this.TaskType.Width = 75;
            // 
            // TaskSubType
            // 
            this.TaskSubType.DataPropertyName = "TASKSUBTYPE";
            this.TaskSubType.HeaderText = "TASKSUBTYPE";
            this.TaskSubType.Name = "TaskSubType";
            this.TaskSubType.ReadOnly = true;
            // 
            // Priority
            // 
            this.Priority.DataPropertyName = "PRIORITY";
            this.Priority.HeaderText = "PRIORITY";
            this.Priority.Name = "Priority";
            this.Priority.ReadOnly = true;
            this.Priority.Width = 75;
            // 
            // FromWaehouseArea
            // 
            this.FromWaehouseArea.DataPropertyName = "FROMWAREHOUSEAREA";
            this.FromWaehouseArea.HeaderText = "FROMWAREHOUSEAREA";
            this.FromWaehouseArea.Name = "FromWaehouseArea";
            this.FromWaehouseArea.ReadOnly = true;
            this.FromWaehouseArea.Width = 110;
            // 
            // FromLocation
            // 
            this.FromLocation.DataPropertyName = "FROMLOCATION";
            this.FromLocation.HeaderText = "FROMLOCATION";
            this.FromLocation.Name = "FromLocation";
            this.FromLocation.ReadOnly = true;
            // 
            // StdTime
            // 
            this.StdTime.DataPropertyName = "STDTIME";
            this.StdTime.HeaderText = "STDTIME";
            this.StdTime.Name = "StdTime";
            this.StdTime.ReadOnly = true;
            this.StdTime.Width = 75;
            // 
            // taskBindingSource
            // 
            this.taskBindingSource.DataSource = typeof(WMS.Logic.Task);
            // 
            // panelTaskButtons
            // 
            this.panelTaskButtons.Controls.Add(this.labelTasksErrors);
            this.panelTaskButtons.Controls.Add(this.labelTaskID);
            this.panelTaskButtons.Controls.Add(this.textBoxTaskID);
            this.panelTaskButtons.Controls.Add(this.buttonCompleteTasks);
            this.panelTaskButtons.Controls.Add(this.buttonAssignTasks);
            this.panelTaskButtons.Controls.Add(this.buttonClearTasks);
            this.panelTaskButtons.Location = new System.Drawing.Point(13, 172);
            this.panelTaskButtons.Name = "panelTaskButtons";
            this.panelTaskButtons.Size = new System.Drawing.Size(874, 121);
            this.panelTaskButtons.TabIndex = 0;
            // 
            // labelTasksErrors
            // 
            this.labelTasksErrors.AutoSize = true;
            this.labelTasksErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelTasksErrors.ForeColor = System.Drawing.Color.Red;
            this.labelTasksErrors.Location = new System.Drawing.Point(5, 90);
            this.labelTasksErrors.Name = "labelTasksErrors";
            this.labelTasksErrors.Size = new System.Drawing.Size(0, 25);
            this.labelTasksErrors.TabIndex = 3;
            // 
            // labelTaskID
            // 
            this.labelTaskID.AutoSize = true;
            this.labelTaskID.Location = new System.Drawing.Point(6, 29);
            this.labelTaskID.Name = "labelTaskID";
            this.labelTaskID.Size = new System.Drawing.Size(45, 13);
            this.labelTaskID.TabIndex = 2;
            this.labelTaskID.Text = "Task ID";
            // 
            // textBoxTaskID
            // 
            this.textBoxTaskID.Location = new System.Drawing.Point(67, 26);
            this.textBoxTaskID.Name = "textBoxTaskID";
            this.textBoxTaskID.Size = new System.Drawing.Size(133, 20);
            this.textBoxTaskID.TabIndex = 1;
            this.textBoxTaskID.TextChanged += new System.EventHandler(this.textBoxTaskID_TextChanged);
            // 
            // buttonCompleteTasks
            // 
            this.buttonCompleteTasks.Location = new System.Drawing.Point(195, 79);
            this.buttonCompleteTasks.Name = "buttonCompleteTasks";
            this.buttonCompleteTasks.Size = new System.Drawing.Size(89, 36);
            this.buttonCompleteTasks.TabIndex = 0;
            this.buttonCompleteTasks.Text = "Complete";
            this.buttonCompleteTasks.UseVisualStyleBackColor = true;
            this.buttonCompleteTasks.Click += new System.EventHandler(this.buttonCompleteTasks_Click);
            // 
            // buttonAssignTasks
            // 
            this.buttonAssignTasks.Location = new System.Drawing.Point(100, 79);
            this.buttonAssignTasks.Name = "buttonAssignTasks";
            this.buttonAssignTasks.Size = new System.Drawing.Size(89, 36);
            this.buttonAssignTasks.TabIndex = 0;
            this.buttonAssignTasks.Text = "Assign";
            this.buttonAssignTasks.UseVisualStyleBackColor = true;
            this.buttonAssignTasks.Click += new System.EventHandler(this.buttonAssignTasks_Click);
            // 
            // buttonClearTasks
            // 
            this.buttonClearTasks.Location = new System.Drawing.Point(5, 79);
            this.buttonClearTasks.Name = "buttonClearTasks";
            this.buttonClearTasks.Size = new System.Drawing.Size(89, 36);
            this.buttonClearTasks.TabIndex = 0;
            this.buttonClearTasks.Text = "Clear";
            this.buttonClearTasks.UseVisualStyleBackColor = true;
            this.buttonClearTasks.Click += new System.EventHandler(this.buttonClearTasks_Click);
            // 
            // buttonUnAssignTasks
            // 
            this.buttonUnAssignTasks.Location = new System.Drawing.Point(0, 565);
            this.buttonUnAssignTasks.Name = "buttonUnAssignTasks";
            this.buttonUnAssignTasks.Size = new System.Drawing.Size(96, 36);
            this.buttonUnAssignTasks.TabIndex = 0;
            this.buttonUnAssignTasks.Text = "Unassign";
            this.buttonUnAssignTasks.UseVisualStyleBackColor = true;
            this.buttonUnAssignTasks.Visible = false;
            this.buttonUnAssignTasks.Click += new System.EventHandler(this.buttonUnAssignTasks_Click);
            // 
            // groupBoxTasks
            // 
            this.groupBoxTasks.Controls.Add(this.panelTaskButtons);
            this.groupBoxTasks.Controls.Add(this.dataGridViewTasks);
            this.groupBoxTasks.Location = new System.Drawing.Point(63, 166);
            this.groupBoxTasks.Name = "groupBoxTasks";
            this.groupBoxTasks.Size = new System.Drawing.Size(887, 299);
            this.groupBoxTasks.TabIndex = 3;
            this.groupBoxTasks.TabStop = false;
            // 
            // dataGridViewPreformance
            // 
            this.dataGridViewPreformance.AllowUserToAddRows = false;
            this.dataGridViewPreformance.AllowUserToDeleteRows = false;
            this.dataGridViewPreformance.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPreformance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewPreformance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPreformance.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewPreformance.Location = new System.Drawing.Point(80, 182);
            this.dataGridViewPreformance.Name = "dataGridViewPreformance";
            this.dataGridViewPreformance.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPreformance.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewPreformance.RowHeadersVisible = false;
            this.dataGridViewPreformance.Size = new System.Drawing.Size(504, 150);
            this.dataGridViewPreformance.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonClockIn);
            this.groupBox2.Controls.Add(this.buttonClockOutCompleteTasks);
            this.groupBox2.Controls.Add(this.buttonClockOut);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 70);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.groupBox2);
            this.panelButtons.Controls.Add(this.buttonShowTaskAssignment);
            this.panelButtons.Controls.Add(this.buttonLogOut);
            this.panelButtons.Controls.Add(this.buttonPerfReport);
            this.panelButtons.Location = new System.Drawing.Point(63, 73);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(887, 88);
            this.panelButtons.TabIndex = 1;
            // 
            // buttonShowTaskAssignment
            // 
            this.buttonShowTaskAssignment.Location = new System.Drawing.Point(495, 14);
            this.buttonShowTaskAssignment.Name = "buttonShowTaskAssignment";
            this.buttonShowTaskAssignment.Size = new System.Drawing.Size(98, 48);
            this.buttonShowTaskAssignment.TabIndex = 3;
            this.buttonShowTaskAssignment.Text = "Task Assignment";
            this.buttonShowTaskAssignment.UseVisualStyleBackColor = true;
            this.buttonShowTaskAssignment.Click += new System.EventHandler(this.buttonShowTaskAssignment_Click);
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonLogOut.Location = new System.Drawing.Point(738, 12);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(98, 48);
            this.buttonLogOut.TabIndex = 1;
            this.buttonLogOut.Text = "Logout";
            this.buttonLogOut.UseVisualStyleBackColor = true;
            this.buttonLogOut.Click += new System.EventHandler(this.buttonLogOut_Click);
            // 
            // buttonPerfReport
            // 
            this.buttonPerfReport.Location = new System.Drawing.Point(599, 14);
            this.buttonPerfReport.Name = "buttonPerfReport";
            this.buttonPerfReport.Size = new System.Drawing.Size(98, 48);
            this.buttonPerfReport.TabIndex = 3;
            this.buttonPerfReport.Text = "Performance Report";
            this.buttonPerfReport.UseVisualStyleBackColor = true;
            this.buttonPerfReport.Click += new System.EventHandler(this.buttonPerfReport_Click);
            // 
            // panelLoggedInControls
            // 
            this.panelLoggedInControls.Controls.Add(this.groupBoxTasks);
            this.panelLoggedInControls.Controls.Add(this.dataGridViewSummaryPref);
            this.panelLoggedInControls.Controls.Add(this.dataGridViewPreformance);
            this.panelLoggedInControls.Controls.Add(this.panelButtons);
            this.panelLoggedInControls.Location = new System.Drawing.Point(12, 171);
            this.panelLoggedInControls.Name = "panelLoggedInControls";
            this.panelLoggedInControls.Size = new System.Drawing.Size(953, 474);
            this.panelLoggedInControls.TabIndex = 4;
            this.panelLoggedInControls.Visible = false;
            // 
            // EmployeeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 646);
            this.Controls.Add(this.panelLoggedInControls);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonUnAssignTasks);
            this.Controls.Add(this.panelLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmployeeViewer";
            this.Text = "Scan Station";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EmployeeViewer_FormClosed);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummaryPref)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.taskBindingSource)).EndInit();
            this.panelTaskButtons.ResumeLayout(false);
            this.panelTaskButtons.PerformLayout();
            this.groupBoxTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreformance)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelLoggedInControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxEmployeeID;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelEmployeeID;
        private System.Windows.Forms.Button buttonClockOutCompleteTasks;
        private System.Windows.Forms.Button buttonClockOut;
        private System.Windows.Forms.Button buttonClockIn;
        private System.Windows.Forms.Panel panelTaskButtons;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private System.Windows.Forms.Button buttonCompleteTasks;
        private System.Windows.Forms.Button buttonUnAssignTasks;
        private System.Windows.Forms.Button buttonAssignTasks;
        private System.Windows.Forms.Button buttonClearTasks;
        private System.Windows.Forms.Label labelTaskID;
        private System.Windows.Forms.TextBox textBoxTaskID;
        private System.Windows.Forms.Label labelClockStatusDetails;
        private System.Windows.Forms.Label labelHandlingEquipmentIDDetails;
        private System.Windows.Forms.Label labelHandlingEquipmentTypeDetails;
        private System.Windows.Forms.Label labelShiftCode;
        private System.Windows.Forms.Label labelShiftStatusDetails;
        private System.Windows.Forms.Label labelEmployeeIDDetails;
        private System.Windows.Forms.Label labelValueClockStatus;
        private System.Windows.Forms.Label labelValueShiftStatus;
        private System.Windows.Forms.Label labelValueShiftCode;
        private System.Windows.Forms.Label labelValueEmployeeID;
        private System.Windows.Forms.Label labeValueHEType;
        private System.Windows.Forms.Label labelValueHEID;
        private System.Windows.Forms.BindingSource taskBindingSource;
        private System.Windows.Forms.DataGridView dataGridViewSummaryPref;
        private System.Windows.Forms.DataGridViewTextBoxColumn Task;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskSubType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromWaehouseArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn StdTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxTasks;
        private System.Windows.Forms.DataGridView dataGridViewPreformance;
        private System.Windows.Forms.Label labelValueEmployeeName;
        private System.Windows.Forms.Label labelEmployeeName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTasksErrors;
        private System.Windows.Forms.Label labelValueLocation;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonShowTaskAssignment;
        private System.Windows.Forms.Button buttonLogOut;
        private System.Windows.Forms.Button buttonPerfReport;
        private System.Windows.Forms.Panel panelLoggedInControls;
    }
}


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Messaging;
using Made4Net.Shared;

namespace MsgQUtils
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>


	public class MsgReaderForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpActionBtn;
		private System.Windows.Forms.GroupBox grpQSelect;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.ComboBox cbQue;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.TextBox txtMsgCont;

		public static string msgText ;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button Resend;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
        private Button ImportAllBtn;
        private Button ExportAllBtn;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
		public System.Messaging.Message vMessage;
        MessageQueue readQ;
        private Button ReadnRemove;
        private Button Edit;
        private Button sendEdited;
        string resendQName = string.Empty;
        private RadioButton RBResponseQueue;
        private RadioButton RBRequestQueue;
        private Button btnExportMsgs;
        Boolean processMessage = false;

		public MsgReaderForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.grpActionBtn = new System.Windows.Forms.GroupBox();
            this.sendEdited = new System.Windows.Forms.Button();
            this.Edit = new System.Windows.Forms.Button();
            this.ReadnRemove = new System.Windows.Forms.Button();
            this.ImportAllBtn = new System.Windows.Forms.Button();
            this.ExportAllBtn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Resend = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.grpQSelect = new System.Windows.Forms.GroupBox();
            this.cbQue = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMsgCont = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnExportMsgs = new System.Windows.Forms.Button();
            this.RBRequestQueue = new System.Windows.Forms.RadioButton();
            this.RBResponseQueue = new System.Windows.Forms.RadioButton();
            this.grpActionBtn.SuspendLayout();
            this.grpQSelect.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            //
            // grpActionBtn
            //
            this.grpActionBtn.Controls.Add(this.btnExportMsgs);
            this.grpActionBtn.Controls.Add(this.sendEdited);
            this.grpActionBtn.Controls.Add(this.Edit);
            this.grpActionBtn.Controls.Add(this.ReadnRemove);
            this.grpActionBtn.Controls.Add(this.ImportAllBtn);
            this.grpActionBtn.Controls.Add(this.ExportAllBtn);
            this.grpActionBtn.Controls.Add(this.button3);
            this.grpActionBtn.Controls.Add(this.button2);
            this.grpActionBtn.Controls.Add(this.Resend);
            this.grpActionBtn.Controls.Add(this.btnRead);
            this.grpActionBtn.Location = new System.Drawing.Point(557, 9);
            this.grpActionBtn.Name = "grpActionBtn";
            this.grpActionBtn.Size = new System.Drawing.Size(192, 495);
            this.grpActionBtn.TabIndex = 0;
            this.grpActionBtn.TabStop = false;
            this.grpActionBtn.Text = "Action Buttons";
            //
            // sendEdited
            //
            this.sendEdited.Location = new System.Drawing.Point(7, 400);
            this.sendEdited.Name = "sendEdited";
            this.sendEdited.Size = new System.Drawing.Size(173, 37);
            this.sendEdited.TabIndex = 8;
            this.sendEdited.Text = "Send Edited Message";
            this.sendEdited.Click += new System.EventHandler(this.sendEdited_Click);
            //
            // Edit
            //
            this.Edit.Location = new System.Drawing.Point(7, 303);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(173, 37);
            this.Edit.TabIndex = 7;
            this.Edit.Text = "Edit Message";
            this.Edit.Click += new System.EventHandler(this.button5_Click);
            //
            // ReadnRemove
            //
            this.ReadnRemove.Location = new System.Drawing.Point(7, 197);
            this.ReadnRemove.Name = "ReadnRemove";
            this.ReadnRemove.Size = new System.Drawing.Size(173, 37);
            this.ReadnRemove.TabIndex = 6;
            this.ReadnRemove.Text = "Read Message";
            this.ReadnRemove.Click += new System.EventHandler(this.button4_Click);
            //
            // ImportAllBtn
            //
            this.ImportAllBtn.Location = new System.Drawing.Point(10, 22);
            this.ImportAllBtn.Name = "ImportAllBtn";
            this.ImportAllBtn.Size = new System.Drawing.Size(172, 37);
            this.ImportAllBtn.TabIndex = 5;
            this.ImportAllBtn.Text = "Import Messages File";
            this.ImportAllBtn.Click += new System.EventHandler(this.ImportAllBtn_Click);
            //
            // ExportAllBtn
            //
            this.ExportAllBtn.Location = new System.Drawing.Point(7, 66);
            this.ExportAllBtn.Name = "ExportAllBtn";
            this.ExportAllBtn.Size = new System.Drawing.Size(173, 37);
            this.ExportAllBtn.TabIndex = 4;
            this.ExportAllBtn.Text = "Export To Messages File";
            this.ExportAllBtn.Click += new System.EventHandler(this.ExportAllBtn_Click);
            //
            // button3
            //
            this.button3.Location = new System.Drawing.Point(7, 110);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(173, 37);
            this.button3.TabIndex = 3;
            this.button3.Text = "Import Message";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(7, 153);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(173, 37);
            this.button2.TabIndex = 2;
            this.button2.Text = "Export Message";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            //
            // Resend
            //
            this.Resend.Location = new System.Drawing.Point(7, 347);
            this.Resend.Name = "Resend";
            this.Resend.Size = new System.Drawing.Size(173, 46);
            this.Resend.TabIndex = 1;
            this.Resend.Text = "ReSend Original Message";
            this.Resend.Click += new System.EventHandler(this.button1_Click);
            //
            // btnRead
            //
            this.btnRead.Location = new System.Drawing.Point(7, 241);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(173, 56);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "Read Without Remove Message";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            //
            // grpQSelect
            //
            this.grpQSelect.Controls.Add(this.RBResponseQueue);
            this.grpQSelect.Controls.Add(this.RBRequestQueue);
            this.grpQSelect.Controls.Add(this.cbQue);
            this.grpQSelect.Location = new System.Drawing.Point(10, 9);
            this.grpQSelect.Name = "grpQSelect";
            this.grpQSelect.Size = new System.Drawing.Size(537, 82);
            this.grpQSelect.TabIndex = 1;
            this.grpQSelect.TabStop = false;
            this.grpQSelect.Text = "Queue selection";
            //
            // cbQue
            //
            this.cbQue.Location = new System.Drawing.Point(7, 51);
            this.cbQue.Name = "cbQue";
            this.cbQue.Size = new System.Drawing.Size(517, 24);
            this.cbQue.TabIndex = 0;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.txtMsgCont);
            this.groupBox2.Location = new System.Drawing.Point(10, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(537, 411);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message Content";
            //
            // txtMsgCont
            //
            this.txtMsgCont.Location = new System.Drawing.Point(7, 22);
            this.txtMsgCont.Multiline = true;
            this.txtMsgCont.Name = "txtMsgCont";
            this.txtMsgCont.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsgCont.Size = new System.Drawing.Size(519, 383);
            this.txtMsgCont.TabIndex = 0;
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog1";
            //
            // RBRequestQueue
            //
            this.RBRequestQueue.AutoSize = true;
            this.RBRequestQueue.Checked = true;
            this.RBRequestQueue.Location = new System.Drawing.Point(94, 19);
            this.RBRequestQueue.Name = "RBRequestQueue";
            this.RBRequestQueue.Size = new System.Drawing.Size(100, 17);
            this.RBRequestQueue.TabIndex = 1;
            this.RBRequestQueue.TabStop = true;
            this.RBRequestQueue.Text = "Request Queue";
            this.RBRequestQueue.UseVisualStyleBackColor = true;
            this.RBRequestQueue.CheckedChanged += new System.EventHandler(this.RBRequestQueue_CheckedChanged);
            //
            // RBResponseQueue
            //
            this.RBResponseQueue.AutoSize = true;
            this.RBResponseQueue.Location = new System.Drawing.Point(207, 19);
            this.RBResponseQueue.Name = "RBResponseQueue";
            this.RBResponseQueue.Size = new System.Drawing.Size(108, 17);
            this.RBResponseQueue.TabIndex = 2;
            this.RBResponseQueue.Text = "Response Queue";
            this.RBResponseQueue.UseVisualStyleBackColor = true;
            this.RBResponseQueue.CheckedChanged += new System.EventHandler(this.RBResponseQueue_CheckedChanged);
            //
            //
            // btnExportMsgs
            //
            this.btnExportMsgs.Location = new System.Drawing.Point(6, 443);
            this.btnExportMsgs.Name = "btnExportMsgs";
            this.btnExportMsgs.Size = new System.Drawing.Size(174, 39);
            this.btnExportMsgs.TabIndex = 9;
            this.btnExportMsgs.Text = "Export to Text File";
            this.btnExportMsgs.UseVisualStyleBackColor = true;
            this.btnExportMsgs.Click += new System.EventHandler(this.btnExportMsgs_Click);
            //
            // MsgReaderForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(760, 517);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpQSelect);
            this.Controls.Add(this.grpActionBtn);
            this.Name = "MsgReaderForm";
            this.Text = "MsgReaderForm";
            this.Load += new System.EventHandler(this.MsgReaderForm_Load);
            this.grpActionBtn.ResumeLayout(false);
            this.grpQSelect.ResumeLayout(false);
            this.grpQSelect.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new MsgReaderForm());
		}

		private void MsgReaderForm_Load(object sender, System.EventArgs e)
		{
            GetQueueList();
		}

		private void btnRead_Click(object sender, System.EventArgs e)
		{
			try
			{
                ReadFromQueue();
                this.txtMsgCont.ReadOnly = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

        private void ReadFromQueue()
        {
            //try
            //{
                this.txtMsgCont.Text = "";
                string msgContent = string.Empty;
                MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
                System.Messaging.Message myMessage = myQueue.Peek(new TimeSpan(0) );
                Made4Net.Shared.QMsgSender qm = Made4Net.Shared.QMsgSender.Deserialize(myMessage.BodyStream);

                msgContent = myMessage.Label + "\r\n";
                msgContent = msgContent + "--------------------------------\r\n";
                for (int i = 0; i < qm.Values.Count; i++)
                {
                    msgContent = msgContent + qm.Values.Keys[i] + " : " + qm.Values[i] + "\r\n";
                }
                this.txtMsgCont.Text = msgContent;
                processMessage = true;
                vMessage = myMessage;
                readQ = myQueue;
                resendQName = qm.Values["QPath"];
            //}
            //catch (Exception ex)
           // {
               // this.txtMsgCont.Text = ex.Message;
            //}
        }

		private void button1_Click(object sender, System.EventArgs e)
		{
            ResendMessage();

		}

        private void ResendMessage()
        {
            if (string.IsNullOrEmpty(this.txtMsgCont.Text.Trim()) || !processMessage)
            {
                this.txtMsgCont.Text = "Can not send blank message. Please modify accordingly.";
                processMessage = false;
                return;
            }
            //If the Message has Q Name associated with take it from there, else take manual selection
            string qName = string.IsNullOrEmpty(resendQName) == true ? this.cbQue.SelectedValue.ToString() : resendQName;
            MessageQueue myQueue = new MessageQueue(qName);//this.cbQue.SelectedValue.ToString());
            myQueue.Send(vMessage);
            if (readQ != null && !string.IsNullOrEmpty(resendQName))
            {
                readQ.Receive();
                processMessage = false;
            }
            this.txtMsgCont.Text = string.Empty;
        }

		private void button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.txtMsgCont.Text = "";
				string msgContent = string.Empty;
				MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
				System.Messaging.Message myMessage = myQueue.Receive();
				byte[] arr = new byte[myMessage.BodyStream.Length];
				myMessage.BodyStream.Read(arr, 0, (int)myMessage.BodyStream.Length);

				string str = System.Text.Encoding.ASCII.GetString(arr);

				using(System.IO.StreamWriter sw = new System.IO.StreamWriter("c:\\udi1.txt"))
				{
					sw.Write(str);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.txtMsgCont.Text = "";
				string msgContent = string.Empty;
				MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
				System.Messaging.Message myMessage = new System.Messaging.Message();
				using(System.IO.StreamReader sr = new System.IO.StreamReader("c:\\udi1.txt"))
				{
					myMessage.BodyStream = sr.BaseStream;
					myQueue.Send(myMessage);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

        private void ExportAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.OverwritePrompt = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                bool TimedOut = false;
                MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
                System.Messaging.Message myMessage ;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile());
                sw.WriteLine("<Messages>");
                sw.WriteLine("<QueuePath>" + "<![CDATA[" + this.cbQue.SelectedValue.ToString() + "]]>" + "</QueuePath>");
                while (!TimedOut)
                {
                    try
                    {
                        myMessage = myQueue.Receive(new TimeSpan(0, 0, 0, 2, 0));
                        byte[] arr = new byte[myMessage.BodyStream.Length];
                        myMessage.BodyStream.Read(arr, 0, (int)myMessage.BodyStream.Length);
                        sw.WriteLine("<Message>");
                        sw.WriteLine("<MessageLabel>" + myMessage.Label + "</MessageLabel>");
                        sw.WriteLine("<MessageData>" + Convert.ToBase64String(arr) + "</MessageData>");
                        sw.WriteLine("</Message>");
                    }
                    catch (Exception)
                    {
                        TimedOut = true;
                    }
                }
                sw.WriteLine("</Messages>");
                sw.Flush();
                sw.Close();
                MessageBox.Show("Export Finished");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ImportAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    System.Xml.XmlDocument oXml = new System.Xml.XmlDocument();
                    oXml.Load(openFileDialog1.OpenFile());
                    string qPath = oXml.SelectSingleNode(".//QueuePath").InnerText;
                    MessageQueue myQueue = new MessageQueue(qPath);
                    System.Messaging.Message myMessage;
                    System.Xml.XmlNodeList mXmlList = oXml.SelectNodes(".//Message");
                    foreach (System.Xml.XmlNode mXml in mXmlList)
                    {
                        myMessage = new System.Messaging.Message();
                        myMessage.Label = mXml.SelectSingleNode(".//MessageLabel").InnerText;
                        byte[] arr = Convert.FromBase64String(mXml.SelectSingleNode(".//MessageData").InnerText);
                        myMessage.BodyStream.Write(arr,0,arr.Length);
                        myQueue.Send(myMessage);
                    }
                    MessageBox.Show("Import Finished");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Remove message from Queue and read the content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtMsgCont.Text = "";
                string msgContent = string.Empty;
                MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
                System.Messaging.Message myMessage = myQueue.Receive(new TimeSpan(0));
                Made4Net.Shared.QMsgSender qm = Made4Net.Shared.QMsgSender.Deserialize(myMessage.BodyStream);

                msgContent = myMessage.Label + "\r\n";
                msgContent = msgContent + "--------------------------------\r\n";
                for (int i = 0; i < qm.Values.Count; i++)
                {
                    msgContent = msgContent + qm.Values.Keys[i] + " : " + qm.Values[i] + "\r\n";
                }
                this.txtMsgCont.Text = msgContent;
                processMessage = true;
                this.txtMsgCont.ReadOnly = true;
                vMessage = myMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Allow to Edit the Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            //ReadFromQueue();
            this.txtMsgCont.ReadOnly = false;
        }

        private void sendEdited_Click(object sender, EventArgs e)
        {
            if (this.txtMsgCont.ReadOnly)
            {
                ResendMessage();
            }
            else
            {
                //TODO: Read text Content and form the Message
                try
                {

                    if (string.IsNullOrEmpty(this.txtMsgCont.Text.Trim()) || !processMessage)
                    {
                        this.txtMsgCont.Text = "Can not send blank message. Please modify accordingly.";
                        processMessage = false;
                        return;
                    }
                    string[] HeaderDelimitChar = new string[] { "--------------------------------\r\n" };
                    string label;
                    string[] LineDelimitChar = new string[] { "\r\n" };
                    string[] KeyValDelimitChar = new string[] { ":" };
                    string editedMessage = this.txtMsgCont.Text;
                    string[] messageLabelAndBody = editedMessage.Split(HeaderDelimitChar, StringSplitOptions.None);
                    label = messageLabelAndBody[0];
                    label = label.Replace("\r\n", "");
                    string[] messageBodyLines = messageLabelAndBody[1].Split(LineDelimitChar, StringSplitOptions.None);
                    string qNameFromMessage = "";
                    MessageQueue myQueue;


                    System.Messaging.Message mm = new System.Messaging.Message();
                    Made4Net.Shared.QMsgSender qm = new Made4Net.Shared.QMsgSender();

                    foreach (string line in messageBodyLines)
                    {
                        string keyVal = "";
                        string keyName = "";
                        if (line != "")
                        {
                            string[] messageKeyValue = line.Split(KeyValDelimitChar, StringSplitOptions.None);
                            keyName = messageKeyValue[0].ToString();
                            if (keyName.Trim() == "QPath")
                            {
                                qNameFromMessage = messageKeyValue[1];
                            }
                            if(messageKeyValue[1] != null)
                            {
                                keyVal = messageKeyValue[1].ToString();
                            }

                            qm.Add(keyName.Trim(), keyVal.Trim());

                        }
                    }
                    if(qNameFromMessage != "")
                    {
                         myQueue = new MessageQueue(qNameFromMessage);
                    }
                    else
                    {
                         myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
                    }

                    qm.Send(myQueue, label, "", System.Messaging.MessagePriority.Normal);
                    this.txtMsgCont.Text = string.Empty;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #region Load combo box
        private void GetQueueList()
        {
            try
            {
                string _instanceName = Made4Net.DataAccess.Util.GetAppConfigNameValue("PWMSInstanceName"); // PWMS-817
                string sql = string.Empty;
                DataTable dt = new DataTable();
                if (RBResponseQueue.Checked)
                    sql = "select * from MESSAGEQUEUES where RESPONSEQ is not null";
                else
                    sql = "SELECT * FROM MESSAGEQUEUES";

                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, false, "Made4NetSchema");

                foreach (DataRow dr in dt.Rows)
                {
                    dr["QUEUEPATH"] = dr["QUEUEPATH"] + "_" + _instanceName;
                    dr.AcceptChanges();
                }

                cbQue.DataSource = dt;

                cbQue.DisplayMember = "QUEUENAME";
                cbQue.ValueMember = "QUEUEPATH";
            }
            catch (Exception Exception)
            {
                this.txtMsgCont.Text = Exception.Message;
            }

        }
        #endregion

        private void RBRequestQueue_CheckedChanged(object sender, EventArgs e)
        {
            GetQueueList();
        }

        private void RBResponseQueue_CheckedChanged(object sender, EventArgs e)
        {
            GetQueueList();
        }

        private void btnExportMsgs_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.AddExtension = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bool TimedOut = false;
                    MessageQueue myQueue = new MessageQueue(this.cbQue.SelectedValue.ToString());
                    System.Messaging.Message myMessage;
                    int count = 0;
                    System.Text.StringBuilder msgContent = new System.Text.StringBuilder();
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.OpenFile());
                    msgContent.Append("<Messages>");
                    msgContent.Append("<QueuePath>" + "<![CDATA[" + this.cbQue.SelectedValue.ToString() + "]]>" + "</QueuePath>");
                    while (!TimedOut)
                    {
                        try
                        {
                            myMessage = myQueue.Receive(new TimeSpan(0, 0, 0, 2, 0));
                            Made4Net.Shared.QMsgSender qm = Made4Net.Shared.QMsgSender.Deserialize(myMessage.BodyStream);

                            msgContent.AppendLine(myMessage.Label);
                            msgContent.AppendLine((++count).ToString() + ": --------------------------------");
                            for (int i = 0; i < qm.Values.Count; i++)
                            {
                                msgContent.AppendLine(qm.Values.Keys[i] + " : " + qm.Values[i]);
                            }
                        }
                        catch (Exception)
                        {
                            TimedOut = true;
                        }
                    }
                    msgContent.AppendLine("</Messages>");
                    sw.WriteLine(msgContent.ToString());

                    sw.Flush();
                    sw.Close();
                    saveFileDialog1.Reset();
                    MessageBox.Show("Export Finished");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
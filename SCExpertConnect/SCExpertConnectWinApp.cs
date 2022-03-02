using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Threading;
using System.Messaging;
using System.Runtime.InteropServices;
using Made4Net.Shared;
using Made4Net.DataAccess;
using ExpertObjectMapper;
using System.Reflection;
using SCExpertConnectPlugins.BO;

namespace SCExpertConnect
{
    public partial class SCExpertConnectWinApp : Form
    {
        #region Members

        protected string mLicensUser = "SCExpert Connect Service";
        protected Thread ImporterThread;
        protected SCExpertConnectExporter mExporter = null;
        private bool bLicensed;

        #endregion

        public SCExpertConnectWinApp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bLicensed)
                {
                    MessageBox.Show("Application is not licensed, cannot start services...", "License not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblState.Text = "The connect services are not running!!!";
                    lblState.ForeColor = System.Drawing.Color.Red;
                }
                ImporterThread = new Thread(InitPlugins);
                ImporterThread.Start();
                mExporter = new SCExpertConnectExporter();
                mExporter.StartQueue();
                lblState.Text = "The connect services are running.";
                lblState.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblState.Text = "The connect services are not running!!!";
                lblState.ForeColor = System.Drawing.Color.Red;
            }
        }

        #region ImportPlugins

        private void InitPlugins()
        {
            SCExpertConnectImporter oImporter = new SCExpertConnectImporter();
            oImporter.InitPlugins();
        }

        #endregion

        #region License Methods

        protected bool Connect()
        {
            return true;// Made4Net.DataAccess.DataInterface.Connect(mLicensUser);
        }

        protected bool DisConnect()
        {
            return true;// Made4Net.DataAccess.DataInterface.Disconnect(mLicensUser);
        }

        #endregion

        #region Form Events

        private void SCExpertConnectWinApp_Load(object sender, EventArgs e)
        {
            try
            {
                bLicensed = Connect();
                if (!bLicensed)
                {
                    MessageBox.Show("License Not Found - Connect Services will not be excexuted!", "License not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SCExpertConnectWinApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisConnect();
                if (mExporter != null)
                    mExporter.StopQueue();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
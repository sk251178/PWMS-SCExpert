using Made4Net.Shared;

namespace WMS.Billing.ChargeExport
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            //this.serviceInstaller1.Description = "Expert BillingChargeExport";
            //this.serviceInstaller1.DisplayName = "Expert BillingChargeExport";
            //this.serviceInstaller1.ServiceName = "BillingChargeExportService";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "MSMQ"};

            this.serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceSection ); // PWMS-817
            this.serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.PWMSBillingChargeExportServiceSection); // PWMS-817
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Manual;

            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}
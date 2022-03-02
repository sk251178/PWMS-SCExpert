using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using Made4Net.Shared;

namespace WMS.OredersAutomation
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

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
            this.serviceProcessInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // serviceInstaller1
            // 
            //this.serviceInstaller1.DisplayName = "Expert Orders Automation";
            //this.serviceInstaller1.ServiceName = "Expert Orders Automation";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "MSMQ"};
            this.serviceInstaller1.DisplayName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.OrdersAutomationServiceSection);
            this.serviceInstaller1.ServiceName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.OrdersAutomationServiceSection);
            
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.DelayedAutoStart = true;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion
    }
}
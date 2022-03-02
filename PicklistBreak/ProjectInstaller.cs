using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using Made4Net.Shared;

namespace PicklistBreak
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            //System.Threading.Thread.Sleep(10000);
            //System.Threading.Thread.Sleep(10000);

            InitializeComponent();
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
            //this.serviceInstaller1.Description = "Expert Picklist Break";
            //this.serviceInstaller1.DisplayName = "Expert Picklist Break";
            //this.serviceInstaller1.ServiceName = "PicklistBreak";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "MSMQ"};
            this.serviceInstaller1.DisplayName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, "PickListConfig");
            this.serviceInstaller1.ServiceName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, "PickListConfig");
            
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
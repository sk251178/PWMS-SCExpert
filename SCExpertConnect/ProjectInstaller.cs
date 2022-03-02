using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace SCExpertConnect
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //System.Diagnostics.Debugger.Launch();   
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
            //this.serviceInstaller1.Description = "Expert Connect ";
            //this.serviceInstaller1.DisplayName = "Expert Connect ";
            //this.serviceInstaller1.ServiceName = "SCExpertConnect";
            this.serviceInstaller1.ServicesDependedOn = new string[] {
        "MSMQ"};
            //this.serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //this.serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectSection);
            this.serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.ExpertConnectSection);
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
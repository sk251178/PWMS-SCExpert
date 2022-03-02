using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.TaxRight
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_BeforeInstall(object sender, InstallEventArgs e)
        {
            serviceInstaller1.DisplayName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.TaxRightServiceSection);
            serviceInstaller1.ServiceName = Made4Net.Shared.Util.GetServiceName(Made4Net.Shared.ConfigurationSettingsConsts.ServiceInstancename, Made4Net.Shared.ConfigurationSettingsConsts.TaxRightServiceSection);
        }
    }
}

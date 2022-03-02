using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace WMS.Billing.ChargeExport
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            System.Threading.Thread.Sleep(10000);
            System.Threading.Thread.Sleep(10000);

            InitializeComponent();
        }
    }
}
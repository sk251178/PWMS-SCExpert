using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WMS.TaxRight
{
    public partial class TaskRightService : ServiceBase
    {
        private TaxRightHandler taxRightHandler = null;

        public TaskRightService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                taxRightHandler = new TaxRightHandler(); //PWMS-817
                taxRightHandler.StartQueue();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnStop()
        {
        }

    }
}

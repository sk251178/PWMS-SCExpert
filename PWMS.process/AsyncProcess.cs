using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Made4Net.Shared; 


namespace PWMS.process
{
    public class AsyncProcess
    {
        public Made4Net.Shared.Logging.LogFile mPWMSService = null;
        public void Process()
        {
            string DirPath = Made4Net.DataAccess.Util.GetInstancePath(true);
            DirPath += "\\Logs\\AsyncQueue";
            mPWMSService = new Made4Net.Shared.Logging.LogFile(DirPath, "AsyncQueue", true);
            mPWMSService.WriteLine("Message from AsynProcess", true);

        }
    }
}

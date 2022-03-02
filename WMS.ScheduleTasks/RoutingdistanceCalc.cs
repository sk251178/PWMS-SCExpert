using System;
using System.Collections.Generic;
using System.Text;
using WMS.Logic; 

namespace WMS.SchedulerTasks
{
    class RoutingdistanceCalc
    {
        public RoutingdistanceCalc(){}

        public void buildNetworkbyParam()
        {
        ContactNetworkBuilder  oContactNetworkBuilder=new ContactNetworkBuilder();
        oContactNetworkBuilder.buildNetworkbyParam();

        }

    }
}

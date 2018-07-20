using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    public interface IServiceBus
    {
        bool Start();

        bool Stop();

        //void ExecCommand(object obj);

        ServiceBusStatus GetServiceBusStatus();

        int GetNodeValue(string nodeStrGuid, out object nodeValue);

        event EventHandler<BusRefreshEventArgs> BusRefreshEvent;

    }
}

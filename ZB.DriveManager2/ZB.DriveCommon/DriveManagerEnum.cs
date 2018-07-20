using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    class DriveManagerEnum
    {
    }

    public enum DriveNodeType
    {
        OPC=1,

    }

    public enum DriveNodeModel
    {
        OPCService=1,
        OPCGroup=2,
        OPCItem=3,
    }

    public enum ServiceBusStatus
    {
        Starting=1,
        Start=2,
        Stoping=3,
        Stop=4,
        Pause=5,
    }
}

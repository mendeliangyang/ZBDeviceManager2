using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    public class BusRefreshEventArgs : EventArgs
    {
        public object RefreshData;
        public string RefreshDeviceId;
    }
}

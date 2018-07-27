using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTDataCentreDaemon
{
    public class ConfigEntity
    {
        private string iotIp;
        private int iotPort;
        private int iotId;
        private int iotGroupId;

        private int iotDataAdapterPort;
        private int iotMockDAId;


        private string processFileName;
        private string processName;

        private int receiveIotTimeOut;
        private int rateIotTime;

        public string IotIp
        {
            get
            {
                return iotIp;
            }

            set
            {
                iotIp = value;
            }
        }

        public int IotPort
        {
            get
            {
                return iotPort;
            }

            set
            {
                iotPort = value;
            }
        }

        public int ReceiveIotTimeOut
        {
            get
            {
                return receiveIotTimeOut;
            }

            set
            {
                receiveIotTimeOut = value;
            }
        }

        public int RateIotTime
        {
            get
            {
                return rateIotTime;
            }

            set
            {
                rateIotTime = value;
            }
        }

        public int IotId
        {
            get
            {
                return iotId;
            }

            set
            {
                iotId = value;
            }
        }

        public int IotGroupId
        {
            get
            {
                return iotGroupId;
            }

            set
            {
                iotGroupId = value;
            }
        }

        public string ProcessFileName
        {
            get
            {
                return processFileName;
            }

            set
            {
                processFileName = value;
            }
        }

        public string ProcessName
        {
            get
            {
                return processName;
            }

            set
            {
                processName = value;
            }
        }

        public int IotDataAdapterPort
        {
            get
            {
                return iotDataAdapterPort;
            }

            set
            {
                iotDataAdapterPort = value;
            }
        }

        public int IotMockDAId
        {
            get
            {
                return iotMockDAId;
            }

            set
            {
                iotMockDAId = value;
            }
        }
    }
}

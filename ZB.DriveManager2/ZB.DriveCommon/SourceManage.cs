using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    /// <summary>
    /// 数据源模型
    /// </summary>
    public class SourceManage : INotifyPropertyChanged
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }

        /// <summary>
        /// ITC网卡的索引
        /// </summary>
        public byte ITCNetCardIndex { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 服务源名称
        /// </summary>
        public string ServerName
        {
            get; set;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string Status
        {
            get
            {
                if (State == ServerStatus.Running.ToString())
                {
                    return "启动";
                }
                else
                {
                    return "未启动";
                }
            }
        }

        private string state;

        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        private string protocolType;

        /// <summary>
        /// 协议类型
        /// </summary>
        public string ProtocolType
        {
            get
            {
                return protocolType;
            }
            set
            {
                protocolType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProtocolType"));
            }
        }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostAddress
        {
            get;set;
        }

        /// <summary>
        /// 服务的实例
        /// </summary>
        public ZB.DdcService.IBaseServerProvider Server
        {
            get; set;
        }

        /// <summary>
        /// OPC服务的GUID
        /// </summary>
        public Guid OpcGuid
        {
            get;set;
        }

        public int Rate
        {
            get;set;
        }

        private int remotePort;

        public int RemotePort
        {
            get { return remotePort; }
            set { remotePort = value; }
        }

        private int localPort;

        public int LocalPort
        {
            get { return localPort; }
            set { localPort = value; }
        }

        private string username;

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        private string passwd;

        public string Passwd
        {
            get { return passwd; }
            set { passwd = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }

        //private List<Tuple<string, string, string>> refreshList = new List<Tuple<string, string, string>>();
        //public List<Tuple<string, string, string>> RefreshList
        //{
        //    get
        //    {
        //        return refreshList;
        //    }
        //    set
        //    {
        //        refreshList = value;
        //    }
        //}

        private List<string> refreshList = new List<string>();
        public List<string> RefreshList
        {
            get
            {
                return refreshList;
            }
            set
            {
                refreshList = value;
            }
        }
    }

    public enum ConnectType
    {
        OPC = 0,
        UDP = 1,
        TCP = 2
    }

    public enum ServerStatus
    {
        Initialized,
        Stopped,
        Paused,
        Running,
        Error
    }
}

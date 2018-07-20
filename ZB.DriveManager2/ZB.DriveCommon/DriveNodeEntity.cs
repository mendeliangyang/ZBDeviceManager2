using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    public class DriveNodeEntity
    {

        public string nodeName;

        public string nodeDisName;

        public List<DriveNodeEntity> childNode = new List<DriveNodeEntity>();

        public DriveNodeType nodeType;

        public DriveNodeModel nodeModel;

        public string nodeId;
        public string paramId;
        public string description;
        public string nodeStrGuid;
        public string serviceIp;
        public int servicePort;
        public int localPort;
        public int rate;
        public int index;
     
    }
}


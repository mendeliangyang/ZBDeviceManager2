using HaiGrang.Package.OpcNetApiChs.Common;
using HaiGrang.Package.OpcNetApiChs.Da;
using HaiGrang.Package.OpcNetApiChs.DaNet;
using HaiGrang.Package.OpcNetApiChs.Opc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using ZB.DriveCommon;
using ZB.NlogHelper;

namespace ZB.OPCBus
{
    public class OPCHelper
    {
        public static Dictionary<string, Guid> GetServersNameAndGuid(string hostNameOrAddress)
        {
            string[] servers = null;
            Guid[] guids = null;
            Dictionary<string, Guid> serverAndGuidDic = new Dictionary<string, Guid>();
            try
            {
                IPHostEntry ip_host = Dns.GetHostEntry(hostNameOrAddress);
                Host host = new Host(ip_host.HostName);
                OpcServerBrowser browser = new OpcServerBrowser(host);
                browser.GetServerList(out servers, out guids);
                for (int i = 0; i < servers.Length; i++)
                {
                    serverAndGuidDic[servers[i]] = guids[i];
                }
            }
            catch (Exception err)
            {
                NLogHelper.ExceptionInfo(err, "GetServersNameAndGuid error :{0}", err.Message);
            }
            return serverAndGuidDic;
        }

        public static int GetOpcAllItemByNodeRoot(DriveNodeEntity nodeRoot,out List<DriveNodeEntity> listItemNodes)
        {
            listItemNodes = null;
            try
            {
                if (null == nodeRoot)
                {
                    return 1;
                }
                listItemNodes = new List<DriveNodeEntity>();
                foreach (DriveNodeEntity group in nodeRoot.childNode)
                {
                    foreach (DriveNodeEntity item in group.childNode)
                    {
                        listItemNodes.Add(item);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "GetOpcAllItemByNodeRoot exception:{0} ",ex.Message);
                return -1;
            }
        }

        public static int GetOpcServerAllItem(string opcSerHostIp,Guid opcSerGuid, out List<DriveNodeEntity> driveNotes)
        {
            int rtc = -1;
            driveNotes = null;
            try
            {
                OpcServer opcSer = new OpcServer();
                rtc = opcSer.Connect(new Host(opcSerHostIp), opcSerGuid);
                if (!HRESULTS.Succeeded(rtc))
                {
                    return rtc;
                }
                //获取组
                if (GetOPCServerStatus(opcSer, out rtc))
                {
                   return GetOpcServerAllItem(opcSer,out driveNotes);
                }

            }
            catch (Exception err)
            {
                NLogHelper.ExceptionInfo(err, "GetDriveNodes error :{0}", err.Message);
            }
            return rtc;


        }
        public static bool GetOPCServerStatus(OpcServer opcSer,out int opcSerStatus)
        {
            bool theGetServerStatusSucceed = false;
            opcSerStatus = -1;
            SERVERSTATUS theServerStatus;

            try
            {
                opcSerStatus = opcSer.GetStatus(out theServerStatus);

                if (HRESULTS.Succeeded(opcSerStatus))
                {
                    theGetServerStatusSucceed =true;
                }
            }
            catch (Exception err)
            {
                NLogHelper.ExceptionInfo(err, "GetOPCServerStatus error :{0}", err.Message);
            }

            return theGetServerStatusSucceed;
        }

        public static List<Tuple<string,Guid>> GetOPCServiceList(string opcSerHostIp)
        {
            try
            {
                Host host = new Host(opcSerHostIp);
                OpcServerBrowser opcServerBrowser = new OpcServerBrowser(host);
                string[] servers;
                Guid[] serverGuids;
                opcServerBrowser.GetServerList(out servers, out serverGuids);
                if (null!=servers)
                {
                    List<Tuple<string, Guid>> serverList = new List<Tuple<string, Guid>>();
                    for (int i = 0; i < servers.Length; i++)
                    {
                        serverList.Add(new Tuple<string, Guid>(servers[i],serverGuids[i]));
                    }
                    return serverList;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "GetOPCServiceList error :{0}", ex.Message);
            }
            return null;
        }

        public static int GetOpcServerAllItem(OpcServer opcSer, out List<DriveNodeEntity> driveNodes)
        {
            int rtc = -1;
            driveNodes = null;
            BrowseTree browseTress = new BrowseTree(opcSer);
            rtc = browseTress.CreateTree();
            if (HRESULTS.Succeeded(rtc))
            {
                TreeNode[] root = browseTress.Root();
                if (root != null)
                {
                    driveNodes = new List<DriveNodeEntity>();
                    // driveNotes 固定两层 ，没有使用递归 转换
                    foreach (TreeNode item in root)
                    {
                        DriveNodeEntity driveNodeTemp=CloneDriveNode(item, DriveNodeModel.OPCGroup);
                        foreach (TreeNode itemChild in item.Nodes)
                        {
                            driveNodeTemp.childNode.Add(CloneDriveNode(itemChild, DriveNodeModel.OPCItem));
                        }
                        driveNodes.Add(driveNodeTemp);
                    }
                    return 0;
                }
            }
            return rtc;
        }

        private static DriveNodeEntity CloneDriveNode(TreeNode treeNode,DriveNodeModel driveNodeModel)
        {
            DriveNodeEntity driveNode = new DriveNodeEntity();
            driveNode.nodeId = GuidHelper.GetStringGuid();
            driveNode.nodeType = DriveNodeType.OPC;
            driveNode.nodeModel = driveNodeModel;
            driveNode.nodeName = treeNode.Tag.ToString();
            driveNode.nodeDisName = treeNode.Text;
            return driveNode;
        }

        public static int ConnectOPCServer(ref OpcServer opcServer, Host opcSerHost,Guid opcSerGuid)
        {
            int rtc = -1;
            try
            {
                if (null==opcServer)
                {
                    opcServer = new OpcServer();
                }
                rtc= opcServer.Connect(opcSerHost, opcSerGuid);
                
            }
            catch (Exception err)
            {
                NLogHelper.ExceptionInfo(err, "ConnectOPCServer error :{0}", err.Message);
            }
            return rtc;
            
        }


    }
}

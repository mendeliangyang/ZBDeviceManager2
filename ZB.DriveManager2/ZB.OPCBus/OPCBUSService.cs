using HaiGrang.Package.OpcNetApiChs.Common;
using HaiGrang.Package.OpcNetApiChs.Da;
using HaiGrang.Package.OpcNetApiChs.DaNet;
using HaiGrang.Package.OpcNetApiChs.Opc;
using System;
using System.Collections.Generic;
using ZB.DriveCommon;
using ZB.NlogHelper;

namespace ZB.OPCBus
{
    public class OPCBUSService : IServiceBus
    {
        private ServiceBusStatus CurrentBusStatus = ServiceBusStatus.Stop;

        Host host = null;
        OpcServer opcSer = null;
        Guid guid;
        SyncIOGroup syncIOGroup;
        RefreshEventHandler refreshEventHandler;

        RefreshGroup refreshGroup;

        DriveNodeEntity opcSerNodes;

        List<DriveNodeEntity> allOPCItemNodes;

        public event EventHandler<BusRefreshEventArgs> BusRefreshEvent;

        public OPCBUSService() { }
        

        public OPCBUSService(DriveNodeEntity _driveNodes)
        {
            opcSerNodes = _driveNodes;
            
        }


        public ServiceBusStatus GetServiceBusStatus()
        {
            return CurrentBusStatus;
        }

        private bool InitOPCService()
        {
            try
            {
                int iRet = OPCHelper.GetOpcAllItemByNodeRoot(opcSerNodes, out allOPCItemNodes);
                if (0 != iRet)
                {
                    NLogHelper.DefalutError("GetOpcAllItemByNodeRoot 获取opc服务设备项失败：{0}", iRet.ToString());
                    return false;
                }
                host = new Host(opcSerNodes.serviceIp);
                opcSer = new OpcServer();
                guid = new Guid(opcSerNodes.nodeStrGuid);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "InitOPCService exception: {0}", ex.Message);
            }
            return false;
        }

        public bool Start()
        {
            try
            {
                bool bRet= InitOPCService();
                if (!bRet)
                {
                    return false;
                }
                if (null == opcSerNodes || null == allOPCItemNodes)
                {
                    NLogHelper.DefalutError("OPCBUSService.Start error,未配置opc服务");
                    return false;
                }
                opcSer.Connect(host, guid);
                syncIOGroup = new SyncIOGroup(opcSer);
                refreshEventHandler = new RefreshEventHandler(RefreshEvent);
                refreshGroup = new RefreshGroup(opcSer, 10, refreshEventHandler);
                foreach (DriveNodeEntity item in allOPCItemNodes)
                {
                   int iRet= refreshGroup.Add(item.nodeName);
                    if (HRESULTS.Succeeded(iRet))
                    {
                        Console.WriteLine(" true "+iRet);
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "Start StartOPCService exception: {0}", ex.Message);
                return false;
            }
        }
        private void RefreshEvent(object sender, RefreshEventArguments e)
        {
            for (int i = 0; i < e.items.Length; i++)
            {
                ItemDef theItem = e.items[i];
                string itemName = theItem.OpcIDef.ItemID;
                string itemQuality = refreshGroup.GetQualityString(theItem.OpcIRslt.Quality);
                object itemValue = theItem.OpcIRslt.DataValue;

                double doubleValue;
                if (double.TryParse(itemValue.ToString(), out doubleValue))
                {
                    itemValue = Math.Round(doubleValue, 2);
                }

                DateTime ItemDateTime = theItem.OpcIRslt.TimeStampNet;
                Console.WriteLine("RefreshEvent ……");

                BusRefreshEventArgs ea = new BusRefreshEventArgs();
                ea.RefreshData = itemValue;
                ea.RefreshDeviceId = itemName;
                BusRefreshEvent?.Invoke(this, ea);

                //// SendRealData?.Invoke(da, itemValue.ToString());
                //// 向数据中心发送数据
                //if (da != null)
                //{
                //    string[] deviceIdAndParamId = SendDataIDMap[itemName];
                //    if (string.IsNullOrEmpty(deviceIdAndParamId[0]) || string.IsNullOrEmpty(deviceIdAndParamId[1]) || string.IsNullOrEmpty(itemValue.ToString()))
                //    {
                //        string info = string.Format("[deviceId:{0}, paramId:{1}, paramValue:{2}]", deviceIdAndParamId[0], deviceIdAndParamId[1], itemValue.ToString());
                //        LogPrint.WriteLog("OPC", "发送数据为空: " + info, 1);
                //        continue;
                //    }

                //    lock (lockObj)
                //    {
                //        da.DataPackager.Device = deviceIdAndParamId[0];
                //        da.DataPackager.Add(Convert.ToInt32(deviceIdAndParamId[1]), itemValue.ToString());
                //        da.Send();
                //    }
                //}
            }
        }




        public bool Stop()
        {
            
            //TODO
            return false;
        }

        public int GetNodeValue(string nodeStrGuid,out object nodeValue)
        {
            int rtc = -1;
            nodeValue = null;
            try
            {
                OPCDATASOURCE opcDataSource = OPCDATASOURCE.OPC_DS_DEVICE;

                OPCItemState opcItemState = new OPCItemState();

                DriveNodeEntity driveNode= allOPCItemNodes.Find(p=> nodeStrGuid.Equals(p.nodeId));

                ItemDef itemdef= syncIOGroup.Item(driveNode.nodeName);

                rtc = syncIOGroup.Read(opcDataSource, itemdef, out opcItemState);

                if (HRESULTS.Succeeded(rtc))
                {
                    nodeValue= opcItemState.DataValue;
                    //txtItemQuality.Text = syncIOGroup.GetQualityString(opcItemState.Quality);
                    //txtTimeStamp.Text = DateTime.FromFileTime(opcItemState.TimeStamp).ToString();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "GetNodeValue param:'{1}' ,exception:{0}",ex.Message,nodeStrGuid);
            }
            
            return rtc;
        }
    }
}

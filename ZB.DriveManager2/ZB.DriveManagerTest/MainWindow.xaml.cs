using System;
using System.Collections.Generic;
using System.Windows;
using ZB.DriveCommon;
using ZB.OPCBus;
using ZB.TransmitDA;
using ZB.DataAdapter;

namespace ZB.DriveManagerTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        DriveNodeEntity driveNodeRoot = new DriveNodeEntity();
        TransmitDAClient transmit;

        IServiceBus serBus;


        private void 获取opc配置_Click(object sender, RoutedEventArgs e)
        {
            Tuple<string,Guid> server= cmb_OpcSerList.SelectedItem as Tuple<string, Guid>;
            if (null==server)
            {
                MessageBox.Show("请选择服务点");
                return;
            }
            List<DriveNodeEntity> listDriveNodes;
            OPCHelper.GetOpcServerAllItem(txtOpcIp.Text.Trim(), server.Item2, out listDriveNodes);
            
            driveNodeRoot.nodeStrGuid = server.Item2.ToString();
            driveNodeRoot.nodeName = server.Item1;
            driveNodeRoot.serviceIp = txtOpcIp.Text.Trim();
            driveNodeRoot.childNode = listDriveNodes;

            string strDriveNodeRoot;
            JsonHelper.ObjectToStrJson(driveNodeRoot, out strDriveNodeRoot);
            Console.WriteLine(strDriveNodeRoot);

        }

        private void 获取opc服务列表_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string,Guid>> opcSerList= OPCHelper.GetOPCServiceList(txtOpcIp.Text.Trim());
            cmb_OpcSerList.ItemsSource = opcSerList;
        }

        private void 开启opc转发_Click(object sender, RoutedEventArgs e)
        {
            serBus = new OPCBUSService(driveNodeRoot);
            serBus.BusRefreshEvent += SerBus_BusRefreshEvent;
            serBus.Start();
        }

        private void SerBus_BusRefreshEvent(object sender, BusRefreshEventArgs e)
        {
            Console.WriteLine("SerBus_BusRefreshEvent ……");

            if (null!=transmit)
            {
                transmit.DataPackager.Device = e.RefreshDeviceId;//deviceId;
                transmit.DataPackager.Add(1, e.RefreshData.ToString());
                transmit.Send();
            }

        }

        private void 获取对应项值_Click(object sender, RoutedEventArgs e)
        {
            string strOpcItem= txtOPCItem.Text.Trim();
            object nodeValue;
            serBus.GetNodeValue(strOpcItem, out nodeValue);
            Console.WriteLine(nodeValue);
        }

        private void 启动da客户端_Click(object sender, RoutedEventArgs e)
        {
            StartTransmitDA();
        }

        private void StartTransmitDA()
        {
            if (null != transmit)
            {
                transmit.Close();
            }
            transmit = new TransmitDAClient();
            transmit.Connect("127.0.0.1", 6000);
        }
    }
}

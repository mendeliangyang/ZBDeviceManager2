using System;
using System.Net.Sockets;
using System.Text;
using ZB.Core;
using ZB.DataBridge;

namespace IOTDataCentreDaemon
{
    public delegate void DataClientDataReceived(string device, int id, object value);



    internal class DaemonHelper
    {
        private DataClient client = null;
        private ConfigEntity iotConfig = null;

        internal DataClientDataReceived dataReceived;


        internal void Init(ConfigEntity config)
        {
            try
            {
                if (null == client)
                {
                    client = new DataClient(ushort.MaxValue);
                    client.DataReceived += new DataReceived(client_DataReceived);
                    client.MessageReceived += new MessageReceived(client_MessageReceived);
                    client.Disconnected += new EventHandler(client_Disconnected);
                }
                if (client.Connected)
                {
                    client.Close();
                }
                iotConfig = config;
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "InitClientException:{0}", ex.Message);
            }
        }

        internal bool ConnectIot()
        {
            try
            {
                if (client.Connected)
                {
                    client.Close();
                }
                client.Connect(iotConfig.IotIp, iotConfig.IotPort);
                client.RegisterAll();
                client.Identify(iotConfig.IotId, iotConfig.IotGroupId);
                return true;
            }
            catch (SocketException ex)
            {
                NLogHelper.DefalutError("  ConnectIot SocketException:{0}", ex.Message);
                NLogHelper.ExceptionInfo(ex, "  ConnectIot SocketException:{0}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                NLogHelper.DefalutError("  ConnectIot Exception:{0}", ex.Message);
                NLogHelper.ExceptionInfo(ex, "  ConnectIot Exception:{0}", ex.Message);
                return false;
            }
            
        }

        internal void CloseIotConnected()
        {
            if (client.Connected)
            {
                client.Close();
            }
        }


        void client_Disconnected(object sender, EventArgs e)
        {

        }

        void client_MessageReceived(object sender, Message message)
        {
            string content;

            if (message.Content is byte[])
                content = "{" + ((byte[])message.Content).ToHexString() + "}" + "string:" + Encoding.UTF8.GetString((byte[])message.Content);
            else
                content = message.Content.ToString();
            NLogHelper.DefalutInfo(string.Format("【消息】 来源：{0} 类别：{2} 消息：{1}", message.AddressID, content, message.Category));
        }

        void client_DataReceived(string device, int id, object value)
        {
            if (null != dataReceived)
            {
                dataReceived(device, id, value);
            }

        }
    }
}

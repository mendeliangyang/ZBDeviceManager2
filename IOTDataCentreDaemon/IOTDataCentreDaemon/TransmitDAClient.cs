using System;
using ZB.Core;
using ZB.DataAdapter;

namespace IOTDataCentreDaemon
{
    public class TransmitDAClient : BaseDataAdapter
    {
        public byte cmd;
        public byte[] cmdext = new byte[12];

        public TransmitDAClient()
            : base(Definition.DEFAULT_BUFFERSIZE)
        {
            cmd = 0;
            
        }

        //public new void Close()
        //{
        //    base.Close();
        //}
        //public new void Connect(string host,int port)
        //{
        //    base.Connect(host,port);
        //}

        protected override void OnReceivedCommand(string device, byte command, byte[] ext, Guid guid, bool reply)
        {
            base.OnReceivedCommand(device, command, ext, guid, reply);
            cmd = command;
            cmdext = ext;
        }

        public new void Reply(Guid index, byte status)
        {
            base.Reply(index, status);
        }

    }
}

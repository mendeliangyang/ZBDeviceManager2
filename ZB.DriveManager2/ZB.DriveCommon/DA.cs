using System;
using ZB.Core;

namespace ZB.DriveCommon
{
    public class DA : ZB.DataAdapter.BaseDataAdapter
    {
        public byte cmd;
        public byte[] cmdext = new byte[12];

        public DA()
            : base(Definition.DEFAULT_BUFFERSIZE)
        {
            cmd = 0;
        }

        protected override void OnReceivedCommand(string device, byte command, byte[] ext, Guid guid, bool reply)
        {
            base.OnReceivedCommand(device, command, ext, guid, reply);
            cmd = command;
            cmdext = ext;


        }

        public new void Reply(Guid index,byte status)
        {
            base.Reply(index,status);
        }
        
    }
}

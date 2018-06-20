using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
namespace WotDossier.Applications.Parser
{
    public class Parser_0_9_13_0 : Parser_0_9_12_0
    {
        protected override ulong PacketChat
        {
            get { return 0x23; }
        }

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x9; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x28; }
        }
    }
}
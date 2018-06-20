using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    /// <summary>
    /// 1.0.0.0
    /// </summary>
    public class Parser_1_0_0_0 : Parser_0_9_21_0
    {
	    protected override ulong UpdateEvent_Arena => 53;

	    protected override ulong UpdateEvent_Slot => 15;

        protected override ulong[] PacketSlotMagicNumber => new ulong[0];
    }
}
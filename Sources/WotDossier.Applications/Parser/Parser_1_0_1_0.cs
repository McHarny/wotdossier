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
    /// 1.0.1.0
    /// </summary>
    public class Parser_1_0_1_0 : Parser_1_0_0_0
    {
	    protected override ulong UpdateEvent_Arena => 67;

	    protected override ulong UpdateEvent_Slot => 29;

        public override ulong UpdateEvent_Health => 7;

        protected override ulong[] PacketSlotMagicNumber => new ulong[]{217};
    }
}
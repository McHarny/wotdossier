using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser_0_9_16_0 : Parser_0_9_15_0
    {
        protected override ulong UpdateEvent_Arena => 0x2A;

	    public override ulong UpdateEvent_Health => 0x04;
	}
}
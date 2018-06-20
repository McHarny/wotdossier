using System;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser_0_9_19_1 : Parser_0_9_17_1
    {
	    protected override ulong UpdateEvent_Arena => 0x2d;

	}
}
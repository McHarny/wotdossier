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
    /// 0.9.20.1.1
    /// </summary>
    public class Parser92011 : Parser920
    {
	    protected override ulong UpdateEvent_Arena => 0x2F;

	    protected override ulong UpdateEvent_Slot => 0x12;
	}
}
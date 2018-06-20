using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    /// <summary>
    /// 0.9.20.1.1
    /// </summary>
    public class Parser_0_9_21_0 : Parser_0_9_20_1_1
    {
	    protected override ulong UpdateEvent_Arena => 52;

	    protected override ulong UpdateEvent_Slot => 14;

        protected virtual ulong[] PacketSlotMagicNumber => new ulong[] {16,4};

        /// <summary>
        /// Process packet 0x08 subType 0x09
        /// Contains slots updates
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="stream">The stream.</param>
        protected override void ProcessPacket_0x08_0x09(Packet packet, MemoryStream stream)
        {
            var subType = stream.Read(1).ConvertLittleEndian();

            if (PacketSlotMagicNumber.Contains(subType)) return;

            packet.Type = PacketType.SlotUpdate;
            //var buffer = new byte[packet.SubTypePayloadLength];
            //Read from your offset to the end of the packet, this will be the "update pickle". 
            //stream.Read(buffer, 0, (int) (packet.SubTypePayloadLength));

            dynamic data = new ExpandoObject();

            packet.Data = data;

            ulong u1 = stream.Read(3).ConvertLittleEndian();

            ulong value = stream.Read(4).ConvertLittleEndian();
            var item = new SlotItem((SlotType)(value & 15), (int)(value >> 4 & 15), (int)(value >> 8 & 65535));

            ulong count = stream.Read(2).ConvertLittleEndian();

            ulong rest = stream.Read(3).ConvertLittleEndian();

            data.Slot = new Slot(item, (int)count, (int)rest);
        }
    }
}
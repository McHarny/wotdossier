namespace WotDossier.Applications.Parser
{
    public class Parser_0_9_1_0 : Parser_0_9_9_0
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0b; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x26; }
        }

	    public override ulong UpdateEvent_Health => 0x06;
    }
}
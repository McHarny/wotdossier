namespace WotDossier.Applications.Parser
{
    public class Parser_0_9_6_0 : BaseParser
    {
        protected override ulong UpdateEvent_Slot
        {
            get { return 0x09; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x21; }
        }
    }
}
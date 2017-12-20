using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Domain.Settings
{
    public enum ExternalFileType
    {
        None = 0,
        ExpectedValuesWN8 = 1,
        ExpectedValuesKttc = 1,
        ExpectedValuesXVM = 2,
        ExpectedValuesPR = 2,
        XvmScale = 2,
        XTE = 2,
        XTDB = 2,

    }


    public class ExternalFiles
    {
        public ExternalFileType FileType { get; set; }

        public string DowloadUrl { get; set; }
        public DateTime LastChecked { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Installed { get; set; }
    }
}

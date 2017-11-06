using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class FirstBlock
    {
        private Version _version;
        private long versionId;

        [DataMember]
        public string dateTime { get; set; }
        [DataMember]
        public string gameplayID { get; set; }
        [DataMember]
        public int battleType { get; set; }
        [DataMember]
        public string mapDisplayName { get; set; }
        //since Version 0.8.6:
        [DataMember]
        public string clientVersionFromXml { get; set; }
        //since Version 0.8.6:
        [DataMember]
        public string clientVersionFromExe { get; set; }

        [IgnoreDataMember]
        public long VersionId
        {
            get
            {
                if (versionId == 0)
                {
                    List<int> ver;
                    if (!string.IsNullOrEmpty(clientVersionFromXml))
                    {
                        var vPos = clientVersionFromXml.IndexOf("v.", StringComparison.Ordinal);
                        var xPos = clientVersionFromXml.IndexOf("#", StringComparison.Ordinal);
                        ver = clientVersionFromXml.Substring(vPos + 2, xPos - vPos - 2).Trim().Split('.')
                            .Select(c => Convert.ToInt32(c)).ToList();
                        for (var i = ver.Count - 1; i < 5; i++)
                        {
                            ver.Add(0);
                        }
                    }
                    else
                        ver = new List<int>{ Version.Major, Version.Minor, Version.Build, Version.Revision};

                    ver.Reverse();
                    for (var i = 0; i < ver.Count; i++)
                    {
                        versionId = versionId + ver[i] * Convert.ToInt64(Math.Pow(100, i + 1));
                    }
                }
                return versionId;
            }
        }

        [IgnoreDataMember]
        public Version Version
        {
            get
            {
                if (_version == null)
                {
                    if (!string.IsNullOrEmpty(clientVersionFromExe))
                    {
                        _version = new Version(clientVersionFromExe.Replace(",", string.Empty).Replace(" ", "."));
                    }
                    else
                    {
                        _version = new Version();
                    }
                }
                return _version;
            }
            set { _version = value; }
        }

	    [DataMember]
        public string mapName { get; set; }
        [DataMember]
        public long playerID { get; set; }
        [DataMember]
        public string playerName { get; set; }
        [DataMember]
        public string playerVehicle { get; set; }
        [DataMember]
        public Dictionary<long, Vehicle> vehicles { get; set; }
        [DataMember]
        public string regionCode { get; set; }
    }
}
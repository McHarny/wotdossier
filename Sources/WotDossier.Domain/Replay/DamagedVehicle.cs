﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Replay
{
    [DataContract]
    public class DamagedVehicle
    {
        [DataMember]
        private int _damageAssisted;

        private int _deathReason = -1;
        private int _killed;
        private List<Device> _tankDamageCrits;
        private List<CrewMember> _crewCrits;
        private List<Device> _tankCrits;
        private int _pierced;
        private int _heHits;
        private int _hits;

        //up to Version 0.8.5: The total number of critical hits scored on this vehicle
        //since Version 0.8.6: Packed value. 
        [DataMember]
        public int crits { get; set; }

        public List<Device> tankDamageCrits
        {
            get
            {
                if (_tankDamageCrits == null)
                {
                    _tankDamageCrits = new List<Device>();
                    int critsFlags = crits >> 12 & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankDamageCrits.Add(device);
                        }
                    }
                }
                return _tankDamageCrits;
            }
        }



        public List<Device> tankCrits
        {
            get
            {
                if (_tankCrits == null)
                {
                    _tankCrits = new List<Device>();
                    int critsFlags = crits & 4095;
                    Array array = Enum.GetValues(typeof(Device));
                    foreach (Device device in array)
                    {
                        if ((critsFlags & (short) device) == (short)device)
                        {
                            _tankCrits.Add(device);
                        }
                    }
                }
                return _tankCrits;
            }
        }

        public List<CrewMember> crewCrits
        {
            get
            {
                if (_crewCrits == null)
                {
                    _crewCrits = new List<CrewMember>();
                    int critsFlags = crits >> 24 & 255;
                    Array array = Enum.GetValues(typeof(CrewMember));
                    foreach (CrewMember member in array)
                    {
                        if ((critsFlags & (short)member) == (short)member)
                        {
                            _crewCrits.Add(member);
                        }
                    }
                }
                return _crewCrits;
            }
        }

        [DataMember]
        public int deathReason
        {
            get { return _deathReason; }
            set { _deathReason = value; }
        }

        [DataMember]
        public int damageAssistedRadio { get; set; }
        [DataMember]
        public int damageAssistedTrack { get; set; }
        [DataMember]
        public int damageAssisted
        {
            get
            {
                int result = damageAssistedRadio + damageAssistedTrack;
                return result > 0 ? result : _damageAssisted;
            }
            set { _damageAssisted = value; }
        }
        [DataMember]
        public int damageDealt { get; set; }
        [DataMember]
        public int fire { get; set; }

        [DataMember]
        public int he_hits
        {
            get
            {
                if (_heHits > 0)
                {
                    return _heHits;
                }
                return explosionHits;
            }
            set { _heHits = value; }
        }

        [DataMember]
        public int hits
        {
            get
            {
                if (_hits > 0)
                {
                    return _hits;
                }
                return directHits;
            }
            set { _hits = value; }
        }

        [DataMember]
        public int directHits { get; set; }
        [DataMember]
        public int explosionHits { get; set; }
        [DataMember]
        //NOTE: Obsolete - "0.8.6"
        public int killed
        {
            get
            {
                if (deathReason >= 0)
                {
                    return 1;
                }
                return _killed;
            }
            set { _killed = value; }
        }

        [DataMember]
        public int pierced
        {
            get
            {
                if (_pierced > 0)
                {
                    return _pierced;
                }
                return piercings;
            }
            set { _pierced = value; }
        }

        [DataMember]
        public int piercings { get; set; }
        [DataMember]
        public int spotted { get; set; }
    }
}
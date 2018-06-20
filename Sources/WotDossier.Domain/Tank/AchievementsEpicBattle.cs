using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class AchievementsEpicBattle : IEpicBattleAchievements
    {
        [DataMember(Name = "occupyingForce")]
        public int OccupyingForce { get; set; }

        [DataMember(Name = "ironShield")]
        public int IronShield { get; set; }

        [DataMember(Name = "generalOfTheArmy")]
        public int GeneralOfTheArmy { get; set; }

        [DataMember(Name = "supremeGun")]
        public int SupremeGun { get; set; }

        [DataMember(Name = "smallArmy")]
        public int SmallArmy { get; set; }
    }
}
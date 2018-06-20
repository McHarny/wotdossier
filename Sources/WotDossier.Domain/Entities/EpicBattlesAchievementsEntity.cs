using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'EpicBattlesAchievements'.
    /// </summary>
    [DataContract]
    public class EpicBattlesAchievementsEntity : EntityBase, IEpicBattleAchievements, IRevised
    {
        [DataMember]
        public virtual int OccupyingForce { get; set; }

        [DataMember]
        public virtual int IronShield { get; set; }

        [DataMember]
        public virtual int GeneralOfTheArmy { get; set; }

        [DataMember]
        public virtual int SupremeGun { get; set; }

        [DataMember]
        public virtual int SmallArmy { get; set; }

    }
}
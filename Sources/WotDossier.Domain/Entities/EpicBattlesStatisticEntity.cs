using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'EpicBattlesStatisticEntity'.
    /// </summary>

    [DataContract]
    public class EpicBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        ///     Gets/Sets the <see cref="EpicBattlesAchievementsEntity" /> object.
        /// </summary>
        [DataMember(Name = "Achievements")]
        public virtual EpicBattlesAchievementsEntity AchievementsIdObject { get; set; } = new EpicBattlesAchievementsEntity();
    }
}
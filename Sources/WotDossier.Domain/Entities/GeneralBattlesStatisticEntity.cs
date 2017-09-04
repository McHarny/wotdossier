﻿using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'GeneralBattlesStatistic'.
    /// </summary>

    [DataContract]
    public class GeneralBattlesStatisticEntity : StatisticEntity
    {
        /// <summary>
        ///     Gets/Sets the <see cref="GeneralBattlesAchievementsEntity" /> object.
        /// </summary>
        [DataMember(Name = "Achievements")]
        public virtual GeneralBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}
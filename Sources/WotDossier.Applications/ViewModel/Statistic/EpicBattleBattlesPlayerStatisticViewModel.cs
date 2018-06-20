using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    /// <summary>
    /// 
    /// </summary>
    public class EpicBattleBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IEpicBattleAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpicBattleBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public EpicBattleBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat)
            : this(stat, new List<StatisticSlice>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpicBattleBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        /// <param name="list">The list.</param>
        public EpicBattleBattlesPlayerStatisticViewModel(RandomBattlesStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IEpicBattleAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}
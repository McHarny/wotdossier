using System;
using System.Collections.Generic;
using TournamentStat.Applications.ViewModel;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace TournamentStat.Applications.BattleModeStrategies
{
    public class RandomStatisticViewStrategy : StatisticViewStrategyBase
    {
        /// <summary>
        /// Predicate to get tank statistic
        /// </summary>
        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.A15x15; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomStatisticViewStrategy"/> class.
        /// </summary>
        /// <param name="dossierRepository">The dossier repository.</param>
        public RandomStatisticViewStrategy(DossierRepository dossierRepository) : base(dossierRepository)
        {
        }

        /// <summary>
        /// Gets the player statistic.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="playerData">The player data.</param>
        /// <returns></returns>
        public override PlayerStatisticViewModel GetPlayerStatistic(PlayerEntity player, List<TankJson> tanks, Player playerData = null)
        {
            return GetPlayerStatistic<RandomBattlesStatisticEntity>(player, tanks, playerData);
        }

        /// <summary>
        /// To the tank statistic row.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="prevStatisticViewModels">The previous statistic view models.</param>
        /// <returns></returns>
        protected override ITankStatisticRow ToTankStatisticRow(TankJson currentStatistic, List<StatisticSlice> prevStatisticViewModels)
        {
            return new RandomBattlesTankStatisticRowViewModel(currentStatistic, prevStatisticViewModels);
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic)
        {
            return new RandomBattlesPlayerStatisticViewModel((RandomBattlesStatisticEntity)currentStatistic);
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="currentStatistic">The current statistic.</param>
        /// <param name="oldStatisticEntities">The old statistic entities.</param>
        /// <returns></returns>
        protected override PlayerStatisticViewModel ToViewModel(StatisticEntity currentStatistic, List<StatisticSlice> oldStatisticEntities)
        {
            return new RandomBattlesPlayerStatisticViewModel((RandomBattlesStatisticEntity)currentStatistic, oldStatisticEntities);
        }

        /// <summary>
        /// Updates the tank statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <returns></returns>
        public override PlayerEntity UpdateTankStatistic(int playerId, List<TankJson> tanks)
        {
            return UpdateTankStatistic<TankRandomBattlesStatisticEntity>(playerId, tanks);
        }

        /// <summary>
        /// Gets the tanks statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks"></param>
        /// <returns></returns>
        public override List<ITankStatisticRow> GetTanksStatistic(int playerId, IEnumerable<TournamentTank> tanks)
        {
            return GetTanksStatistic<TankRandomBattlesStatisticEntity>(playerId, tanks);
        }

        /// <summary>
        /// Updates the player statistic.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tanks">The tanks.</param>
        /// <param name="serverStatistic">The server statistic.</param>
        /// <returns></returns>
        public override PlayerEntity UpdatePlayerStatistic(int playerId, List<TankJson> tanks, Player serverStatistic)
        {
            return DossierRepository.UpdatePlayerStatistic(new RandomBattlesStatAdapter(tanks), serverStatistic, playerId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class EpicBattleBattlesStatAdapter : AbstractStatisticAdapter<RandomBattlesStatisticEntity>, IEpicBattleAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EpicBattleBattlesStatAdapter(List<TankJson> tanks)
            : base(tanks)
        {
            var achievementsEpicBattle = new AchievementsEpicBattle();
            Func<TankJson, AchievementsEpicBattle> achievementsPredicate = tankJson => tankJson.AchievementsEpicBattle ?? achievementsEpicBattle;

            OccupyingForce = tanks.Sum(x => achievementsPredicate(x).OccupyingForce);
            IronShield = tanks.Sum(x => achievementsPredicate(x).IronShield);
            GeneralOfTheArmy = tanks.Sum(x => achievementsPredicate(x).GeneralOfTheArmy);
            SupremeGun = tanks.Sum(x => achievementsPredicate(x).SupremeGun);
            SmallArmy = tanks.Sum(x => achievementsPredicate(x).SmallArmy);
		}

        public List<ITankStatisticRow> Tanks { get; set; }

		#region Achievements

		public int OccupyingForce { get; set; }
	    public int IronShield { get; set; }
	    public int GeneralOfTheArmy { get; set; }
	    public int SupremeGun { get; set; }
	    public int SmallArmy { get; set; }

		#endregion

		public override void Update(RandomBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RandomBattlesAchievementsEntity();
            }

            Mapper.Map<IEpicBattleAchievements>(this, entity.AchievementsIdObject);
        }

        public override Func<TankJson, StatisticJson> Predicate
        {
            get { return tank => tank.EpicBattle ?? new StatisticJson(); }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Common;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications
{
	public static class ReplayExtensions
	{
		public static string GetMapMode(this Domain.Replay.Replay replay)
		{
			var gameplayId = (Gameplay)Enum.Parse(typeof(Gameplay), replay.datablock_1.gameplayID);
			var battleType = (BattleType) replay.datablock_1.battleType;

			List<BattleType> list = new List<BattleType>
			{
				BattleType.Unknown,
				BattleType.Regular,
				BattleType.CompanyWar,
				BattleType.ClanWar,
				BattleType.Event,
				BattleType.Event2
			};

			if (list.Contains(battleType))
			{
				if (gameplayId == Gameplay.ctf)
				{
					return Resources.Resources.BattleType_ctf;
				}
				if (gameplayId == Gameplay.domination)
				{
					return Resources.Resources.BattleType_domination;
				}
				if (gameplayId == Gameplay.nations)
				{
					return Resources.Resources.BattleType_nations;
				}
				if (gameplayId == Gameplay.leviathan)
				{
					return Resources.Resources.BattleType_leviathan;
				}
				return Resources.Resources.BattleType_assault;
			}
			return Resources.Resources.ResourceManager.GetEnumResource(battleType);
		}
	}
}

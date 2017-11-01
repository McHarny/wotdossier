using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Framework;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class DbReplay : ReplayFile
    {


	    private readonly Domain.Replay.Replay _replay;

	    public static DbReplay GetReplay(string jsonData, Guid folderId)
	    {
			JsonSerializerSettings settings = new JsonSerializerSettings();
		    settings.Converters.Add(new PersonalAchievementsConverter());

			return new DbReplay(JsonConvert.DeserializeObject<Domain.Replay.Replay>(jsonData, settings), folderId);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DbReplay" /> class.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public DbReplay(Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            _replay = replay;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public override void Move(ReplayFolder targetFolder)
        {
        }

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            return _replay;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            CompositionContainerFactory.Instance.GetExport<DossierRepository>().DeleteReplay(ReplayId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiaSoft.Jeff.DataAccess;
//using Nejdb;
//using Nejdb.Bson;
using NUnit.Framework;
using WotDossier.Dal.DataAccess;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;
//using BsonDocument = Nejdb.Bson.BsonDocument;

namespace WotDossier.Test
{
	[TestFixture]
	public class MongoTestFixture: TestFixtureBase
	{
		[Test]
		public void ConnectionText()
		{
		    var dsFile = Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db");
			var dsContext = new JeffDbContext(ProviderType.SQLite, $@"Data Source=""{dsFile}""");


			var tankList = dsContext.CreateStatement($"SELECT * FROM Tank").ExecuteReader().OfType<IDataRecord>().Select(rec =>
				new
				{
					TankId = rec.Get("TankId", -1),
					Name = rec.Get("Name", ""),
					Tier = rec.Get("Tier", -1),
					PlayerId = rec.Get("PlayerId", -1),
					CountryId = rec.Get("CountryId", -1),
					TankType = rec.Get("TankType", -1),
					IsPremium = rec.Get("IsPremium", false),
					IsFavorite = rec.Get("IsFavorite", false),
					UniqueId = rec.Get("UniqueId", -1),
					TankUId = rec.Get("TankUId", Guid.Empty)
				}
			).ToList();

			var statList = dsContext.CreateStatement($"SELECT * FROM TankRandomBattlesStatistic").ExecuteReader().OfType<IDataRecord>().Select(rec =>
				new
				{
					TankId = rec.Get("TankId", -1),
					Name = rec.Get("Name", ""),
					Tier = rec.Get("Tier", -1),
					PlayerId = rec.Get("PlayerId", -1),
					CountryId = rec.Get("CountryId", -1),
					TankType = rec.Get("TankType", -1),
					IsPremium = rec.Get("IsPremium", false),
					IsFavorite = rec.Get("IsFavorite", false),
					UniqueId = rec.Get("UniqueId", -1),
					TankUId = rec.Get("TankUId", Guid.Empty)
				}
			).ToList();

//			using (var tCommand = dsContext.CreateStatement($"SELECT * FROM Tank"))
//			{
//			}

//			var dbFile = Path.Combine(Environment.CurrentDirectory, $@"Data\dossier.ejdb");
//			using (var lib = Library.Create())
//			{
//				using (var db = lib.CreateDatabase())
//				{
//					db.Open(dbFile);
//					var tanks = db.CreateCollection("VehicleEntities", new CollectionOptions(false, true, 128000, 0));
//					var tables = new[]
//					{
///*"TankTeamBattleStatistic", "TankHistoricalBattleStatistic",*/ "TankRandomBattlesStatistic"
//					};
//					foreach (var table in tables)
//					{
//						var i = 0;
//						using (var tCommand = dsContext.CreateStatement($"SELECT * FROM {table}"))
//						{
//							Transaction trans = tanks.BeginTransaction();
							
//							foreach (var rec in tCommand.OpenReader())
//							{
//								if (i % 1000 == 0)
//								{
//									trans.Commit();
//									tanks.BeginTransaction();
//								}

//								var data = rec.Get<byte[]>("Raw");
//								if (data == null) continue;

//								var v = new VehicleEntity
//								{
//									Id = rec.Get<int>("Id"),
//									Statistics = CompressHelper.DecompressObject<TankJson>(data)
//								};

//								tanks.Save(v, false);
//								i++;
//							}

//							trans.Commit();

//						}
//					}
//				}

			//}
		}
	}
}

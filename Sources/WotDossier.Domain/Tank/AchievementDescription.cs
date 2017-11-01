using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Common.Extensions;

namespace WotDossier.Domain.Tank
{
	[DataContract]
	public class AchievementDescription
	{
		[DataMember(Name = "name")]
		public string nameString { get; set; }

	}
}
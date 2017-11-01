using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Entities
{
	[DataContract]
	public class VehicleEntity
	{
		/// <summary>
		/// 	Identifier of an entity
		/// </summary>
		[DataMember]
		public virtual int Id { get; set; }

		/// <summary>
		/// 	Gets/Sets version of current entity.
		/// </summary>
		[DataMember]
		public virtual int Version { get; set; }
		/// <summary>
		/// Gets/Sets the field "TankId".
		/// </summary>
		[DataMember]
		public virtual int TankId { get; set; }

		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
		[DataMember]
		public virtual string Name { get; set; }

		/// <summary>
		/// Gets/Sets the field "Tier".
		/// </summary>
		[DataMember]
		public virtual int Tier { get; set; }

		/// <summary>
		/// Gets/Sets the field "CountryId".
		/// </summary>
		[DataMember]
		public virtual int CountryId { get; set; }

		/// <summary>
		/// Gets/Sets the field "TankType".
		/// </summary>
		[DataMember]
		public virtual int TankType { get; set; }

		/// <summary>
		/// Gets/Sets the field "IsPremium".
		/// </summary>
		[DataMember]
		public virtual Boolean IsPremium { get; set; }

		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		[DataMember]
		public virtual int PlayerId { get; set; }

		/// <summary>
		/// Gets or sets the rev.
		/// </summary>
		[DataMember]
		public virtual int UniqueId { get; set; }

		/// <summary>
		/// Gets/Sets the field "IsFavorite".
		/// </summary>
		[DataMember]
		public virtual bool IsFavorite { get; set; }

		/// <summary>
		/// Gets/Sets the field "IsFavorite".
		/// </summary>
		[DataMember]
		public virtual TankJson Statistics { get; set; }

		/// <summary>
		/// Gets or sets the battles count.
		/// </summary>
		[DataMember]
		public virtual int BattlesCount { get; set; }

		/// <summary>
		/// Gets/Sets the field "Updated".
		/// </summary>
		[DataMember]
		public virtual DateTime Updated { get; set; }
	}
}

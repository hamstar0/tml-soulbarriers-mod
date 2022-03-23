using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public AccessBarrier(
					string id,
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle tileArea,
					Color color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base(
					id: id,
					strength: strength,
					maxRegenStrength: maxRegenStrength,
					strengthRegenPerTick: strengthRegenPerTick,
					tileArea: tileArea,
					color: color,
					isSaveable: isSaveable,
					hostType: hostType,
					hostWhoAmI: hostWhoAmI ) {
			this.OnPreBarrierEntityCollision.Add( (ref Entity intruder, ref double damage) => false );
			this.OnPreBarrierBarrierCollision.Add( (Barrier thatBarrier, ref double damage) => false );
			//this.OnPreBarrierRawHit.Add( (ref double damage) => false );

			this.OnPostBarrierEntityCollision.Add( this.OnPostBarrierEntityCollide );
			this.OnPostBarrierBarrierCollision.Add( this.OnPostBarrierBarrierCollide );
		}
	}
}
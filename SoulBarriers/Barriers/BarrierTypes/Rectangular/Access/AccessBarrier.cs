using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public AccessBarrier(
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle tileArea,
					Color color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base(
					strength: strength,
					maxRegenStrength: maxRegenStrength,
					strengthRegenPerTick: strengthRegenPerTick,
					tileArea: tileArea,
					color: color,
					isSaveable: isSaveable,
					hostType: hostType,
					hostWhoAmI: hostWhoAmI ) {
			//this.OnPreBarrierEntityCollision += ( ref Entity intruder ) => true;
			this.OnBarrierEntityCollision.Add( this.OnBarrierEntityCollide );
			this.OnBarrierBarrierCollision.Add( this.OnBarrierBarrierCollide );
		}
	}
}
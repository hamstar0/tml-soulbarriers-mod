using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public override Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create(
				position: position,
				color: this.Color,
				isHit: isHit,
				durationPercentPerTick: BarrierDust.DefaultPercentDurationElapsedPerTick * 2f
			);
		}
	}
}
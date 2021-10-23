using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public bool DecideIfParticleTooFarAwayForFx( Vector2 worldPos ) {
			// any distance is fine, 10% of the time
			if( Main.rand.NextFloat() <= 0.1f ) {
				return false;
			}

			//

			float distSqr = (Main.LocalPlayer.MountedCenter - worldPos).LengthSquared();

			float maxRandDist = 12f * 16f;	// 12 tiles
			float maxRandDistSqr = maxRandDist * maxRandDist;

			return distSqr > Main.rand.NextFloat( maxRandDistSqr );
		}


		////////////////

		public override Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create(
				source: this,
				position: position,
				color: this.Color,
				isHit: isHit,
				durationPercentPerTick: isHit
					? BarrierDust.DefaultPercentDurationElapsedPerTick * 0.25f
					: BarrierDust.DefaultPercentDurationElapsedPerTick * 0.5f
			);
		}
	}
}
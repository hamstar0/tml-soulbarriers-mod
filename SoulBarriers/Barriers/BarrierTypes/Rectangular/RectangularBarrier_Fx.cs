using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int GetMaxAnimationParticleCount() {
			float chunkSize = 12f * 16f;
			float chunksX = (float)this.WorldArea.Width / chunkSize;
			float chunksY = (float)this.WorldArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			int count = (int)( (float)base.GetMaxAnimationParticleCount() * (int)chunks * 2 );

			return Math.Min( count, 300 );
		}


		////////////////

		public bool DecideIfParticleTooFarAwayForFx( Vector2 offset ) {
			// any distance is fine, 10% of the time
			if( Main.rand.NextFloat() <= 0.1f ) {
				return false;
			}

			Vector2 worldPos = offset + this.GetBarrierWorldCenter();
			float distSqr = (Main.LocalPlayer.MountedCenter - worldPos).LengthSquared();

			float maxRandDist = 12f * 16f;
			float maxRandDistSqr = maxRandDist * maxRandDist;

			return distSqr > Main.rand.NextFloat( maxRandDistSqr );
		}


		////////////////

		public override Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create(
				position: position,
				color: this.Color,
				isHit: isHit,
				durationPercentPerTick: BarrierDust.DefaultPercentDurationElapsedPerTick * 0.5f
			);
		}
	}
}
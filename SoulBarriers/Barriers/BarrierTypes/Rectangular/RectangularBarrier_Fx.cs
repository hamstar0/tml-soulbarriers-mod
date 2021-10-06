using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int ComputeCurrentMaxAnimatedParticleCount() {
			float chunkSize = 12f * 16f;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			int count = (int)( (float)base.ComputeCurrentMaxAnimatedParticleCount() * chunks * 2f );

			return Math.Min( count, 300 );
		}

		
		public override int ComputeMaxAnimatableParticleCount() {
			float chunkSize = 12f * 16f;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			int count = (int)( (float)base.ComputeMaxAnimatableParticleCount() * chunks * 2f );

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
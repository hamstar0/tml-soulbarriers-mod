using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int GetParticleCount() {
			float chunkSize = 12f * 16f;
			float chunksX = (float)this.WorldArea.Width / chunkSize;
			float chunksY = (float)this.WorldArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			int count = (int)( (float)base.GetParticleCount() * (int)chunks * 2 );

			return Math.Max( count, 100 );
		}


		////////////////

		public bool DecideIfParticleTooFarAway( Vector2 randOffset ) {
			if( Main.rand.NextFloat() <= 0.1f ) {
				return false;
			}

			Vector2 worldPos = randOffset + this.GetBarrierWorldCenter();
			float distSqr = (Main.LocalPlayer.MountedCenter - worldPos).LengthSquared();

			float maxRandDist = 12f * 16f;
			float maxRandDistSqr = maxRandDist * maxRandDist;

			return distSqr > Main.rand.NextFloat( maxRandDistSqr );
		}


		////////////////

		public override Dust CreateBarrierParticleAt( Vector2 position, Vector2? velocity=null, float scale = 2f / 3f ) {
			Dust dust = base.CreateBarrierParticleAt( position, default(Vector2), scale * 1.5f );
			dust.velocity += new Vector2( 0f, -1f );

			return dust;
		}
	}
}
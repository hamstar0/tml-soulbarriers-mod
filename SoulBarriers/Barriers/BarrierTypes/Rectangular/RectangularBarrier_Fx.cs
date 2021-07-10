using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public bool DecideIfParticleTooFarAway( Vector2 randPos ) {
			if( Main.rand.NextFloat() <= 0.1f ) {
				return false;
			}

			Vector2 worldPos = randPos + this.GetBarrierWorldCenter();
			float distSqr = ( Main.LocalPlayer.MountedCenter - worldPos ).LengthSquared();

			float maxDistSqr = 16f * 16f;
			maxDistSqr *= maxDistSqr;

			return distSqr > Main.rand.NextFloat( maxDistSqr );
		}


		////////////////

		public override Dust CreateBarrierParticleAt( Vector2 position, float scale = 2f / 3f ) {
			Dust dust = base.CreateBarrierParticleAt( position, scale * 2f );
			dust.velocity += new Vector2( 0f, -1f );

			return dust;
		}
	}
}
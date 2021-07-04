using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		public static Vector2 GetRandomOffset( float radius ) {
			float distScale = Main.rand.NextFloat();
			distScale = 1f - ( distScale * distScale * distScale * distScale * distScale );
			distScale *= radius;

			Vector2 offset = Vector2.One.RotatedByRandom( 2d * Math.PI );
			offset *= distScale;

			return offset;
		}



		////////////////

		public override (Dust dust, Vector2 offset) CreateBarrierParticleForArea( Vector2 basePosition ) {
			Vector2 offset = SphericalBarrier.GetRandomOffset( this.Radius );
			Dust dust = this.CreateBarrierParticle( basePosition + offset );

			return (dust, offset);
		}


		////////////////

		public override (Dust dust, Vector2 offset) CreateHitParticleForArea( Vector2 basePosition ) {
			Vector2 offset = SphericalBarrier.GetRandomOffset( this.Radius );
			Dust dust = this.CreateHitParticle( basePosition + offset );

			return (dust, offset);
		}
	}
}
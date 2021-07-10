using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int GetParticleCount() {
			float chunksX = (float)this.WorldArea.Width / (12f * 16f);
			float chunksY = (float)this.WorldArea.Height / (12f * 16f);
			float chunks = chunksX * chunksY;

			return (int)((float)base.GetParticleCount() * chunks);
		}


		////////////////

		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}

		public override Vector2 GetRandomOffsetForArea( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			var randPos = new Vector2(
				Main.rand.Next( -this.WorldArea.Width/2, this.WorldArea.Width/2 ),
				Main.rand.Next( -this.WorldArea.Height/2, this.WorldArea.Height/2 )
			);

			if( isFxOnly ) {
				isFarAway = Main.rand.NextFloat() > 0.15f;

				if( isFarAway ) {
					Vector2 worldPos = randPos + this.GetBarrierWorldCenter();
					float distSqr = (Main.LocalPlayer.MountedCenter - worldPos).LengthSquared();

					float maxDistSqr = 12f * 16f;
					maxDistSqr *= maxDistSqr;

					isFarAway = distSqr > Main.rand.NextFloat( maxDistSqr );
				}
			} else {
				isFarAway = false;
			}

			return randPos;
		}
	}
}
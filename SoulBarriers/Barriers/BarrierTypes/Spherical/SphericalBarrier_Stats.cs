using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public void SetRadius( float radius ) {
			this.Radius = radius;
		}


		////////////////

		public override Vector2 GetBarrierWorldCenter() {
			if( this.HostType == BarrierHostType.Player ) {
				Player plr = (Player)this.Host;
				return plr?.MountedCenter ?? default;
			} else {
				return this.Host?.Center ?? default;
			}
		}

		public override Vector2 GetRandomOffsetWithinAreaForFx( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			isFarAway = false;

			if( isFxOnly ) {
				float maxDist = 256 * 16;
				float maxDistSqr = maxDist * maxDist;

				if( (Main.LocalPlayer.MountedCenter - origin).LengthSquared() > maxDistSqr ) {
					isFarAway = true;
				}
			}

			float distScale = Main.rand.NextFloat();
			if( isFxOnly ) {
				distScale = 1f - ( distScale * distScale * distScale * distScale * distScale );
			} else {
				distScale = 1f - distScale;
			}
			distScale *= this.Radius;

			Vector2 offset = Vector2.One.RotatedByRandom( 2d * Math.PI );
			offset *= distScale;

			return offset;
		}
	}
}
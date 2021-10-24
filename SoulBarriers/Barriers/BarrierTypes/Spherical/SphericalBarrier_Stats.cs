using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public void SetRadius( float radius ) {
			this.Radius = radius;
		}


		////////////////

		public override Vector2 GetBarrierWorldCenter() {
			if( this.HostType == BarrierHostType.Player ) {
				Player plr = (Player)this.Host;
//LogLibraries.LogOnce( "SphericalBarrier "+this.IsActive+", center: "+plr?.MountedCenter, false );
				return plr?.MountedCenter ?? default;
			} else {
				return this.Host?.Center ?? default;
			}
		}

		////

		public override Vector2 GetRandomWorldPositionWithinArea( Vector2 worldCenter, bool fxOnly ) {
			float distScale = Main.rand.NextFloat();
			if( fxOnly ) {
				distScale = 1f - (distScale * distScale * distScale * distScale * distScale);
			} else {
				distScale = 1f - distScale;
			}

			Vector2 randDir = new Vector2(1f, 0f)
				.RotatedByRandom( 2d * Math.PI );

			return worldCenter + randDir * distScale * this.Radius;
		}

		public override Vector2? GetRandomWorldPositionWithinAreaOnScreen(
					Vector2 worldCenter,
					bool fxOnly,
					out bool isFarAway ) {
			var scrRect = new Rectangle(
				(int)Main.screenPosition.X,
				(int)Main.screenPosition.Y,
				Main.screenWidth,
				Main.screenHeight
			);

			int halfRad = (int)(this.Radius * 0.5f);	// approx
			if( worldCenter.X < (scrRect.Left - halfRad)
					|| worldCenter.X > (scrRect.Right + halfRad)
					|| worldCenter.Y < (scrRect.Top - halfRad)
					|| worldCenter.Y > (scrRect.Bottom + halfRad) ) {
//DebugLibraries.Print( "SPHERE FAIL 1", "rect: "+scrRect+", wld: "+worldCenter+", halfRad: "+halfRad );
				isFarAway = true;
				return null;
			}

			Vector2 pos, offset;

			do {
				float distScale = Main.rand.NextFloat();
				if( fxOnly ) {
					distScale = 1f - (distScale * distScale * distScale * distScale * distScale);
				} else {
					distScale = 1f - distScale;
				}

				Vector2 randDir = new Vector2(1f, 0f)
					.RotatedByRandom( 2d * Math.PI );

				offset = randDir * distScale * this.Radius;
				pos = worldCenter + offset;
			} while( !scrRect.Contains( pos.ToPoint() ) );    // ugh

			//

			float maxDist = 256 * 16;	// 256 tiles = too far
			float maxDistSqr = maxDist * maxDist;
			float distSqr = offset.LengthSquared();

			isFarAway = distSqr > maxDistSqr;

			//
			
			return pos;
		}
	}
}
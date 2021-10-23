using System;
using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
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
//LogLibraries.LogOnce( "SphericalBarrier "+this.IsActive+", center: "+plr?.MountedCenter, false );
				return plr?.MountedCenter ?? default;
			} else {
				return this.Host?.Center ?? default;
			}
		}

		////

		public override Vector2 GetRandomWorldPositionWithinArea( Vector2 origin, bool fxOnly ) {
			float distScale = Main.rand.NextFloat();
			if( fxOnly ) {
				distScale = 1f - ( distScale * distScale * distScale * distScale * distScale );
			} else {
				distScale = 1f - distScale;
			}

			Vector2 randDir = new Vector2(1f, 0f)
				.RotatedByRandom( 2d * Math.PI );

			return randDir * distScale * this.Radius;
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

			int halfRad = (int)(this.Radius / 2f) - 1;
			if( worldCenter.X <= (scrRect.Right-halfRad)
					|| worldCenter.X >= (scrRect.Left+halfRad)
					|| worldCenter.Y <= (scrRect.Top-halfRad)
					|| worldCenter.Y >= (scrRect.Bottom+halfRad) ) {
				isFarAway = true;
				return null;
			}

			Vector2 pos;

			do {
				float distScale = Main.rand.NextFloat();
				if( fxOnly ) {
					distScale = 1f - ( distScale * distScale * distScale * distScale * distScale );
				} else {
					distScale = 1f - distScale;
				}

				Vector2 randDir = new Vector2(1f, 0f)
					.RotatedByRandom( 2d * Math.PI );

				pos = randDir * distScale * this.Radius;
			} while( !scrRect.Contains(pos.ToPoint()) );    // ugh

			//

			float maxDist = 256 * 16;	// 256 tiles = too far
			float maxDistSqr = maxDist * maxDist;
			isFarAway = (Main.LocalPlayer.MountedCenter - pos).LengthSquared() > maxDistSqr;

			//

			return pos;
		}
	}
}
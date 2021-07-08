using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		public float Radius { get; private set; }



		////////////////

		public SphericalBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					float radius,
					BarrierColor color
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.Radius = radius;
		}


		////////////////

		public void SetRadius( float radius ) {
			this.Radius = radius;
		}


		////////////////

		public override Vector2? GetRandomOffsetForArea( Vector2 origin, bool isFxOnly ) {
			if( isFxOnly ) {
				float maxDist = 256 * 16;
				float maxDistSqr = maxDist * maxDist;

				if( (Main.LocalPlayer.MountedCenter - origin).LengthSquared() > maxDistSqr ) {
					return null;
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

		public override Vector2 GetBarrierWorldCenter() {
			if( this.HostType == BarrierHostType.Player ) {
				Player plr = (Player)this.Host;
				return plr?.MountedCenter ?? default;
			} else {
				return this.Host?.Center ?? default;
			}
		}
	}
}
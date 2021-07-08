using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		public Rectangle WorldArea { get; private set; }



		////////////////

		public RectangularBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					Rectangle worldArea,
					BarrierColor color
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.WorldArea = worldArea;
		}


		////////////////

		public override int GetParticleCount() {
			int chunksX = this.WorldArea.Width / (128*16);
			int chunksY = this.WorldArea.Height / (128*16);
			int chunks = chunksX * chunksY;

			return base.GetParticleCount() * chunks;
		}


		////////////////

		public override Vector2? GetRandomOffsetForArea( Vector2 origin, bool isFxOnly ) {
			var pos = new Vector2(
				Main.rand.NextFloat( this.WorldArea.Width ),
				Main.rand.NextFloat( this.WorldArea.Height )
			);

			if( isFxOnly ) {
				float distSqr = (Main.LocalPlayer.MountedCenter - pos).LengthSquared();
				float maxDistSqr = 96f * 16f;
				maxDistSqr *= maxDistSqr;

				if( distSqr > Main.rand.NextFloat(maxDistSqr) ) {
					return null;
				}
			}

			return pos;
		}

		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}
	}
}
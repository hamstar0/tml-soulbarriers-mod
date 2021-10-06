using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override Vector2 GetBarrierWorldCenter() {
			return this.TileArea.Center.ToVector2();
		}

		public override Vector2 GetRandomOffsetWithinAreaForFx( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			var randOffset = new Vector2(
				Main.rand.Next( -this.TileArea.Width/2, this.TileArea.Width/2 ),
				Main.rand.Next( -this.TileArea.Height/2, this.TileArea.Height/2 )
			);

			if( isFxOnly ) {
				isFarAway = this.DecideIfParticleTooFarAwayForFx( randOffset );
			} else {
				isFarAway = false;
			}

			return randOffset;
		}
	}
}
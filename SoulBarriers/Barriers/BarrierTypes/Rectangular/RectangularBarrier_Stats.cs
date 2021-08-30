using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}

		public override Vector2 GetRandomOffsetWithinAreaForFx( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			var randOffset = new Vector2(
				Main.rand.Next( -this.WorldArea.Width/2, this.WorldArea.Width/2 ),
				Main.rand.Next( -this.WorldArea.Height/2, this.WorldArea.Height/2 )
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
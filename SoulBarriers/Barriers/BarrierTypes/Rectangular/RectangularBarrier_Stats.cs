using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}

		public override Vector2 GetRandomOffsetWithinArea( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			Rectangle wldArea = this.WorldArea;
			var randOffset = new Vector2(
				Main.rand.Next( -wldArea.Width/2, wldArea.Width/2 ),
				Main.rand.Next( -wldArea.Height/2, wldArea.Height/2 )
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
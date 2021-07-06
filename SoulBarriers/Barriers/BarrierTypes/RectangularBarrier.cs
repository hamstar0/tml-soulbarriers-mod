using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		public Rectangle WorldArea { get; private set; }



		////////////////

		public RectangularBarrier( Rectangle worldArea, BarrierColor color ) : base( color ) {
			this.WorldArea = worldArea;
		}


		////////////////

		public override Vector2 GetRandomOffsetForArea() {
			return new Vector2(
				Main.rand.NextFloat( this.WorldArea.Width ),
				Main.rand.NextFloat( this.WorldArea.Height )
			);
		}
	}
}
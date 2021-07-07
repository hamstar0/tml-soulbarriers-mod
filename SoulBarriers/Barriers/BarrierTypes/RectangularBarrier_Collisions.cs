using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		public override bool IsHostedCollidingDirectly( Entity intruder ) {
			throw new NotImplementedException( "Rectangular barriers cannot have hosts (currently)" );
		}


		public override bool IsHostlessCollidingDirectly( Entity intruder ) {
			var rect = new Rectangle( (int)intruder.position.X, (int)intruder.position.Y, intruder.width, intruder.height );

			return this.WorldArea.Intersects( rect );
		}
	}
}
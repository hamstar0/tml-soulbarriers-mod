using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		public override bool IsHostedCollidingDirectly( Entity host, Entity intruder ) {
			throw new NotImplementedException( "Rectangular barriers cannot have hosts (currently)" );
		}

		public override bool IsHostlessCollidingDirectly( Entity intruder ) {
			f
		}
	}
}
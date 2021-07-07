using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		public override bool IsHostedCollidingDirectly( Entity intruder ) {
			//bool intersects = host.GetRectangle()
			//	.Intersects( intruder.GetRectangle() );
			//if( intersects ) {
			//	return true;
			//}

			Vector2 origin = this.GetBarrierWorldCenter();
			int leastDim = intruder.width < intruder.height
				? intruder.width
				: intruder.height;

			float dist = (origin - intruder.Center).Length() - (float)(leastDim / 2);

			//Main.NewText("3 "+((Projectile)intruder).Name+" "+intersects );
			return dist < this.Radius;
		}


		public override bool IsHostlessCollidingDirectly( Entity intruder ) {
			throw new NotImplementedException( "Spherical barriers must have hosts (currently)" );
		}
	}
}
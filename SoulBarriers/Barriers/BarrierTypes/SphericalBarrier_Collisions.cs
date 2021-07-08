using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		public override bool IsCollidingDirectly( Entity intruder ) {
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


		public override bool IsBarrierColliding( Barrier barrier ) {
			Vector2 myPos = this.GetBarrierWorldCenter();

			if( barrier is SphericalBarrier ) {
				Vector2 diff = barrier.GetBarrierWorldCenter() - myPos;
				float joinedRadii = this.Radius + ((SphericalBarrier)barrier).Radius;

				return diff.LengthSquared() < (joinedRadii * joinedRadii);
			} else if( barrier is RectangularBarrier ) {
				return Barrier.IsSphereCollidingRectangle(
					(myPos.X, myPos.Y, this.Radius),
					((RectangularBarrier)barrier).WorldArea
				);
			} else {
				return barrier.IsBarrierColliding( this );
			}
		}
	}
}
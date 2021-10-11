using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Barriers.BarrierTypes.Spherical;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override bool IsEntityCollidingPhysically( Entity intruder ) {
			var entRect = new Rectangle(
				(int)intruder.position.X,
				(int)intruder.position.Y,
				intruder.width,
				intruder.height
			);

			return this.WorldArea.Intersects( entRect );
		}

		public override bool IsBarrierColliding( Barrier barrier ) {
			if( barrier is RectangularBarrier ) {
				return ((RectangularBarrier)barrier).TileArea
					.Intersects( this.TileArea );
			} else if( barrier is SphericalBarrier ) {
				var sphBarrier = (SphericalBarrier)barrier;
				Vector2 sphPos = barrier.GetBarrierWorldCenter();

				return Barrier.IsSphereCollidingRectangle(
					(sphPos.X, sphPos.Y, sphBarrier.Radius),
					this.WorldArea
				);
			}

			return false;
		}
	}
}
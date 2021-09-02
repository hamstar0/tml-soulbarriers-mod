using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		/// Credit: https://stackoverflow.com/a/1879223/14068996
		public static bool IsSphereCollidingRectangle( (float X, float Y, float R) circle, Rectangle rect ) {
			// Find the closest point to the circle within the rectangle
			float closestX = MathHelper.Clamp( circle.X, rect.Left, rect.Right );
			float closestY = MathHelper.Clamp( circle.Y, rect.Top, rect.Bottom );

			// Calculate the distance between the circle's center and this closest point
			float distanceX = circle.X - closestX;
			float distanceY = circle.Y - closestY;

			// If the distance is less than the circle's radius, an intersection occurs
			float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
			return distanceSquared < (circle.R * circle.R);
		}



		////////////////

		internal void CheckCollisionsAgainstBarriers( IEnumerable<Barrier> barriers ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}
			if( !this.IsActive ) {
				return;
			}

			foreach( Barrier barrier in barriers ) {
				if( barrier == this ) {
					continue;
				}
				if( !barrier.IsActive ) {
					continue;
				}
/*if( this is SphericalBarrier ) {
	DebugLibraries.Print(
		"b_v_b_"+this.GetID()+" v "+barrier.GetID(),
		"colliding? "+this.IsBarrierColliding(barrier)
	);
}*/
				if( this.IsBarrierColliding(barrier) ) {
					this.ApplyBarrierCollisionHitIf( barrier, true );
				}
			}
		}


		////////////////

		public abstract bool IsBarrierColliding( Barrier barrier );
	}
}
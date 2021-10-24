using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}

		////

		public override Vector2 GetRandomWorldPositionWithinArea( Vector2 origin, bool _fxOnly ) {
			Rectangle wldArea = this.WorldArea;
			var randOffset = new Vector2(
				Main.rand.Next( -wldArea.Width/2, wldArea.Width/2 ),
				Main.rand.Next( -wldArea.Height/2, wldArea.Height/2 )
			);

			return randOffset;
		}

		public override Vector2? GetRandomWorldPositionWithinAreaOnScreen(
					Vector2 _worldCenter,
					bool _fxOnly,
					out bool isFarAway ) {
			Rectangle wldArea = this.WorldArea;
			int minX = wldArea.Left;
			int minY = wldArea.Top;
			int maxX = wldArea.Right;
			int maxY = wldArea.Bottom;

			int scrMaxX = (int)Main.screenPosition.X + Main.screenWidth;
			int scrMaxY = (int)Main.screenPosition.Y + Main.screenHeight;

			if( minX < Main.screenPosition.X ) {
				if( minX < scrMaxX ) {
					minX = (int)Main.screenPosition.X;
				} else {
					isFarAway = true;
					return null;
				}
			}
			if( maxX > scrMaxX ) {
				if( maxX >= Main.screenPosition.X ) {
					maxX = scrMaxX;
				} else {
					isFarAway = true;
					return null;
				}
			}

			if( minY < Main.screenPosition.Y ) {
				if( minY < scrMaxY ) {
					minY = (int)Main.screenPosition.Y;
				} else {
					isFarAway = true;
					return null;
				}
			}
			if( maxY > scrMaxY ) {
				if( maxY >= Main.screenPosition.Y ) {
					maxY = scrMaxY;
				} else {
					isFarAway = true;
					return null;
				}
			}

			if( minX >= maxX || minY >= maxY ) {
				isFarAway = true;
				return null;
			}

			//
			
			var pos = new Vector2(
				Main.rand.Next( minX, maxX ),
				Main.rand.Next( minY, maxY )
			);

			//

			isFarAway = this.DecideIfParticleTooFarAwayForFx( pos );

			return pos;
		}
	}
}
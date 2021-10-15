using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public static ISet<(int tileX, int tileY)> GetTilesWithinCircle( int left, int right, int top, int bottom ) {
			var tiles = new HashSet<(int, int)>();

			int radius = (right - left) / 2;
			float radTileSqr = radius * radius;

			int midTileX = left + (radius / 2);
			int midTileY = right + (radius / 2);

			for( int x=left; x<midTileX; x++ ) {
				for( int y=top; y<midTileY; y++ ) {
					int xDiff = x - left;
					int yDiff = y - top;
					int distSqr = (xDiff * xDiff) + (yDiff * yDiff);

					if( distSqr < radTileSqr ) {
						int x2 = right - xDiff - 1;
						int y2 = bottom - yDiff - 1;
						tiles.Add( (x, y) );
						tiles.Add( (x2, y) );
						tiles.Add( (x, y2) );
						tiles.Add( (x2, y2) );
					}
				}
			}

			return tiles;
		}



		////////////////

		public float Radius { get; private set; }



		////////////////

		public SphericalBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					int? maxRegenStrength,
					float strengthRegenPerTick,
					float radius,
					Color color
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.Radius = radius;
		}


		////////////////

		public override bool CanSave() {
			return false;
		}


		////////////////

		public override ISet<(int tileX, int tileY)> GetTilesUponBarrier( float worldPadding ) {
			Vector2 wldCenter = this.GetBarrierWorldCenter();
			float rad = this.Radius + worldPadding;
			float wldLeft = wldCenter.X - rad;
			float wldRight = wldCenter.X + rad;
			float wldTop = wldCenter.Y - rad;
			float wldBot = wldCenter.Y + rad;
			int lTile = (int)Math.Ceiling( wldLeft / 16f );
			int rTile = (int)Math.Floor( wldRight / 16f );
			int tTile = (int)Math.Ceiling( wldTop / 16f );
			int bTile = (int)Math.Floor( wldBot / 16f );

			return SphericalBarrier.GetTilesWithinCircle( lTile, rTile, tTile, bTile );
		}


		////////////////

		public override string GetID() {
			return base.GetID()+" - R"+this.Radius;
		}
	}
}
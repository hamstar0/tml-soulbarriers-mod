using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public static ISet<(int tileX, int tileY)> GetTilesWithinCircle( float radius, Vector2 wldCenter ) {
			float wldLeft = wldCenter.X - radius;
			float wldTop = wldCenter.Y - radius;

			int midX = (int)wldCenter.X / 16;
			int midY = (int)wldCenter.Y / 16;
			int left = (int)wldLeft / 16;
			int top = (int)wldTop / 16;

			var tiles = new HashSet<(int, int)>();

			float tileRad = radius / 16f;
			int radTileSqr = (int)(tileRad * tileRad);

			for( int x=left; x<=midX; x++ ) {
				for( int y=top; y<=midY; y++ ) {
					int xDiff = midX - x;
					int yDiff = midY - y;

					int distSqr = (xDiff * xDiff) + (yDiff * yDiff);
					if( distSqr > radTileSqr ) {
						continue;
					}

					int x2 = midX + xDiff;
					int y2 = midY + yDiff;

					tiles.Add( (x, y) );
					tiles.Add( (x2, y) );
					tiles.Add( (x, y2) );
					tiles.Add( (x2, y2) );
				}
			}

			return tiles;
		}



		////////////////

		public float Radius { get; private set; }



		////////////////

		public SphericalBarrier(
					string id,
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					int? maxRegenStrength,
					float strengthRegenPerTick,
					float radius,
					Color color
				) : base( id, hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.Radius = radius;
		}


		////////////////

		public override bool CanSave() {
			return false;
		}


		////////////////

		public override ISet<(int tileX, int tileY)> GetTilesUponBarrier( float worldPadding ) {
			return SphericalBarrier.GetTilesWithinCircle( this.Radius + worldPadding, this.GetBarrierWorldCenter() );
		}


		////////////////

		/*public override string GetID() {
			return base.GetID()+" - R"+this.Radius;
		}*/
	}
}
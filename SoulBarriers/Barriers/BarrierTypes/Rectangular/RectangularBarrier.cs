using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		private bool IsSaveable;


		////////////////

		public Rectangle TileArea { get; private set; }

		////

		public Rectangle WorldArea => new Rectangle(
			this.TileArea.X*16,
			this.TileArea.Y*16,
			this.TileArea.Width*16,
			this.TileArea.Height*16
		);



		////////////////

		public RectangularBarrier(
					string id,
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle tileArea,
					Color color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base( id, hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.TileArea = tileArea;
			this.IsSaveable = isSaveable;
		}


		////////////////

		public override bool CanSave() {
			return this.IsSaveable;
		}


		////////////////

		public override ISet<(int tileX, int tileY)> GetTilesUponBarrier( float worldPadding ) {
			var tiles = new HashSet<(int, int)>();

			int tilePadding = (int)worldPadding / 16;
			int left = this.TileArea.Left - tilePadding;
			int top = this.TileArea.Top - tilePadding;
			int right = this.TileArea.Right + tilePadding;
			int bot = this.TileArea.Bottom + tilePadding;

			for( int x=left; x<right; x++ ) {
				for( int y=top; y<bot; y++ ) {
					tiles.Add( (x, y) );
				}
			}

			return tiles;
		}


		////////////////

		/*public override string GetID() {
			return base.GetID()+" - A"+this.TileArea.ToString();
		}*/
	}
}
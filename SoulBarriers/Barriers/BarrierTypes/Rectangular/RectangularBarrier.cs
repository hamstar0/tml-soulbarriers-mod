using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		private bool IsSaveable;


		////////////////

		public Rectangle TileArea { get; private set; }



		////////////////

		public RectangularBarrier(
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle tileArea,
					Color color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.TileArea = tileArea;
			this.IsSaveable = isSaveable;
		}


		////////////////

		public override bool CanSave() {
			return this.IsSaveable;
		}


		////////////////

		public override ISet<(int tileX, int tileY)> GetTilesUponBarrier() {
			var tiles = new HashSet<(int, int)>();

			int right = this.TileArea.Right;
			int bot = this.TileArea.Bottom;

			for( int x=this.TileArea.Left; x<right; x++ ) {
				for( int y=this.TileArea.Top; y<bot; y++ ) {
					tiles.Add( (x, y) );
				}
			}

			return tiles;
		}


		////////////////

		public override string GetID() {
			return this.GetID()+" - "+this.TileArea.ToString();
		}
	}
}
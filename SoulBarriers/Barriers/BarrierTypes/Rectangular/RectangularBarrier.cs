using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		private bool IsSaveable;


		////////////////

		public Rectangle WorldArea { get; private set; }



		////////////////

		public RectangularBarrier(
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle worldArea,
					Color color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.WorldArea = worldArea;
			this.IsSaveable = isSaveable;
		}


		////////////////

		public override bool CanSave() {
			return this.IsSaveable;
		}


		////////////////

		public override ISet<(int tileX, int tileY)> GetTilesUponBarrier() {
			var tiles = new HashSet<(int, int)>();

			int right = this.WorldArea.Right;
			int bot = this.WorldArea.Bottom;

			for( int x=this.WorldArea.Left; x<right; x++ ) {
				for( int y=this.WorldArea.Top; y<bot; y++ ) {
					tiles.Add( (x, y) );
				}
			}

			return tiles;
		}


		////////////////

		public override string GetID() {
			return (int)this.HostType+":"+this.HostWhoAmI+","+this.WorldArea.ToString();
		}
	}
}
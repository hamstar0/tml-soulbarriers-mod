using System;
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
					BarrierColor color,
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

		public override string GetID() {
			return (int)this.HostType+":"+this.HostWhoAmI+","+this.WorldArea.ToString();
		}
	}
}
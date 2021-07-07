using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		public Rectangle WorldArea { get; private set; }



		////////////////

		public RectangularBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					Rectangle worldArea,
					BarrierColor color
				) : base( hostType, hostWhoAmI, strength, maxRegenStrength, strengthRegenPerTick, color ) {
			this.WorldArea = worldArea;
		}


		////////////////

		public override Vector2 GetRandomOffsetForArea() {
			return new Vector2(
				Main.rand.NextFloat( this.WorldArea.Width ),
				Main.rand.NextFloat( this.WorldArea.Height )
			);
		}

		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
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

		public override string GetID() {
			return (int)this.HostType+":"+this.HostWhoAmI;
		}
	}
}
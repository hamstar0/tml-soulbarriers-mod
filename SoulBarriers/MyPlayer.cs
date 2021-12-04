using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		public Barrier Barrier { get; private set; }

		public int BarrierImmunityTimer { get; internal set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////
		
		public override void PreUpdate() {
			this.UpdatePersonalBarrier();

			if( !this.player.dead ) {
				if( this.BarrierImmunityTimer >= 1 ) {
					this.BarrierImmunityTimer--;
				}
			}

			this.AnimateBarrierFxIf();
		}
	}
}
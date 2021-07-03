using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		private Barrier Barrier = new Barrier( 48f, BarrierColor.BigBlue );

		private bool IsCharging = false;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////
		
		public override void DrawEffects(
					PlayerDrawInfo drawInfo,
					ref float r,
					ref float g,
					ref float b,
					ref float a,
					ref bool fullBright ) {
			if( this.Barrier.Strength >= 1 ) {
				int particles = this.Barrier.GetParticleCount();

				this.Barrier.AnimateAt( particles, this.player.MountedCenter, this.player.velocity );
			}
		}


		////////////////

		public override void PreUpdate() {
			this.UpdateBarrier();
		}
	}
}
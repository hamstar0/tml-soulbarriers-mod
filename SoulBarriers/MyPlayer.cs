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

			this.AnimateBarrierFx();
		}


		////////////////

		/*public override void DrawEffects(
					PlayerDrawInfo drawInfo,
					ref float r,
					ref float g,
					ref float b,
					ref float a,
					ref bool fullBright ) {
			if( this.Barrier != null && this.Barrier.IsActive ) {
				int particles = this.Barrier.ComputeCappedNormalParticleCount();

				this.Barrier.Animate( particles );
			}
		}*/

		private void AnimateBarrierFx() {
			if( this.Barrier != null && this.Barrier.IsActive ) {
				int particles = this.Barrier.ComputeCappedNormalParticleCount();

				this.Barrier.Animate( particles );
			}
		}
	}
}
using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Items;


namespace SoulBarriers {
	class SoulBarriersPlayer : ModPlayer {
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
			if( !this.IsCharging ) {
				if( this.player.HeldItem?.active == true && this.player.HeldItem.type == ModContent.ItemType<PBGItem>() ) {
					if( this.player.itemTime >= 1 ) {
						this.IsCharging = true;

						this.Barrier.SetStrength( 0 );
					}
				}
			} else {
				if( this.player.HeldItem?.active != true || this.player.itemTime <= 0 ) {
					this.IsCharging = false;
				}
			}

			SoulBarriersMod.BarrierMngr.UpdateBarrier( this.player, this.Barrier );
		}


		////////////////

		public int GetBarrierStrength() {
			return this.Barrier.Strength;
		}

		public void AddBarrier( int strength ) {
			var config = SoulBarriersConfig.Instance;

			this.Barrier.SetRadius( config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) ) );
			this.Barrier.SetStrength( this.Barrier.Strength + strength );
		}
	}
}
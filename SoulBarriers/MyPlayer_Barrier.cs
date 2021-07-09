using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Items;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		private void UpdatePersonalBarrier() {
			if( this.Barrier == null ) {
				this.Barrier = BarrierManager.Instance.GetOrMakePlayerBarrier( this.player.whoAmI );
			}

			if( this.player.dead ) {
				if( this.Barrier.Strength >= 1 ) {
					this.Barrier.SetStrength( 0 );
				}
			} else {
				this.UpdatePersonalBarrierCharging();
			}
		}


		private void UpdatePersonalBarrierCharging() {
			if( !this.IsBarrierCharging ) {
				if( this.player.HeldItem?.active == true && this.player.HeldItem.type == ModContent.ItemType<PBGItem>() ) {
					if( this.player.itemTime >= 1 ) {
						this.IsBarrierCharging = true;

						this.Barrier.SetStrength( 0 );
					}
				}
			} else {
				if( this.player.HeldItem?.active != true || this.player.itemTime <= 0 ) {
					this.IsBarrierCharging = false;
				}
			}
		}


		////////////////

		public int GetBarrierStrength() {
			return this.Barrier.Strength;
		}

		////

		public void AddBarrier( int strength ) {
			var config = SoulBarriersConfig.Instance;
			float radius = config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) );

			if( this.Barrier is SphericalBarrier ) {
				((SphericalBarrier)this.Barrier).SetRadius( radius );
			}

			this.Barrier.SetStrength( this.Barrier.Strength + strength );
		}
	}
}
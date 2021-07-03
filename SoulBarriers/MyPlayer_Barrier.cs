using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Items;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		private void UpdateBarrier() {
			if( this.player.dead ) {
				if( this.Barrier.Strength >= 1 ) {
					this.Barrier.SetStrength( 0 );
				}

				return;
			}

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

			SoulBarriersMod.BarrierMngr.TrackBarrier( this.player, this.Barrier );
		}


		////////////////

		public int GetBarrierStrength() {
			return this.Barrier.Strength;
		}

		////

		public void AddBarrier( int strength ) {
			var config = SoulBarriersConfig.Instance;

			this.Barrier.SetRadius( config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) ) );
			this.Barrier.SetStrength( this.Barrier.Strength + strength );
		}
	}
}
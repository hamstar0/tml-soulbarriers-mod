using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Items;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		private void UpdateBarrier() {
			if( this.player.dead ) {
				if( this.Barrier.Strength >= 1 ) {
					this.Barrier.SetStrength( this.player, 0 );
				}

				return;
			}

			if( !this.IsBarrierCharging ) {
				if( this.player.HeldItem?.active == true && this.player.HeldItem.type == ModContent.ItemType<PBGItem>() ) {
					if( this.player.itemTime >= 1 ) {
						this.IsBarrierCharging = true;

						this.Barrier.SetStrength( this.player, 0 );
					}
				}
			} else {
				if( this.player.HeldItem?.active != true || this.player.itemTime <= 0 ) {
					this.IsBarrierCharging = false;
				}
			}

			BarrierManager.Instance.TrackPlayerBarrier( this.player, this.Barrier );
		}


		////////////////

		public int GetBarrierStrength() {
			return this.Barrier.Strength;
		}

		////

		public void AddBarrier( int strength ) {
			var config = SoulBarriersConfig.Instance;

			this.Barrier.SetRadius( config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) ) );
			this.Barrier.SetStrength( this.player, this.Barrier.Strength + strength );
		}
	}
}
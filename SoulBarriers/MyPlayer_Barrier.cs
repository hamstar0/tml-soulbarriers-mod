using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


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
			}
		}
	}
}
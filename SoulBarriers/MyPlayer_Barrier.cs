using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		private void UpdatePersonalBarrier() {
			if( this.Barrier == null ) {
				this.Barrier = BarrierManager.Instance.GetPlayerBarrier( this.player.whoAmI );
				if( this.Barrier == null ) {
					this.Barrier = BarrierManager.Instance.CreateAndDeclarePlayerBarrier( this.player.whoAmI );
				}
			}

			if( this.player.dead ) {
				if( this.Barrier.Strength > 0d ) {
					this.Barrier.SetStrength( 0, true, true );
				}
			}
		}
	}
}
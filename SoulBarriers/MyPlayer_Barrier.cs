using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes.Spherical;


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


		////////////////

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
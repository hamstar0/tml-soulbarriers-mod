using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SpherericalBarrier : Barrier {
		private void ApplyDebuffHit( Player hostPlayer, int buffIdx ) {
			var config = SoulBarriersConfig.Instance;

			hostPlayer.DelBuff( buffIdx );

			int dmg = config.Get<int>( nameof( config.BarrierDebuffRemovalCost ) );
			this.SetStrength( hostPlayer, this.Strength - dmg );

			Vector2 origin = Barrier.GetEntityBarrierOrigin( hostPlayer );

			this.CreateHitParticlesForArea( origin, dmg * 4 );
		}
	}
}
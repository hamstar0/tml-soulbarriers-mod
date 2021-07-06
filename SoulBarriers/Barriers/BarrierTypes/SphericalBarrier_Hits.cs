using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		private void ApplyDebuffHit( Player hostPlayer, int buffIdx, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			var config = SoulBarriersConfig.Instance;
			int buffType = hostPlayer.buffType[buffIdx];

			hostPlayer.DelBuff( buffIdx );

			int dmg = config.Get<int>( nameof( config.BarrierDebuffRemovalCost ) );
			this.SetStrength( hostPlayer, this.Strength - dmg );

			Vector2 origin = Barrier.GetEntityBarrierOrigin( hostPlayer );

			int particles = Barrier.GetHitParticleCount( dmg * 4 );

			this.CreateHitParticlesForArea( origin, particles );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitPacket.Broadcast( hostPlayer, dmg, buffType );
				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
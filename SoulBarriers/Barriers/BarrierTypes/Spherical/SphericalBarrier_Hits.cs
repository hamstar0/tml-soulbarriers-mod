using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		private void ApplyDebuffHit( Player hostPlayer, int buffIdx, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			var config = SoulBarriersConfig.Instance;
			int buffType = hostPlayer.buffType[buffIdx];

			hostPlayer.DelBuff( buffIdx );

			int damage = config.Get<int>( nameof( config.BarrierDebuffRemovalCost ) );
			this.SetStrength( this.Strength - damage );

			Vector2 origin = this.GetBarrierWorldCenter();

			int particles = Barrier.GetHitParticleCount( damage * 4 );

			this.CreateHitParticlesForArea( particles, 4f );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitPacket.Broadcast( this.HostType, this.HostWhoAmI, origin, damage, buffType );

				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
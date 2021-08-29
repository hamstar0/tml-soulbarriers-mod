using System;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyPlayerDebuffHit( int buffType, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			var config = SoulBarriersConfig.Instance;
			var hostPlayer = (Player)this.Host;
			int buffIdx = hostPlayer?.FindBuffIndex( buffType ) ?? -1;

			hostPlayer.DelBuff( buffIdx );

			double damage = (double)config.Get<float>( nameof(config.BarrierDebuffRemovalCost) );

			this.SetStrength( this.Strength - damage );

			this.ApplyHitFx( (int)(damage * 4d) );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients( this, false, default, damage, buffType );

				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
using System;
using Microsoft.Xna.Framework;
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

			int damage = config.Get<int>( nameof( config.BarrierDebuffRemovalCost ) );
			this.SetStrength( this.Strength - damage );

			Vector2 origin = this.GetBarrierWorldCenter();

			this.ApplyHitFx( damage * 4 );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients( this, origin, damage, buffType );

				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
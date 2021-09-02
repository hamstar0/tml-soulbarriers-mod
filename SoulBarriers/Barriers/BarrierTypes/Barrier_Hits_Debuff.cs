using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyPlayerDebuffHit( int buffType, bool syncFromServer ) {
			if( this.HostType != BarrierHostType.Player ) {
				throw new ModLibsException( "Incorrect barrier type." );
			}

			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			var config = SoulBarriersConfig.Instance;
			var hostPlayer = (Player)this.Host;
			int buffIdx = hostPlayer?.FindBuffIndex( buffType ) ?? -1;

			hostPlayer.DelBuff( buffIdx );

			double damage = (double)config.Get<float>( nameof(config.BarrierDebuffRemovalCost) );

			this.SetStrength( this.Strength - damage, false, false );

			int particles = (int)( damage * 4d );
			if( particles >= 1 ) {
				this.ApplyHitFx( particles );
			}

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitDebuffPacket.BroadcastToClients(
					barrier: this,
					buffType: buffType
				);

				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
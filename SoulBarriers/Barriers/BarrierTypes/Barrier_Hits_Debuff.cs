using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool ApplyPlayerDebuffHit_If( int buffType, bool syncIfServer ) {
			if( this.HostType != BarrierHostType.Player ) {
				throw new ModLibsException( "Incorrect barrier type." );
			}

			var config = SoulBarriersConfig.Instance;
			var hostPlayer = (Player)this.Host;
			int buffIdx = hostPlayer?.FindBuffIndex( buffType ) ?? -1;
			if( buffIdx == -1 ) {
				return false;
			}

			//

			double damage = (double)config.Get<float>( nameof(config.BarrierStrengthCostToRemoveDebuff) );

			//

			this.ApplyPlayerDebuffRemoveAndBarrierHit( buffType, this.Strength - damage, syncIfServer );

			return true;
		}


		internal void ApplyPlayerDebuffRemoveAndBarrierHit(
					int buffType,
					double newBarrierStrength,
					bool syncIfServer ) {
			var config = SoulBarriersConfig.Instance;
			var hostPlayer = (Player)this.Host;
			int buffIdx = hostPlayer?.FindBuffIndex( buffType ) ?? -1;

			//

			if( buffIdx != -1 ) {
				hostPlayer.DelBuff( buffIdx );
			}

			//

			double damage = (double)config.Get<float>( nameof(config.BarrierStrengthCostToRemoveDebuff) );

			//
			
			if( SoulBarriersConfig.Instance.DebugModeHitInfo ) {
				var hitData = new BarrierHitContext( "PlrDebuff_"+buffType, damage );

				hitData.Output( this );
			}

			//

			this.SetStrength(
				strength: this.Strength - damage,
				clearRegenBuffer: false,
				refreshHostBuffState: false,
				syncsOwnerBuffChanges: syncIfServer && Main.netMode == NetmodeID.Server
			);

			//

			if( Main.netMode != NetmodeID.Server ) {
				if( damage > 0d ) {
					this.ApplyHitFx( 0, 4f, damage, !this.IsActive );
				}
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitDebuffPacket.BroadcastToClients(
					barrier: this,
					buffType: buffType,
					newBarrierStrength: this.Strength
				);

				NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, hostPlayer.whoAmI );
			}
		}
	}
}
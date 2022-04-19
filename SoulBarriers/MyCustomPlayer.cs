using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.PlayerData;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical.Personal;
using SoulBarriers.Packets;


namespace SoulBarriers {
	partial class SoulBarriersPlayerData : CustomPlayerData {
		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( Main.netMode == NetmodeID.Server ) {
				this.SyncBarriers();
			}
		}


		////////////////

		private void SyncBarriers() {
			var mngr = BarrierManager.Instance;

			//

			foreach( Barrier barrier in mngr.GetPlayerBarriers().Values ) {
				if( barrier.HostWhoAmI == this.PlayerWho ) {
					continue;
				}

				//

				BarrierStrengthPacket.SendToClient( -1, barrier, barrier.Strength, false, true );
			}

			//

			foreach( Barrier barrier in mngr.GetNPCBarriers().Values ) {
				NPCBarrierCreatePacket.BroadcastToClients( barrier as PersonalBarrier );
			}
		}
	}
}
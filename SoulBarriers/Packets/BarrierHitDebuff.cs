using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierHitDebuffPacket : SimplePacketPayload {
		public static void BroadcastToClients( Barrier barrier, int buffType, double newBarrierStrength ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitDebuffPacket( barrier, buffType, newBarrierStrength );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int BuffType;

		public double NewBarrierStrength;



		////////////////

		private BarrierHitDebuffPacket() { }

		private BarrierHitDebuffPacket( Barrier barrier, int buffType, double newBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.BuffType = buffType;
			this.NewBarrierStrength = newBarrierStrength;
		}

		////////////////

		private void Receive() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );
				return;
			}

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert(
					"Barrier hit: "+this.BarrierID
					+", BuffType: "+this.BuffType
					+", NewBarrierStrength: "+this.NewBarrierStrength
				);
			}

			//

			barrier.ApplyPlayerDebuffRemoveAndBarrierHit( this.BuffType, this.NewBarrierStrength, false );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive();
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier hits." );
		}
	}
}

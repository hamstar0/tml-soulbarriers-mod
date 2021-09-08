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
		public static void BroadcastToClients( Barrier barrier, int buffType ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitDebuffPacket( barrier, buffType );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int BuffType;



		////////////////

		private BarrierHitDebuffPacket() { }

		private BarrierHitDebuffPacket( Barrier barrier, int buffType ) {
			this.BarrierID = barrier.GetID();
			this.BuffType = buffType;
		}

		////////////////

		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier from "+Main.player[fromWho]+" ("+fromWho+")"
					+" id'd: "+this.BarrierID );
				return;
			}

			barrier.ApplyPlayerDebuffHit( this.BuffType, false );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive( 255 );
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier hits." );
		}
	}
}

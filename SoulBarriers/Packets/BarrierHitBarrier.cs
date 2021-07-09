using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierHitBarrierPacket : SimplePacketPayload {
		public static void BroadcastToClients( Barrier barrier, Barrier otherBarrier ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitBarrierPacket( barrier, otherBarrier );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		private string BarrierID;

		private string OtherBarrierID;



		////////////////

		private BarrierHitBarrierPacket() { }

		private BarrierHitBarrierPacket( Barrier barrier, Barrier otherBarrier ) {
			this.BarrierID = barrier.GetID();
			this.OtherBarrierID = otherBarrier.GetID();
		}

		////////////////

		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier from "+Main.player[fromWho]+" ("+fromWho+") id'd: "+this.BarrierID );
				return;
			}
			Barrier otherBarrier = BarrierManager.Instance.GetBarrierByID( this.OtherBarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such other barrier from "+Main.player[fromWho]+" ("+fromWho+") id'd: "+this.OtherBarrierID );
				return;
			}

			barrier.ApplyCollisionHit( otherBarrier, false );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive( 255 );
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier collision hits." );
		}
	}
}

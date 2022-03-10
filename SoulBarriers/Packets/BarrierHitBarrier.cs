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
		public static void BroadcastToClients(
					Barrier barrier,
					Barrier otherBarrier,
					double prevBarrierStrength,
					double prevIntruderBarrierStrength ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitBarrierPacket(
				barrier,
				otherBarrier,
				prevBarrierStrength,
				prevIntruderBarrierStrength
			);

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public string OtherBarrierID;

		public double BarrierStrength;

		public double OtherBarrierStrength;



		////////////////

		private BarrierHitBarrierPacket() { }

		private BarrierHitBarrierPacket(
					Barrier barrier,
					Barrier otherBarrier,
					double prevBarrierStrength,
					double prevIntruderBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.OtherBarrierID = otherBarrier.ID;

			//this.BarrierStrength = barrier.Strength;
			//this.OtherBarrierStrength = otherBarrier.Strength;
			this.BarrierStrength = prevBarrierStrength;
			this.OtherBarrierStrength = prevIntruderBarrierStrength;
		}
		

		////////////////

		private void Receive() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );
				return;
			}

			Barrier otherBarrier = BarrierManager.Instance.GetBarrierByID( this.OtherBarrierID );
			if( otherBarrier == null ) {
				LogLibraries.Warn( "No such other barrier id'd: "+this.OtherBarrierID );
				return;
			}

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit: "+this.BarrierID+" ("+this.BarrierStrength+")"
					+" vs "+this.OtherBarrierID+" ("+this.OtherBarrierStrength+")"
				);
			}

			barrier.SetStrength( this.BarrierStrength, false, false, false );
			otherBarrier.SetStrength( this.OtherBarrierStrength, false, false, false );

//LogLibraries.Log( "BARRIER V BARRIER - "
//	+ "barrier:" + this.BarrierID + " (" + this.BarrierStrength + ") vs "
//	+ "barrier:" + this.OtherBarrierID + " (" + this.OtherBarrierStrength + ")" );
			barrier.ApplyBarrierCollisionHitIf( otherBarrier, false );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive();
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier collision hits." );
		}
	}
}

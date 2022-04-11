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
					Barrier sourceBarrier,
					Barrier otherBarrier,
					bool defaultCollisionAllowed,
					double damage,
					double oldSourceBarrierStrength,
					double oldOtherBarrierStrength ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitBarrierPacket(
				barrier: sourceBarrier,
				otherBarrier: otherBarrier,
				defaultCollisionAllowed: defaultCollisionAllowed,
				damage: damage,
				oldSourceBarrierStrength: oldSourceBarrierStrength,
				oldOtherBarrierStrength: oldOtherBarrierStrength
			);

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public string OtherBarrierID;

		public bool DefaultCollisionAllowed;

		public double Damage;

		public double OldSourceBarrierStrength;

		public double OldOtherBarrierStrength;



		////////////////

		private BarrierHitBarrierPacket() { }

		private BarrierHitBarrierPacket(
					Barrier barrier,
					Barrier otherBarrier,
					bool defaultCollisionAllowed,
					double damage,
					double oldSourceBarrierStrength,
					double oldOtherBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.OtherBarrierID = otherBarrier.ID;

			this.DefaultCollisionAllowed = defaultCollisionAllowed;
			this.Damage = damage;
			this.OldSourceBarrierStrength = oldSourceBarrierStrength;
			this.OldOtherBarrierStrength = oldOtherBarrierStrength;
		}
		

		////////////////

		private void Receive_If() {
			var barrierMngr = BarrierManager.Instance;

			Barrier barrier = barrierMngr.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );

				return;
			}

			Barrier otherBarrier = barrierMngr.GetBarrierByID( this.OtherBarrierID );
			if( otherBarrier == null ) {
				LogLibraries.Warn( "No such other barrier id'd: "+this.OtherBarrierID );

				return;
			}

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit: "+this.BarrierID+" ("+this.OldSourceBarrierStrength+")"
					+" vs "+this.OtherBarrierID+" ("+this.OldOtherBarrierStrength+")"
				);
			}

			//

			barrier.SetStrength( this.OldSourceBarrierStrength, false, false, false );
			otherBarrier.SetStrength( this.OldOtherBarrierStrength, false, false, false );

			//

//LogLibraries.Log( "BARRIER V BARRIER - "
//	+ "barrier:" + this.BarrierID + " (" + this.BarrierStrength + ") vs "
//	+ "barrier:" + this.OtherBarrierID + " (" + this.OtherBarrierStrength + ")" );
			barrier.ApplyBarrierCollisionHit( otherBarrier, this.DefaultCollisionAllowed, this.Damage, false );
		}


		////

		public override void ReceiveOnClient() {
			this.Receive_If();
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier collision hits." );
		}
	}
}

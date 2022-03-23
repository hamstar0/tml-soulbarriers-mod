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
					double newSourceBarrierStrength,
					double newOtherBarrierStrength ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitBarrierPacket(
				barrier: sourceBarrier,
				otherBarrier: otherBarrier,
				defaultCollisionAllowed: defaultCollisionAllowed,
				newBarrierStrength: newSourceBarrierStrength,
				newIntruderBarrierStrength: newOtherBarrierStrength
			);

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public string OtherBarrierID;

		public bool DefaultCollisionAllowed;

		public double NewBarrierStrength;

		public double NewIntruderBarrierStrength;



		////////////////

		private BarrierHitBarrierPacket() { }

		private BarrierHitBarrierPacket(
					Barrier barrier,
					Barrier otherBarrier,
					bool defaultCollisionAllowed,
					double newBarrierStrength,
					double newIntruderBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.OtherBarrierID = otherBarrier.ID;

			this.DefaultCollisionAllowed = defaultCollisionAllowed;
			this.NewBarrierStrength = newBarrierStrength;
			this.NewIntruderBarrierStrength = newIntruderBarrierStrength;
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
				LogLibraries.Alert( "Barrier hit: "+this.BarrierID+" ("+this.NewBarrierStrength+")"
					+" vs "+this.OtherBarrierID+" ("+this.NewIntruderBarrierStrength+")"
				);
			}

			//

			double prevThisBarrierStr = barrier.Strength;
			double prevThatBarrierStr = otherBarrier.Strength;

			barrier.SetStrength( this.NewBarrierStrength, false, false, false );
			otherBarrier.SetStrength( this.NewIntruderBarrierStrength, false, false, false );

			double thisDamage = prevThisBarrierStr - barrier.Strength;
			double thatDamage = prevThatBarrierStr - otherBarrier.Strength;
			double damage = thisDamage > thatDamage
				? thisDamage
				: thatDamage;

			//

//LogLibraries.Log( "BARRIER V BARRIER - "
//	+ "barrier:" + this.BarrierID + " (" + this.BarrierStrength + ") vs "
//	+ "barrier:" + this.OtherBarrierID + " (" + this.OtherBarrierStrength + ")" );
			barrier.ApplyBarrierCollisionHit_If( otherBarrier, this.DefaultCollisionAllowed, damage, false );
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

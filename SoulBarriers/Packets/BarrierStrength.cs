using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierStrengthPacket : SimplePacketPayload {
		public static void SyncFromClientToServer( Barrier barrier, int strength, bool applyHitFx ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not client." );
			}

			var packet = new BarrierStrengthPacket( barrier.GetID(), strength, applyHitFx );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		private string BarrierID;

		private int Strength;

		private bool ApplyHitFx;



		////////////////

		public BarrierStrengthPacket( string barrierID, int strength, bool applyHitFx ) {
			this.BarrierID = barrierID;
			this.Strength = strength;
			this.ApplyHitFx = applyHitFx;
		}

		////////////////
		
		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier from "+Main.player[fromWho]+" ("+fromWho+") id'd: "+this.BarrierID );
				return;
			}

			int damage = barrier.Strength - this.Strength;

			if( this.ApplyHitFx ) {
				barrier.ApplyHitFx( damage >= 1 ? damage : 8 );
			}

			barrier.SetStrength( this.Strength );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive( 255 );
		}

		public override void ReceiveOnServer( int fromWho ) {
			this.Receive( fromWho );

			SimplePacket.SendToClient( this, -1, fromWho );
		}
	}
}

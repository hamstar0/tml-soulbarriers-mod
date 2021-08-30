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
		public static void SyncToServerForEveryone(
					Barrier barrier,
					int strength,
					bool applyHitFx,
					bool clearRegenBuffer ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not client." );
			}

			var packet = new BarrierStrengthPacket( barrier, strength, applyHitFx, clearRegenBuffer );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public string BarrierID;

		public int Strength;

		public bool ApplyHitFx;

		public bool ClearRegenBuffer;



		////////////////

		private BarrierStrengthPacket() { }

		private BarrierStrengthPacket( Barrier barrier, int strength, bool applyHitFx, bool clearRegenBuffer ) {
			this.BarrierID = barrier.GetID();
			this.Strength = strength;
			this.ApplyHitFx = applyHitFx;
			this.ClearRegenBuffer = clearRegenBuffer;
		}

		////////////////

		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier from "+Main.player[fromWho]+" ("+fromWho+") id'd: "+this.BarrierID );
				return;
			}

			double damage = barrier.Strength - this.Strength;

			if( this.ApplyHitFx ) {
				barrier.ApplyHitFx( damage > 0d ? damage : 8d );
			}

			barrier.SetStrength( this.Strength, this.ClearRegenBuffer );
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

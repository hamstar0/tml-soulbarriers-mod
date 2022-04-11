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
		public static void BroadcastToClients(
					Barrier barrier,
					int buffType,
					double damage,
					double oldBarrierStrength ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitDebuffPacket( barrier, buffType, damage, oldBarrierStrength );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int BuffType;

		public double Damage;

		public double OldBarrierStrength;



		////////////////

		private BarrierHitDebuffPacket() { }

		private BarrierHitDebuffPacket(
					Barrier barrier,
					int buffType,
					double damage,
					double oldBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.BuffType = buffType;
			this.Damage = damage;
			this.OldBarrierStrength = oldBarrierStrength;
		}

		////////////////

		private void Receive_If() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );

				return;
			}

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert(
					"Barrier hit: "+this.BarrierID
					+", BuffType: "+this.BuffType
					+", Damage: "+this.Damage
					+", OldBarrierStrength: "+this.OldBarrierStrength
				);
			}

			//

			barrier.SetStrength( this.OldBarrierStrength, false, false, false );

			//

			barrier.ApplyPlayerDebuffRemoveAndBarrierHit( this.BuffType, this.Damage, false );
		}

		////

		public override void ReceiveOnClient() {
			this.Receive_If();
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server doesn't sync barrier hits." );
		}
	}
}

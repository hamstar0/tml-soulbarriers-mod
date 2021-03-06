using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierHitMetaphysicalPacket : SimplePacketPayload {
		public static void BroadcastToClients(
					Barrier barrier,
					bool hasHitPosition,
					Vector2 hitPosition,
					double damage ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitMetaphysicalPacket( barrier, hasHitPosition, hitPosition, damage );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public bool HasHitPosition;

		public Vector2 HitPosition;

		public double Damage;



		////////////////

		private BarrierHitMetaphysicalPacket() { }

		private BarrierHitMetaphysicalPacket(
					Barrier barrier,
					bool hasHitPosition,
					Vector2 hitPosition,
					double damage ) {
			this.BarrierID = barrier.ID;
			this.HasHitPosition = hasHitPosition;
			this.HitPosition = hitPosition;
			this.Damage = damage;
		}

		////////////////

		private void Receive() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );

				return;
			}

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit: "+this.BarrierID
					+", pos: "+(this.HasHitPosition ? this.HitPosition.ToString() : "none")
					+", dmg: "+this.Damage
				);
			}

			//

			barrier.ApplyMetaphysicalHit(
				hitAt: this.HasHitPosition ? this.HitPosition : (Vector2?)null,
				damage: this.Damage,
				syncIfServer: false
			);
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

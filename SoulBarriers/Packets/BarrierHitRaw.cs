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
	class BarrierHitRawPacket : SimplePacketPayload {
		public static void BroadcastToClients(
					Barrier barrier,
					bool hasHitPosition,
					Vector2 hitPosition,
					double damage,
					BarrierHitContext context ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitRawPacket( barrier, hasHitPosition,  hitPosition, damage, context );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public bool HasHitPosition;

		public Vector2 HitPosition;

		public double Damage;

		public string AbridgedHitContext;



		////////////////

		private BarrierHitRawPacket() { }

		private BarrierHitRawPacket(
					Barrier barrier,
					bool hasHitPosition,
					Vector2 hitPosition,
					double damage,
					BarrierHitContext context ) {
			this.BarrierID = barrier.ID;
			this.HasHitPosition = hasHitPosition;
			this.HitPosition = hitPosition;
			this.Damage = damage;

			this.AbridgedHitContext = SoulBarriersConfig.Instance.DebugModeHitInfo
				? context?.SourceToString() ?? ""
				: "";
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

			if( this.Damage > 0d ) {
				barrier.ApplyRawHit(
					this.HasHitPosition ? this.HitPosition : (Vector2?)null,
					this.Damage,
					false,
					this.AbridgedHitContext != null
						? new BarrierHitContext("NET_"+this.AbridgedHitContext, this.Damage)
						: new BarrierHitContext("NET_UKN", this.Damage)
				);
			}
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

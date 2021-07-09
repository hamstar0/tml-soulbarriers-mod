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
					Vector2 hitPosition,
					int damage,
					int buffType = -1 ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierHitRawPacket( barrier, hitPosition, damage, buffType );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		private string BarrierID;

		private Vector2 HitPosition;

		private int Damage;

		private int BuffType;



		////////////////

		private BarrierHitRawPacket() { }

		private BarrierHitRawPacket( Barrier barrier, Vector2 hitPosition, int damage, int buffType ) {
			this.BarrierID = barrier.GetID();
			this.HitPosition = hitPosition;
			this.Damage = damage;
			this.BuffType = buffType;
		}

		////////////////

		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier from "+Main.player[fromWho]+" ("+fromWho+") id'd: "+this.BarrierID );
				return;
			}

			if( this.Damage >= 1 ) {
				barrier.ApplyRawHit( this.HitPosition, this.Damage, false );
			}

			if( this.BuffType >= 1 ) {
				barrier.ApplyPlayerDebuffHit( this.BuffType, false );
			}
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

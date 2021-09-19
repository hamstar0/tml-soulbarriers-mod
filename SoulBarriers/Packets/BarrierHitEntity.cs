using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierHitEntityPacket : SimplePacketPayload {
		public static void BroadcastToClients( Barrier barrier, BarrierIntruderType entityType, int entityWhoAmI ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			if( entityType == BarrierIntruderType.Barrier ) {
				throw new ModLibsException( "Use BarrierCollideBarrierHitPacket instead." );
			}

			var packet = new BarrierHitEntityPacket( barrier, entityType, entityWhoAmI );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int EntityType;

		public int EntityWhoAmI;



		////////////////

		private BarrierHitEntityPacket() { }

		private BarrierHitEntityPacket( Barrier barrier, BarrierIntruderType entityType, int entityWhoAmI ) {
			this.BarrierID = barrier.GetID();
			this.EntityType = (int)entityType;
			this.EntityWhoAmI = entityWhoAmI;
		}

		////////////////

		private void Receive() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );
				return;
			}

			var entType = (BarrierIntruderType)this.EntityType;
			Entity entity = null;

			switch( entType ) {
			case BarrierIntruderType.Player:
				entity = Main.player[ this.EntityWhoAmI ];
				break;
			case BarrierIntruderType.NPC:
				entity = Main.npc[ this.EntityWhoAmI ];
				break;
			case BarrierIntruderType.Projectile:
				entity = Main.projectile[ this.EntityWhoAmI ];
				break;
			}

			if( entity == null ) {
				LogLibraries.Warn( "Could not identify intruder entity "+entType+" "+this.EntityWhoAmI );
				return;
			}

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit: "+this.BarrierID+", entity: "+entity );
			}

			barrier.ApplyEntityCollisionHitIf( entity, false );
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

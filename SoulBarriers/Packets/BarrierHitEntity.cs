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
		public static void BroadcastToClients(
					Barrier barrier,
					BarrierIntruderType entityType,
					int entityIdentity,
					bool defaultCollisionAllowed ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			if( entityType == BarrierIntruderType.Barrier ) {
				throw new ModLibsException( "Use BarrierCollideBarrierHitPacket instead." );
			}

			var packet = new BarrierHitEntityPacket(
				barrier: barrier,
				entityType: entityType,
				entityIdentity: entityIdentity,
				defaultCollisionAllowed: defaultCollisionAllowed
			);

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int EntityType;

		public int EntityIdentity;

		public bool DefaultCollisionAllowed;



		////////////////

		private BarrierHitEntityPacket() { }

		private BarrierHitEntityPacket(
					Barrier barrier,
					BarrierIntruderType entityType,
					int entityIdentity,
					bool defaultCollisionAllowed ) {
			this.BarrierID = barrier.ID;
			this.EntityType = (int)entityType;
			this.EntityIdentity = entityIdentity;
			this.DefaultCollisionAllowed = defaultCollisionAllowed;
		}

		////////////////

		private void Receive() {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( "No such barrier id'd: "+this.BarrierID );

				return;
			}

			//

			var entType = (BarrierIntruderType)this.EntityType;
			Entity entity = null;

			switch( entType ) {
			case BarrierIntruderType.Player:
				entity = Main.player[ this.EntityIdentity ];
				break;
			case BarrierIntruderType.NPC:
				entity = Main.npc[ this.EntityIdentity ];
				break;
			case BarrierIntruderType.Projectile:
				entity = Main.projectile[ this.EntityIdentity ];
				break;
			}

			if( entity == null ) {
				LogLibraries.Warn( "Could not identify intruder entity "+entType+" "+this.EntityIdentity );

				return;
			}

			//

			bool hasHit = barrier.ApplyEntityCollisionHit_If(
				intruder: entity,
				defaultCollisionAllowed: this.DefaultCollisionAllowed,
				syncIfServer: false
			);

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit? "+hasHit+", "+this.BarrierID+" vs ent "+entity );
			}
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

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
					bool defaultCollisionAllowed,
					//double damage,
					double oldBarrierStrength ) {
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
				defaultCollisionAllowed: defaultCollisionAllowed,
				//damage: damage
				oldBarrierStrength: oldBarrierStrength
			);

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;

		public int EntityType;

		public int EntityIdentity;

		public bool DefaultCollisionAllowed;

		//public double Damage;

		public double OldBarrierStrength;



		////////////////

		private BarrierHitEntityPacket() { }

		private BarrierHitEntityPacket(
					Barrier barrier,
					BarrierIntruderType entityType,
					int entityIdentity,
					bool defaultCollisionAllowed,
					//double damage,
					double oldBarrierStrength ) {
			this.BarrierID = barrier.ID;
			this.EntityType = (int)entityType;
			this.EntityIdentity = entityIdentity;
			this.DefaultCollisionAllowed = defaultCollisionAllowed;
			//this.Damage = damage;
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

			barrier.SetStrength( this.OldBarrierStrength, false, false, false );

			//

			bool hasHit = barrier.ApplyEntityCollisionHit_If_Syncs(
				intruderEnt: entity,
				defaultCollisionAllowedIf: this.DefaultCollisionAllowed,
				syncIfServer: false
			);

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier hit? "+hasHit+", "+this.BarrierID+" vs ent "+entity );
			}
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

using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool ApplyEntityCollisionHit_If(
					Entity intruder,
					bool? defaultCollisionAllowed,
					bool syncIfServer ) {
			double damage = 0d;
			bool collisionAllowed = !defaultCollisionAllowed.HasValue
				|| (defaultCollisionAllowed.HasValue && defaultCollisionAllowed.Value);

			//

			if( intruder.active ) {
				if( intruder is Projectile ) {
					collisionAllowed = this.ApplyProjectileCollisionHit_If(
						intruderProjectile: (Projectile)intruder,
						defaultCollisionAllowed: collisionAllowed,
						syncIfServer: syncIfServer,
						damage: out damage
					);
				} else if( intruder is Player ) {
					collisionAllowed = this.ApplyPlayerCollisionHit_If(
						intruderPlayer: (Player)intruder,
						defaultCollisionAllowed: collisionAllowed,
						syncIfServer: syncIfServer,
						damage: out damage
					);
				} else if( intruder is NPC ) {
					collisionAllowed = this.ApplyNpcCollisionHit_If(
						intruderNpc: (NPC)intruder,
						defaultCollisionAllowed: collisionAllowed,
						syncIfServer: syncIfServer,
						damage: out damage
					);
				}
			}

			//

			foreach( PostBarrierEntityCollisionHook e in this.OnPostBarrierEntityCollision ) {
				e.Invoke( intruder, collisionAllowed, damage );
			}

			return collisionAllowed;
		}


		////////////////

		private bool ApplyProjectileCollisionHit_If(
					Projectile intruderProjectile,
					bool defaultCollisionAllowed,
					bool syncIfServer,
					out double damage ) {
			if( !intruderProjectile.active || intruderProjectile.damage == 0 ) {
				damage = 0d;
				return false;
			}

			//

			Entity myIntruder = intruderProjectile;
			double myDamage = intruderProjectile.damage;
			
			bool isDefaultCollision = this.OnPreBarrierEntityCollision
				.All( f=>f.Invoke(ref myIntruder, ref myDamage) );

			intruderProjectile = myIntruder as Projectile;
			damage = myDamage;

			//

			if( defaultCollisionAllowed && isDefaultCollision ) {
				BarrierHitContext hitData = new BarrierHitContext( intruderProjectile, damage );

				this.ApplyRawHit( intruderProjectile.Center, damage, false, hitData );	// TODO: Confirm sync

				//

				intruderProjectile.Kill();
				intruderProjectile.active = false;
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients(
					barrier: this,
					entityType: BarrierIntruderType.Projectile,
					entityIdentity: intruderProjectile.identity,
					defaultCollisionAllowed: defaultCollisionAllowed && isDefaultCollision
				);
			}
			//if( Main.netMode == NetmodeID.MultiplayerClient || (Main.netMode == NetmodeID.Server && syncIfServer) ) {
			//	NetMessage.SendData( MessageID.SyncProjectile, -1, -1, null, intruderProjectile.identity );
			//}

			return isDefaultCollision;
		}


		////////////////

		private bool ApplyPlayerCollisionHit_If(
					Player intruderPlayer,
					bool defaultCollisionAllowed,
					bool syncIfServer,
					out double damage ) {
			Entity myIntruder = intruderPlayer;
			double myDamage = 0d;

			bool isCollisionAllowed = this.OnPreBarrierEntityCollision
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isCollisionAllowed ) {
				// No effect, by default; (Pre)BarrierEntityCollision hook(s) must implement behavior
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients(
					barrier: this,
					entityType: BarrierIntruderType.Player,
					entityIdentity: intruderPlayer.whoAmI,
					defaultCollisionAllowed: defaultCollisionAllowed && isCollisionAllowed
				);
			}

			return isCollisionAllowed;
		}


		////////////////

		private bool ApplyNpcCollisionHit_If(
					NPC intruderNpc,
					bool defaultCollisionAllowed,
					bool syncIfServer,
					out double damage ) {
			// Is npc a "projectile"?
			if( NPCID.Sets.ProjectileNPC[intruderNpc.type] ) {
				return this.ApplyProjectileNpcCollisionHit_If(
					intruderNpc: intruderNpc,
					defaultCollisionAllowed: defaultCollisionAllowed,
					syncIfServer: syncIfServer,
					damage: out damage
				);
			} else {
				return this.ApplyNonProjectileNpcCollisionHit_If(
					intruderNpc: intruderNpc,
					defaultCollisionAllowed: defaultCollisionAllowed,
					syncIfServer: syncIfServer,
					damage: out damage
				);
			}
		}


		////

		private bool ApplyProjectileNpcCollisionHit_If(
					NPC intruderNpc,
					bool defaultCollisionAllowed,
					bool syncIfServer,
					out double damage ) {
			Entity myIntruder = intruderNpc;
			double myDamage = intruderNpc.damage;

			bool isDefaultCollision = this.OnPreBarrierEntityCollision
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isDefaultCollision ) {
				var hitData = new BarrierHitContext( intruderNpc, damage );

				this.ApplyRawHit( intruderNpc.Center, damage, syncIfServer, hitData );

				//

				NPCLibraries.Kill( intruderNpc, syncIfServer && Main.netMode == NetmodeID.Server );	// Redundant?

				//

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, intruderNpc.whoAmI );	// Redundant?
				}
			}

			//

			Main.PlaySound( SoundID.NPCHit15, intruderNpc.Center );

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients(
					barrier: this,
					entityType: BarrierIntruderType.NPC,
					entityIdentity: intruderNpc.whoAmI,
					defaultCollisionAllowed: defaultCollisionAllowed && isDefaultCollision
				);
			}

			//

			return isDefaultCollision;
		}


		private bool ApplyNonProjectileNpcCollisionHit_If(
					NPC intruderNpc,
					bool defaultCollisionAllowed,
					bool syncIfServer,
					out double damage ) {
			Entity myIntruder = intruderNpc;
			double myDamage = 0d;

			bool isDefaultCollision = this.OnPreBarrierEntityCollision
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isDefaultCollision ) {
				// Non-"projectile" NPC? No effect, by default
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients(
					barrier: this,
					entityType: BarrierIntruderType.NPC,
					entityIdentity: intruderNpc.whoAmI,
					defaultCollisionAllowed: defaultCollisionAllowed && isDefaultCollision
				);
			}

			return isDefaultCollision;
		}
	}
}
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool ApplyEntityCollisionHit_Syncs(
					Entity intruderEnt,
					bool? defaultCollisionAllowedIf,
					bool syncIfServer ) {
			double damage = 0d;
			bool defaultCollisionAllowed = !defaultCollisionAllowedIf.HasValue
				|| (defaultCollisionAllowedIf.HasValue && defaultCollisionAllowedIf.Value);

			bool isDefaultCollision = false;

			int entId = -1;
			BarrierIntruderType entType = default;

			//

			if( intruderEnt.active ) {
				if( intruderEnt is Projectile ) {
					isDefaultCollision = this.ApplyProjectileCollisionHit_If(
						intruderProjectile: (Projectile)intruderEnt,
						defaultCollisionAllowed: defaultCollisionAllowed,
						damage: out damage
					);

					entId = ((Projectile)intruderEnt).identity;
					entType = BarrierIntruderType.Projectile;
				} else if( intruderEnt is Player ) {
					isDefaultCollision = this.ApplyPlayerCollisionHit_If(
						intruderPlayer: (Player)intruderEnt,
						defaultCollisionAllowed: defaultCollisionAllowed,
						damage: out damage
					);

					entId = ((Player)intruderEnt).whoAmI;
					entType = BarrierIntruderType.Player;
				} else if( intruderEnt is NPC ) {
					isDefaultCollision = this.ApplyNpcCollisionHit_If(
						intruderNpc: (NPC)intruderEnt,
						defaultCollisionAllowed: defaultCollisionAllowed,
						syncIfServer: syncIfServer,
						damage: out damage
					);

					entId = ((NPC)intruderEnt).whoAmI;
					entType = BarrierIntruderType.NPC;
				}
			}

			bool isDefaultCollisionHappening = defaultCollisionAllowed && isDefaultCollision;

			//

			foreach( PostBarrierEntityHitHook e in this.OnPostBarrierEntityHit ) {
				e.Invoke( intruderEnt, isDefaultCollisionHappening, damage );
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients(
					barrier: this,
					entityType: entType,
					entityIdentity: entId,
					defaultCollisionAllowed: isDefaultCollisionHappening
				);
			}

			return isDefaultCollision;
		}


		////////////////

		private bool ApplyProjectileCollisionHit_If(
					Projectile intruderProjectile,
					bool defaultCollisionAllowed,
					out double damage ) {
			if( !intruderProjectile.active || intruderProjectile.damage == 0 ) {
				damage = 0d;
				return false;
			}

			//

			Entity myIntruder = intruderProjectile;
			double myDamage = intruderProjectile.damage;
			
			bool isDefaultCollision = this.OnPreBarrierEntityHit
				.All( f=>f.Invoke(ref myIntruder, ref myDamage) );

			//

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

			return isDefaultCollision;
		}


		////////////////

		private bool ApplyPlayerCollisionHit_If(
					Player intruderPlayer,
					bool defaultCollisionAllowed,
					out double damage ) {
			Entity myIntruder = intruderPlayer;
			double myDamage = 0d;

			bool isCollisionAllowed = this.OnPreBarrierEntityHit
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isCollisionAllowed ) {
				// No effect, by default; (Pre)BarrierEntityCollision hook(s) must implement behavior
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

			bool isDefaultCollision = this.OnPreBarrierEntityHit
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isDefaultCollision ) {
				var hitData = new BarrierHitContext( intruderNpc, damage );

				this.ApplyRawHit( intruderNpc.Center, damage, syncIfServer, hitData );

				//

				// Kill "projectile" NPC just like regular projectiles
				NPCLibraries.Kill( intruderNpc, syncIfServer && Main.netMode == NetmodeID.Server );

				//

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, intruderNpc.whoAmI );	// Redundant?
				}
			}

			//

			Main.PlaySound( SoundID.NPCHit15, intruderNpc.Center );

			//

			return isDefaultCollision;
		}


		private bool ApplyNonProjectileNpcCollisionHit_If(
					NPC intruderNpc,
					bool defaultCollisionAllowed,
					out double damage ) {
			Entity myIntruder = intruderNpc;
			double myDamage = 0d;

			bool isDefaultCollision = this.OnPreBarrierEntityHit
				.All( f => f.Invoke( ref myIntruder, ref myDamage ) );

			damage = myDamage;

			//

			if( defaultCollisionAllowed && isDefaultCollision ) {
				// Non-"projectile" NPC? No effect, by default
			}

			//

			return isDefaultCollision;
		}
	}
}
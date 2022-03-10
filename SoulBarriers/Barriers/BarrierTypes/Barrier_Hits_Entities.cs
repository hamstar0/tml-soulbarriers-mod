using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyEntityCollisionHitIf( Entity intruder, bool syncIfServer ) {
			if( !this.OnPreBarrierEntityCollision.All( f=>f.Invoke(ref intruder) ) ) {
				return;
			}

			//

			foreach( BarrierEntityCollisionHook e in this.OnBarrierEntityCollision ) {
				e.Invoke(intruder);
			}

			if( intruder.active ) {
				if( intruder is Projectile ) {
					this.ApplyProjectileCollisionHit( (Projectile)intruder, syncIfServer );
				} else if( intruder is Player ) {
					this.ApplyPlayerCollisionHit( (Player)intruder, syncIfServer );
				} else if( intruder is NPC ) {
					this.ApplyNpcCollisionHit( (NPC)intruder, syncIfServer );
				}
			}
		}


		////////////////

		private void ApplyProjectileCollisionHit( Projectile intruderProjectile, bool syncIfServer ) {
			if( !intruderProjectile.active || intruderProjectile.damage == 0 ) {
				return;
			}

			//

			BarrierHitContext hitData = new BarrierHitContext( intruderProjectile, intruderProjectile.damage );

			//

			this.ApplyRawHit( intruderProjectile.Center, intruderProjectile.damage, syncIfServer, hitData );

			//

			intruderProjectile.Kill();
		}

		private void ApplyPlayerCollisionHit( Player intruderPlayer, bool syncIfServer ) {
			// No effect, by default; (Pre)BarrierEntityCollision hook must implement behavior

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.Player, intruderPlayer.whoAmI );
			}
		}

		private void ApplyNpcCollisionHit( NPC intruderNpc, bool syncIfServer ) {
			// Is npc a "projectile"?
			if( NPCID.Sets.ProjectileNPC[intruderNpc.type] ) {
				var hitData = new BarrierHitContext( intruderNpc, intruderNpc.damage );

				//

				this.ApplyRawHit( intruderNpc.Center, intruderNpc.damage, true, hitData );

				NPCLibraries.Kill( intruderNpc, Main.netMode != NetmodeID.MultiplayerClient );

				Main.PlaySound( SoundID.NPCHit15, intruderNpc.Center );

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, intruderNpc.whoAmI );
				}
			} else {
				// Non-"projectile" NPC? No effect, by default

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.NPC, intruderNpc.whoAmI );
				}
			}
		}
	}
}
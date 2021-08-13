using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyEntityCollisionHitIf( Entity intruder, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( this.OnPreBarrierEntityCollision.All( f=>f.Invoke(ref intruder) ) ) {
				foreach( BarrierEntityCollisionEvent e in this.OnBarrierEntityCollision ) {
					e.Invoke(intruder);
				}

				if( intruder.active ) {
					if( intruder is Projectile ) {
						this.ApplyProjectileCollisionHit( (Projectile)intruder, syncFromServer );
					} else if( intruder is Player ) {
						this.ApplyPlayerCollisionHit( (Player)intruder, syncFromServer );
					} else if( intruder is NPC ) {
						this.ApplyNpcCollisionHit( (NPC)intruder, syncFromServer );
					}
				}
			}
		}

		public void ApplyBarrierCollisionHitIf( Barrier intruder, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( this.OnPreBarrierBarrierCollision.All( f=>f.Invoke(intruder) ) ) {
				foreach( BarrierBarrierCollisionEvent e in this.OnBarrierBarrierCollision ) {
					e.Invoke(intruder);
				}

				this.ApplyBarrierCollisionHit( intruder, syncFromServer );
			}
		}


		////////////////

		private void ApplyProjectileCollisionHit( Projectile intruderProjectile, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( intruderProjectile.active && intruderProjectile.damage >= 1 ) {
				this.ApplyHitAgainstSelf( intruderProjectile.Center, intruderProjectile.damage, syncFromServer );

				intruderProjectile.Kill();
			}
		}

		private void ApplyPlayerCollisionHit( Player intruderPlayer, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			// No effect, by default

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.Player, intruderPlayer.whoAmI );
			}
		}

		private void ApplyNpcCollisionHit( NPC intruderNpc, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			// Is npc a "projectile"?
			if( NPCID.Sets.ProjectileNPC[intruderNpc.type] ) {
				this.ApplyHitAgainstSelf( intruderNpc.Center, intruderNpc.damage, true );

				NPCLibraries.Kill( intruderNpc, Main.netMode != NetmodeID.MultiplayerClient );

				Main.PlaySound( SoundID.NPCHit15, intruderNpc.Center );

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, intruderNpc.whoAmI );
				}
			} else {
				// Non-"projectile" NPC? No effect, by default

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.NPC, intruderNpc.whoAmI );
				}
			}
		}

		////

		private void ApplyBarrierCollisionHit( Barrier intruder, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}
			
			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitBarrierPacket.BroadcastToClients( this, intruder );
			}
		}
	}
}
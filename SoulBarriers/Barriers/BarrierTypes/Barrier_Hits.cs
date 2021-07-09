using System;
using Terraria;
using Terraria.ID;
using ModLibsGeneral.Libraries.NPCs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyCollisionHit( Entity intruder, bool syncFromServer ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent(this, ref intruder) ) {
					this.ApplyEntityCollisionHit( (Projectile)intruder, syncFromServer );
				}
			} else if( intruder is Player ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent(this, ref intruder) ) {
					this.ApplyEntityCollisionHit( (Player)intruder, syncFromServer );
				}
			} else if( intruder is NPC ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent(this, ref intruder) ) {
					this.ApplyEntityCollisionHit( (NPC)intruder, syncFromServer );
				}
			}
		}

		public void ApplyCollisionHit( Barrier intruder, bool syncFromServer ) {
			if( BarrierManager.Instance.OnPreBarrierBarrierCollisionEvent(this, intruder) ) {
				this.ApplyBarrierCollisionHit( intruder, syncFromServer );
			}
		}


		////////////////

		public void ApplyEntityCollisionHit( Projectile intruderProjectile, bool syncFromServer ) {
			this.ApplyRawHit( intruderProjectile.Center, intruderProjectile.damage, syncFromServer );

			intruderProjectile.Kill();
		}

		public void ApplyEntityCollisionHit( Player intruderPlayer, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			BarrierManager.Instance.OnBarrierEntityCollisionEvent( this, intruderPlayer );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.Player, intruderPlayer.whoAmI );
			}
		}

		public void ApplyEntityCollisionHit( NPC intruderNpc, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( NPCID.Sets.ProjectileNPC[intruderNpc.type] ) {
				this.ApplyRawHit( intruderNpc.Center, intruderNpc.damage, true );

				NPCLibraries.Kill( intruderNpc, Main.netMode != NetmodeID.MultiplayerClient );

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, intruderNpc.whoAmI );
				}
			} else {
				BarrierManager.Instance.OnBarrierEntityCollisionEvent( this, intruderNpc );

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					BarrierHitEntityPacket.BroadcastToClients( this, BarrierIntruderType.NPC, intruderNpc.whoAmI );
				}
			}
		}

		////

		public void ApplyBarrierCollisionHit( Barrier intruder, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			BarrierManager.Instance.OnBarrierBarrierCollisionEvent( this, intruder );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitBarrierPacket.BroadcastToClients( this, intruder );
			}
		}
	}
}
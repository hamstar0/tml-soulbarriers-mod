using System;
using Terraria;
using Terraria.ID;
using ModLibsGeneral.Libraries.NPCs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyCollisionHit( Entity intruder ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent( this, ref intruder) ) {
					this.ApplyEntityCollisionHit( (Projectile)intruder );
				}
			} else if( intruder is Player ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent( this, ref intruder ) ) {
					this.ApplyEntityCollisionHit( (Player)intruder );
				}
			} else if( intruder is NPC ) {
				if( BarrierManager.Instance.OnPreBarrierEntityCollisionEvent( this, ref intruder ) ) {
					this.ApplyEntityCollisionHit( (NPC)intruder );
				}
			}
		}

		public void ApplyCollisionHit( Barrier intruder ) {
			if( BarrierManager.Instance.OnPreBarrierBarrierCollisionEvent( this, intruder) ) {
				this.ApplyBarrierCollisionHit( intruder );
			}
		}


		////////////////

		public void ApplyEntityCollisionHit( Projectile intruderProjectile ) {
			this.ApplyRawHit( intruderProjectile.Center, intruderProjectile.damage, true );

			intruderProjectile.Kill();
		}

		public void ApplyEntityCollisionHit( Player intruderPlayer ) {
			BarrierManager.Instance.OnBarrierEntityCollisionEvent( this, intruderPlayer );
		}

		public void ApplyEntityCollisionHit( NPC intruderNpc ) {
			if( NPCID.Sets.ProjectileNPC[intruderNpc.type] ) {
				this.ApplyRawHit( intruderNpc.Center, intruderNpc.damage, true );

				NPCLibraries.Kill( intruderNpc, Main.netMode != NetmodeID.MultiplayerClient );
			} else {
				BarrierManager.Instance.OnBarrierEntityCollisionEvent( this, intruderNpc );
			}
		}

		////

		public void ApplyBarrierCollisionHit( Barrier intruder ) {
			BarrierManager.Instance.OnBarrierBarrierCollisionEvent( this, intruder );
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyCollisionHit( Entity intruder ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnEntityBarrierCollisionEvent(this, ref intruder) ) {
					this.ApplyProjectileCollisionHit( (Projectile)intruder );
				}
			}
		}


		////////////////

		public void ApplyProjectileCollisionHit( Projectile proj ) {
			this.ApplyRawHit( proj.Center, proj.damage, true );

			proj.Kill();
		}


		////////////////
		
		public void ApplyRawHit( Vector2 hitAt, int damage, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( !BarrierManager.Instance.OnBarrierRawHitEvent(this, ref damage) ) {
				return;
			}
			
			if( damage >= 1 && this.Strength >= 1 ) {
				this.Strength = 0;
			} else {
				this.Strength -= damage;

				// Saved from total destruction
				if( this.Strength <= 0 ) {
					this.Strength = 1;
				}
			}

			int particles = Barrier.GetHitParticleCount( damage );

			this.CreateHitParticlesAt( hitAt, particles, 4f );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitPacket.Broadcast( this.HostType, this.HostWhoAmI, hitAt, damage, -1 );
			}
		}
	}
}
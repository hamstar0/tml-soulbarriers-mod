using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyCollisionHit( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnEntityBarrierCollisionEvent(this, host, ref intruder) ) {
					this.ApplyProjectileCollisionHit( Barrier.GetEntityBarrierOrigin(host), (Projectile)intruder );
				}
			}
		}


		////////////////

		public void ApplyProjectileCollisionHit( Vector2 barrierCenter, Projectile proj ) {
			this.ApplyRawHit( proj.Center, proj.damage );

			int dispersal = proj.width < proj.height
				? proj.width
				: proj.height;

			proj.Kill();

			this.CreateHitParticlesAt( barrierCenter, proj.Center, dispersal );
		}


		////////////////
		
		public void ApplyRawHit( Vector2 basePosition, int damage, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( !BarrierManager.Instance.OnBarrierRawHitEvent(this, ref damage) ) {
				return;
			}
			
			if( damage >= 1 && this.Strength == 1 ) {
				this.Strength = 0;
			} else {
				this.Strength -= damage;

				// Saved from total destruction
				if( this.Strength <= 0 ) {
					this.Strength = 1;
				}
			}

			int particles = Barrier.GetHitParticleCount( damage );

			this.CreateHitParticlesForArea( basePosition, particles );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitPacket.Broadcast( hostPlayer, dmg, buffType );
			}
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyCollisionHit( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnEntityBarrierCollisionEvent(this, host, ref intruder) ) {
					this.ApplyProjectileCollisionHit( (Projectile)intruder );
				}
			}
		}


		////////////////

		public void ApplyProjectileCollisionHit( Projectile proj ) {
			this.ApplyRawHit( proj.Center, proj.damage );

			proj.Kill();

			int dispersal = proj.width < proj.height
				? proj.width
				: proj.height;
		}


		////////////////
		
		public void ApplyRawHit( Vector2 basePosition, int damage ) {
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

			this.CreateHitParticlesForArea( basePosition, damage );
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class Barrier {
		public void ApplyCollisionHit( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				if( BarrierManager.Instance.OnEntityBarrierCollisionEvent(this, host, intruder) ) {
					this.ApplyProjectileCollisionHit( (Projectile)intruder );
				}
			}
		}


		////////////////

		public void ApplyProjectileCollisionHit( Projectile proj ) {
			this.ApplyRawHit( proj.damage );

			proj.Kill();

			int dispersal = proj.width < proj.height
				? proj.width
				: proj.height;

			this.ApplyHitFx( proj.Center, dispersal, proj.damage );
		}


		////////////////
		
		public void ApplyRawHit( int damage ) {
			if( !BarrierManager.Instance.OnEntityBarrierRawHit(ref damage) ) {
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
		}


		////////////////

		private void ApplyDebuffHit( Player hostPlayer, int buffIdx ) {
			var config = SoulBarriersConfig.Instance;

			hostPlayer.DelBuff( buffIdx );

			int dmg = config.Get<int>( nameof( config.BarrierDebuffRemovalCost ) );
			this.SetStrength( hostPlayer, this.Strength - dmg );

			Vector2 origin = this.GetEntityBarrierOrigin( hostPlayer );
			this.ApplyHitFx( origin, (int)this.Radius, dmg * 4 );
		}
	}
}
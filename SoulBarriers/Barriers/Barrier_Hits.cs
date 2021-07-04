using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class Barrier {
		public void ApplyCollisionHit( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				this.ApplyProjectileCollisionHit( (Projectile)intruder );
			}
		}


		////////////////

		public void ApplyProjectileCollisionHit( Projectile proj ) {
			if( proj.damage >= 1 && this.Strength == 1 ) {
				this.Strength = 0;

				proj.Kill();
			} else {
				this.Strength -= proj.damage;

				if( this.Strength <= 0 ) {
					this.Strength = 1;
				}

				proj.Kill();
			}

			int dispersal = proj.width < proj.height
				? proj.width
				: proj.height;

			this.ApplyHitFx( proj.Center, dispersal, proj.damage );
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
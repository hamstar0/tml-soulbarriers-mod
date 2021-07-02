using System;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class Barrier {
		public void ApplyCollision( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				this.ApplyProjectileCollision( (Projectile)intruder );
			}
		}


		////////////////

		public void ApplyProjectileCollision( Projectile proj ) {
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
	}
}
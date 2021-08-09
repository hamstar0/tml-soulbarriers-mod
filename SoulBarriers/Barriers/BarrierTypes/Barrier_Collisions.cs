using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool CanCollide( Entity intruder ) {
			if( !this.IsActive ) {
				return false;
			}

			if( intruder is Projectile ) {
				return this.CanCollideVsProjectile( (Projectile)intruder );
			} else if( intruder is Player ) {
				return this.CanCollideVsPlayer( (Player)intruder );
			} else if( intruder is NPC ) {
				return this.CanCollideVsNpc( (NPC)intruder );
			}

			return false;
		}


		////////////////

		public bool IsColliding( Entity intruder ) {
			if( !this.CanCollide( intruder ) ) {
				return false;
			}

			return this.IsCollidingDirectly( intruder );
		}

		////

		public abstract bool IsCollidingDirectly( Entity intruder );
	}
}
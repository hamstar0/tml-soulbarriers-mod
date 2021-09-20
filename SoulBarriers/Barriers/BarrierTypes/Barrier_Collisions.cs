using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool CanEntityCollide( Entity intruder ) {
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

		public bool IsEntityColliding( Entity intruder ) {
			if( !this.CanEntityCollide(intruder) ) {
				return false;
			}

			return this.IsEntityCollidingPhysically( intruder );
		}

		////

		public abstract bool IsEntityCollidingPhysically( Entity intruder );
	}
}
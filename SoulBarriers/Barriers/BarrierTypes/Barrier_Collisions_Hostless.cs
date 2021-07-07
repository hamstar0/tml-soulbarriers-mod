using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool CanHostlessCollide( Entity intruder ) {
			if( intruder is Projectile ) {
				//return this.CanCollideHostlessVsProjectile( (Projectile)intruder );
			} else if( intruder is Player ) {
				return this.CanCollideHostlessVsPlayer( (Player)intruder );
			} else if( intruder is NPC ) {
				//return this.CanCollideHostlessVsNpc( (NPC)intruder );
			}

			return false;
		}

		////////////////

		private bool CanCollideHostlessVsPlayer( Player plr ) {
			return plr.active && !plr.dead;	// TODO: Currently indiscriminate
		}


		////////////////

		public abstract bool IsHostlessCollidingDirectly( Entity intruder );
	}
}
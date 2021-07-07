using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool CanCollide( Entity intruder ) {
			if( this.HostType == BarrierHostType.None ) {
				return this.CanHostlessCollide( intruder );
			} else {
				return this.CanHostedCollide( intruder );
			}
		}


		////////////////

		public bool IsColliding( Entity intruder ) {
			if( !this.CanCollide( intruder ) ) {
				return false;
			}

			if( this.Strength <= 0 ) {
				return false;
			}

			return this.IsCollidingDirectly( intruder );
		}

		////

		private bool IsCollidingDirectly( Entity intruder ) {
			if( this.HostType == BarrierHostType.None ) {
				return this.IsHostlessCollidingDirectly( intruder );
			} else {
				return this.IsHostedCollidingDirectly( intruder );
			}
		}
	}
}
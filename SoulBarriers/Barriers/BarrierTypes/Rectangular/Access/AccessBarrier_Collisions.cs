using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		private bool PreBarrierEntityHit( ref Entity intruder, ref double damage ) {
			if( !intruder.active ) {
				return false;
			}

			//

//DebugLibraries.ChatOnce( "b_col_ent_"+this.GetID(), "ent: "+intruder );
			if( intruder is Player ) {
				this.ApplyAccessPlayerHit( intruder as Player );
			} else if( intruder is NPC ) {
				this.ApplyAccessNpcHit( intruder as NPC );
			} else if( intruder is Projectile ) {
				this.ApplyAccessProjectileHit( intruder as Projectile );
			}

			return false;
		}


		private bool PreBarrierBarrierHit( Barrier otherBarrier, ref double damage ) {
			if( !otherBarrier.IsActive || otherBarrier is AccessBarrier ) {
				return false;
			}

			return true;
		}
	}
}
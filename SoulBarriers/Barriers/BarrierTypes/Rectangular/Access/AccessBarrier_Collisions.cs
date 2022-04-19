using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		private bool ApplyBarrierEntityHit( ref Entity intruder, ref double damage ) {
			if( !intruder.active ) {
				return false;
			}

			//

//DebugLibraries.ChatOnce( "b_col_ent_"+this.GetID(), "ent: "+intruder );
			if( intruder is Player ) {
				this.ApplyPlayerHit_If( intruder as Player );
			} else if( intruder is NPC ) {
				this.ApplyNpcHit_If( intruder as NPC );
			} else if( intruder is Projectile ) {
				this.ApplyProjectileHit_If( intruder as Projectile );
			}

			return false;
		}


		private bool ApplyBarrierBarrierHit( Barrier otherBarrier, ref double damage ) {
			if( !otherBarrier.IsActive || otherBarrier is AccessBarrier ) {
				return false;
			}

			return true;
		}
	}
}
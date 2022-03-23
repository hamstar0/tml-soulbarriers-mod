using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		private void OnPostBarrierEntityCollide( Entity intruder, bool isDefaultHit, double damage ) {
			if( !intruder.active ) {
				return;
			}

//DebugLibraries.ChatOnce( "b_col_ent_"+this.GetID(), "ent: "+intruder );
			if( intruder is Player ) {
				this.ApplyAccessPlayerHit( intruder as Player );
			} else if( intruder is NPC ) {
				this.ApplyAccessNpcHit( intruder as NPC );
			} else if( intruder is Projectile ) {
				this.ApplyAccessProjectileHit( intruder as Projectile );
			}
		}


		private void OnPostBarrierBarrierCollide( Barrier otherBarrier, bool isDefaultHit, double damage ) {
			if( !otherBarrier.IsActive || otherBarrier is AccessBarrier ) {
				return;
			}

			double myDamage = this.ComputeCollisionDamage( otherBarrier );
//LogLibraries.Log( "B V B OnBarrierBarrierCollision 1 - " + damage );

			if( damage > 0d ) {
				var toHitData = new BarrierHitContext( otherBarrier, myDamage );
				var froHitData = new BarrierHitContext( this, myDamage );

				//

				this.ApplyRawHit( null, myDamage, false, toHitData );
				otherBarrier.ApplyRawHit( null, myDamage, false, froHitData );
			}

			if( this.Strength == 0d ) {
				Main.NewText( "Access granted.", Color.Lime );
				Main.PlaySound( SoundID.Item94 );
			} else {
				//Main.NewText( "Access denied.", Color.Red );
			}
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		private void OnBarrierEntityCollide( Entity intruder ) {
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


		private void OnBarrierBarrierCollide( Barrier otherBarrier ) {
			if( !otherBarrier.IsActive || otherBarrier is AccessBarrier ) {
				return;
			}

			double damage = this.Strength > otherBarrier.Strength
				? otherBarrier.Strength
				: this.Strength;
			damage = Math.Ceiling( damage );
			//LogLibraries.Log( "B V B OnBarrierBarrierCollision 1 - " + damage );

			if( damage > 0d ) {
				this.ApplyRawHit( null, damage, false );
				otherBarrier.ApplyRawHit( null, damage, false );
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
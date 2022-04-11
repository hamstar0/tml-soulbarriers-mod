using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		private void PostBarrierBarrierHit( Barrier otherBarrier, bool isDefaultHit, double damage ) {
			if( !isDefaultHit ) {
				return;
			}

			//

			if( this.Strength <= 0d ) {
				Main.NewText( "Access granted.", Color.Lime );
				Main.PlaySound( SoundID.Item94 );
			} else {
				string str;
				if( (this.Strength % 1d) > 0d ) {
					str = ((int)this.Strength + (this.Strength % 1d)).ToString("N2");
				} else {
					str = ((int)this.Strength).ToString();
				}

				//

				Main.NewText( $"Gate barrier is too strong. +{str} strength needed to breach.", Color.Yellow );
				Main.PlaySound( SoundID.NPCHit53 );
			}
		}
	}
}
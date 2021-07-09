using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager : ILoadable {
		private void CheckCollisionsAgainstAllBarriers() {
			ISet<Barrier> collisionTestedBarriers = new HashSet<Barrier>( this.PlayerBarriers.Values );
			collisionTestedBarriers.UnionWith( this.WorldBarriers.Values );

			foreach( Barrier barrier in collisionTestedBarriers.ToArray() ) {
				collisionTestedBarriers.Remove( barrier );

				barrier.CheckCollisionsAgainstBarriers( collisionTestedBarriers );
			}
		}


		////////////////

		internal void CheckCollisionsAgainstEntity( Entity ent ) {
			foreach( (int plrWho, Barrier barrier) in this.PlayerBarriers ) {
				Player plr = Main.player[plrWho];
				if( plr?.active != true ) {
					continue;
				}

//Main.NewText( "projectile "+projectile.Name+" ("+projectile.whoAmI+") collides? "+plrBarrier.IsColliding(plr, projectile) );
				if( barrier.IsColliding( ent ) ) {
					barrier.ApplyCollisionHit( ent );
				}
			}

			foreach( Barrier barrier in this.WorldBarriers.Values ) {
				if( barrier.IsColliding( ent ) ) {
					barrier.ApplyCollisionHit( ent );
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager : ILoadable {
		private void CheckCollisionsAgainstAllBarriers() {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			ISet<Barrier> collisionTestedBarriers = new HashSet<Barrier>( this.BarriersByID.Values );

			foreach( Barrier barrier in collisionTestedBarriers.ToArray() ) {
				collisionTestedBarriers.Remove( barrier );

				barrier.CheckCollisionsAgainstBarriers( collisionTestedBarriers );
			}
		}


		////////////////

		internal void CheckCollisionsAgainstEntity( Entity ent ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			foreach( (int plrWho, Barrier barrier) in this.PlayerBarriers ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( barrier.Host == ent ) {
					continue;
				}

				Player plr = Main.player[plrWho];
				if( plr?.active != true ) {
					continue;
				}

//Main.NewText( "projectile "+projectile.Name+" ("+projectile.whoAmI+") collides? "+plrBarrier.IsColliding(plr, projectile) );
				if( barrier.IsColliding( ent ) ) {
					barrier.ApplyCollisionHit( ent, true );
				}
			}

			foreach( Barrier barrier in this.WorldBarriers.Values ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( barrier.Host == ent ) {
					continue;
				}

				if( barrier.IsColliding( ent ) ) {
					barrier.ApplyCollisionHit( ent, true );
				}
			}
		}
	}
}
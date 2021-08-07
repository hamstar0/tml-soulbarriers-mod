using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
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

			// Check for player barriers:
			foreach( (int plrWho, Barrier barrier) in this.PlayerBarriers ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( barrier.Host != null && barrier.Host == ent ) {
					continue;
				}

				Player plr = Main.player[plrWho];
				if( plr?.active != true ) {
					continue;
				}

/*DebugLibraries.Print(
	"pb_v_e_"+barrier.GetID()+"_"+ent,
	"collide? "+barrier.IsColliding(ent)
);*/
				if( barrier.IsColliding( ent ) ) {
					barrier.ApplyEntityCollisionHitIf( ent, true );
				}
			}

			// Check for world barriers:
			foreach( Barrier barrier in this.WorldBarriers.Values ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( barrier.Host != null && barrier.Host == ent ) {
					continue;
				}

/*DebugLibraries.Print( "wb_v_e_"+barrier.GetID(), "collide? "+barrier.IsColliding(ent)+", ent: "+new Rectangle(
	(int)ent.position.X,
	(int)ent.position.Y,
	ent.width,
	ent.height
) );*/
				if( barrier.IsColliding(ent) ) {
					barrier.ApplyEntityCollisionHitIf( ent, true );
				}
			}
		}
	}
}
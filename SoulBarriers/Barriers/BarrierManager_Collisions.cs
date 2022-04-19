using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		private void CheckCollisionsAgainstAllBarriers_Host() {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			//

			IEnumerable<Barrier> activeBarriers = this.BarriersByID.Values
				.Where( b => b.IsActive );

			foreach( Barrier barrier in activeBarriers ) {
				IList<Barrier> hitBarriers = barrier.CheckCollisionsAgainstBarriers_Host_If( activeBarriers );
				if( hitBarriers == null ) {
					continue;
				}

				//

				foreach( Barrier hitBarrier in hitBarriers ) {
					barrier.ApplyBarrierCollisionHit( hitBarrier, true, true );
				}
			}
		}


		////////////////

		internal void CheckCollisionsAgainstEntity( Entity ent ) {
			// Check for player barriers:
			foreach( Barrier barrier in this.PlayerBarriers.Values ) {
				if( !barrier.IsActive ) {
					continue;
				}

				// Owner cannot collide with their own barrier
				if( barrier.Host != null && barrier.Host == ent ) {
					continue;
				}

				if( barrier.Host?.active != true || (barrier.Host as Player)?.dead == true ) {
					continue;
				}

				//

//DebugLibraries.Print(
//	"pb_v_e_"+barrier.ID+"_"+ent,
//	"collide? "+barrier.IsEntityColliding( ent)
//);
				if( barrier.IsEntityColliding(ref ent) ) {
					barrier.ApplyEntityCollisionHit_If_Syncs( ent, null, true );

					if( !ent.active ) {
						return;
					}
				}
			}

			//

			// Check for npc barriers:
			foreach( Barrier barrier in this.NPCBarriers.Values ) {
				if( !barrier.IsActive ) {
					continue;
				}

				// Owner cannot collide with their own barrier
				if( barrier.Host != null && barrier.Host == ent ) {
					continue;
				}

				if( barrier.Host?.active != true ) {
					continue;
				}

				//

				if( barrier.IsEntityColliding(ref ent) ) {
					barrier.ApplyEntityCollisionHit_If_Syncs( ent, null, true );

					if( !ent.active ) {
						return;
					}
				}
			}

			//

			// Check for world barriers:
			foreach( Barrier barrier in this.TileBarriers.Values ) {
				if( !barrier.IsActive ) {
					continue;
				}

				// Owner cannot collide with their own barrier
				if( barrier.Host != null && barrier.Host == ent ) {
					continue;
				}

				//

/*DebugLibraries.Print( "wb_v_e_"+barrier.GetID(), "collide? "+barrier.IsColliding(ent)+", ent: "+new Rectangle(
	(int)ent.position.X,
	(int)ent.position.Y,
	ent.width,
	ent.height
) );*/
				if( barrier.IsEntityColliding(ref ent) ) {
					barrier.ApplyEntityCollisionHit_If_Syncs( ent, null, true );

					if( !ent.active ) {
						return;
					}
				}
			}
		}
	}
}
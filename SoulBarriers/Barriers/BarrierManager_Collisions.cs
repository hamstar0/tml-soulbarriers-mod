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
	partial class BarrierManager : ILoadable {
		private void CheckCollisionsAgainstAllBarriers() {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			IEnumerable<Barrier> activeBarriers = this.BarriersByID.Values
				.Where( b => b.IsActive );

			foreach( Barrier barrier in activeBarriers ) {
				barrier.CheckCollisionsAgainstBarriers( activeBarriers );
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
				if( plr?.active != true || plr.dead ) {
					continue;
				}

/*DebugLibraries.Print(
	"pb_v_e_"+barrier.GetID()+"_"+ent,
	"collide? "+barrier.IsColliding(ent)
);*/
				if( barrier.IsEntityColliding( ent ) ) {
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
				if( barrier.IsEntityColliding(ent) ) {
					barrier.ApplyEntityCollisionHitIf( ent, true );
				}
			}
		}
	}
}
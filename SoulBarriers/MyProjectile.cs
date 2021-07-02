using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	class SoulBarriersProjectile : GlobalProjectile {
		public override bool InstancePerEntity => true;



		////////////////

		public override bool PreAI( Projectile projectile ) {
			//NPCID.Sets.ProjectileNPC
			if( SoulBarriersMod.BarrierMngr.GetPlayerBarrierCount() >= 1 ) {
				this.CheckBarriersHit( projectile );
			}

			return base.PreAI( projectile );
		}

		////

		private void CheckBarriersHit( Projectile projectile ) {
			foreach( (int plrWho, Barrier plrBarrier) in SoulBarriersMod.BarrierMngr.GetPlayerBarriers() ) {
				Player plr = Main.player[plrWho];
				if( plr?.active != true ) {
					continue;
				}

//Main.NewText( "projectile "+projectile.Name+" ("+projectile.whoAmI+") collides? "+plrBarrier.IsColliding(plr, projectile) );
				if( plrBarrier.IsColliding(plr, projectile) ) {
					plrBarrier.ApplyCollision( plr, projectile );
				}
			}
		}
	}
}
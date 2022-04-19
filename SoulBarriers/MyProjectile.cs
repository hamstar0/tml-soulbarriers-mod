using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	class SoulBarriersProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				BarrierManager.Instance.CheckCollisionsAgainstEntity( projectile );
			}

			return base.PreAI( projectile );
		}
	}
}
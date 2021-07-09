using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	class SoulBarriersProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
			BarrierManager.Instance.CheckCollisionsAgainstEntity( projectile );

			return base.PreAI( projectile );
		}
	}
}
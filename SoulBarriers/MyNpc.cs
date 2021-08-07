using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	class SoulBarriersNPC : GlobalNPC {
		public override bool PreAI( NPC npc ) {
			BarrierManager.Instance.CheckCollisionsAgainstEntity( npc );

			return base.PreAI( npc );
		}
	}
}
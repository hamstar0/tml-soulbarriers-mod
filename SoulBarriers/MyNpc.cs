using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	class SoulBarriersNPC : GlobalNPC {
		internal bool BlockLoot = false;


		////////////////

		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override bool SpecialNPCLoot( NPC npc ) {
			return this.BlockLoot;
		}

		////////////////

		public override bool PreAI( NPC npc ) {
			BarrierManager.Instance.CheckCollisionsAgainstEntity( npc );

			return base.PreAI( npc );
		}
	}
}
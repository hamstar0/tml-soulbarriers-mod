using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using ModLibsGeneral.Libraries.NPCs;


namespace SoulBarriers {
	class SoulBarriersNPC : GlobalNPC {
		internal bool KillFromBarrier = false;


		////////////////

		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override bool SpecialNPCLoot( NPC npc ) {
			return this.KillFromBarrier;
		}

		////////////////

		public override bool PreAI( NPC npc ) {
			BarrierManager.Instance.CheckCollisionsAgainstEntity( npc );

			if( this.KillFromBarrier ) {
				npc.HitEffect( 1 );
				NPCLibraries.Kill( npc, Main.netMode == NetmodeID.Server );
			}

			return !this.KillFromBarrier;
		}
	}
}
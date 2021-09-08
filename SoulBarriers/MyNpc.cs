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
				npc.HitEffect();
				npc.life = 0;
				npc.checkDead();
				
				if( Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npc.whoAmI );
				}
			}

			return !this.KillFromBarrier;
		}
	}
}
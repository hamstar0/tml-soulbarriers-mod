using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	partial class SoulBarriersNPC : GlobalNPC {
		internal bool KillFromBarrier = false;


		////////////////

		public Barrier Barrier { get; private set; }


		////////////////
		
		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override bool SpecialNPCLoot( NPC npc ) {
			return this.KillFromBarrier;
		}

		////////////////

		public override bool PreAI( NPC npc ) {
			if( npc.life > 0 ) {
				BarrierManager.Instance.CheckCollisionsAgainstEntity( npc );
			}

			if( this.KillFromBarrier ) {
				npc.HitEffect();
				npc.life = 0;
				npc.checkDead();
				
				if( Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npc.whoAmI );
				}
			}

			this.AnimateBarrierFxIf();

			return !this.KillFromBarrier;
		}
	}
}
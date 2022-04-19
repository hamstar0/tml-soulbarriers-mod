using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	partial class SoulBarriersNPC : GlobalNPC {
		internal bool KillFromBarrier_Host = false;


		////////////////

		public Barrier Barrier { get; private set; }


		////////////////
		
		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override bool SpecialNPCLoot( NPC npc ) {
			return this.KillFromBarrier_Host;
		}


		////////////////

		public override bool PreAI( NPC npc ) {
			bool isKilledByBarrier = this.KillFromBarrier_Host
				&& Main.netMode != NetmodeID.MultiplayerClient;

			this.UpdateForBarriers( npc, isKilledByBarrier );

			return !isKilledByBarrier;
		}


		////////////////

		private void UpdateForBarriers( NPC npc, bool isKilledByBarrier ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				if( !isKilledByBarrier && npc.life > 0 ) {
					BarrierManager.Instance.CheckCollisionsAgainstEntity( npc );
				}

				//

				if( isKilledByBarrier ) {
					npc.HitEffect();
					npc.life = 0;
					npc.checkDead();

					//
				
					if( Main.netMode == NetmodeID.Server ) {
						NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npc.whoAmI );
					}
				}
			}

			//

			if( !isKilledByBarrier ) {
				this.AnimateBarrierFx_If();
			}
		}
	}
}
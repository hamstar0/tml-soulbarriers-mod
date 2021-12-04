using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	class SoulBarriersNPC : GlobalNPC {
		public static void ApplySpawnBarrierIf( int npcWho ) {
			NPC npc = Main.npc[ npcWho ];
			if( npc?.active != true ) {
				return;
			}

			if( npc.friendly ) {
				return;
			}
			if( npc.realLife >= 0 ) {	// only the main part
				return;
			}
			if( npc.immortal || npc.dontTakeDamage ) {
				return;
			}
			if( npc.aiStyle == 14 ) {	// bats
				return;
			}
			if( NPCID.Sets.ProjectileNPC[npc.type] ) {
				return;
			}

			//

			var config = SoulBarriersConfig.Instance;

			int strength = npc.lifeMax + 50;
			float strengthRegenPerTick = config.Get<float>( nameof( config.NPCBarrierDefaultRegenPercentPerTick ) );
			//float radius = config.Get<float>( nameof( config.DefaultNPCBarrierRadius ) );
			float radius = npc.scale
				* (float)Math.Sqrt( (double)(npc.width * npc.width) + (double)(npc.height * npc.height) )
				* 2f;

			Barrier barrier = BarrierManager.Instance.CreateAndDeclareActiveNPCBarrier(
				npcWho,
				strength,
				strengthRegenPerTick,
				radius
			);
		}



		////////////////

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

			return !this.KillFromBarrier;
		}
	}
}
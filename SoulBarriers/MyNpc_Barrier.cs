using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	partial class SoulBarriersNPC : GlobalNPC {
		public static void ApplySpawnBarrierIf( int npcWho ) {
			NPC npc = Main.npc[ npcWho ];
			if( npc?.active != true ) {
				return;
			}

			if( npc.lifeMax <= 5 ) {	// i guess critter failsafe
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
				* (float)Math.Sqrt( (double)((npc.width * npc.width) + (npc.height * npc.height)) )
				* 1.25f;

			var mynpc = npc.GetGlobalNPC<SoulBarriersNPC>();
			mynpc.Barrier = BarrierManager.Instance.CreateAndDeclareActiveNPCBarrier(
				npcWho,
				strength,
				strengthRegenPerTick,
				radius
			);
		}


		////////////////

		private void AnimateBarrierFxIf() {
			if( this.Barrier == null || !this.Barrier.IsActive ) {
				return;
			}

			int particles = this.Barrier.ComputeCappedNormalParticleCount();

			this.Barrier.Animate( particles );
		}
	}
}
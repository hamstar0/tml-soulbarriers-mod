using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	partial class SoulBarriersNPC : GlobalNPC {
		public static bool CanSpawnWithBarrier( int npcWho ) {
			NPC npc = Main.npc[npcWho];
			if( npc?.active != true ) {
				return false;
			}

			return npc.lifeMax > 5    // i guess critter failsafe
				&& !npc.friendly
				&& npc.realLife < 0	// only the main part
				&& !npc.immortal 
				&& !npc.dontTakeDamage
				&& npc.aiStyle != 14	// no bats
				&& !NPCID.Sets.ProjectileNPC[npc.type];
		}


		public static bool RollBarrierSpawnChance() {
			var config = SoulBarriersConfig.Instance;
			float perc = config.Get<float>( nameof(config.NPCBarrierRandomPercentChance) );

			return Main.rand.NextFloat() < perc;
		}


		////////////////

		public static bool ApplySpawnBarrierIf( int npcWho ) {
			if( SoulBarriersNPC.CanSpawnWithBarrier(npcWho) ) {
				if( SoulBarriersNPC.RollBarrierSpawnChance() ) {
					SoulBarriersNPC.ApplySpawnBarrier( npcWho );

					return true;
				}
			}

			return false;
		}


		public static void ApplySpawnBarrier( int npcWho ) {
			NPC npc = Main.npc[npcWho];
			var config = SoulBarriersConfig.Instance;
			
			int strength = npc.lifeMax * config.Get<int>( nameof(config.NPCBarrierLifeToStrengthScale) );
			strength += config.Get<int>( nameof(config.NPCBarrierStrengthAdded) );

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
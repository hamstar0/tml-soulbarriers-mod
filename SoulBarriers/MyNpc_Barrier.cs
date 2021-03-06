using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Packets;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical.Personal;


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

		public static Barrier ApplySpawnBarrier_If(
					int npcWho,
					int? customStrength,
					float? customStrengthRegenPerTick,
					bool noRandomChance,
					bool syncIfServer ) {
			if( !SoulBarriersNPC.CanSpawnWithBarrier(npcWho) ) {
				return null;
			}

			if( !noRandomChance && !SoulBarriersNPC.RollBarrierSpawnChance() ) {
				return null;
			}

			//

			return SoulBarriersNPC.ApplySpawnBarrier(
				npcWho,
				customStrength,
				customStrengthRegenPerTick,
				syncIfServer
			);
		}


		////

		public static Barrier ApplySpawnBarrier(
					int npcWho,
					int? customStrength,
					float? customStrengthRegenPerTick,
					bool syncIfServer ) {
			NPC npc = Main.npc[npcWho];
			var config = SoulBarriersConfig.Instance;

			//
			
			int strength;

			if( customStrength.HasValue ) {
				strength = customStrength.Value;
			} else {
				strength = (int)((float)npc.lifeMax * config.Get<float>(nameof(config.NPCBarrierLifeToStrengthScale)) );
				strength += config.Get<int>( nameof(config.NPCBarrierStrengthAdded) );
			}

			//

			float strengthRegenPerTick;

			if( customStrengthRegenPerTick.HasValue ) {
				strengthRegenPerTick = customStrengthRegenPerTick.Value;
			} else {
				strengthRegenPerTick = config.Get<float>( nameof(config.NPCBarrierDefaultRegenPercentPerTick) );
			}

			//

			//float radius = config.Get<float>( nameof( config.DefaultNPCBarrierRadius ) );
			float radius = npc.scale
				* (float)Math.Sqrt( (double)((npc.width * npc.width) + (npc.height * npc.height)) )
				* 1.25f;

			//

			var mynpc = npc.GetGlobalNPC<SoulBarriersNPC>();
			mynpc.Barrier = BarrierManager.Instance.CreateAndDeclareActiveNPCBarrier(
				npcWho: npcWho,
				strength: strength,
				strengthRegenPerTick: strengthRegenPerTick,
				radius: radius
			);

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				NPCBarrierCreatePacket.BroadcastToClients( (PersonalBarrier)mynpc.Barrier );
			}

			return mynpc.Barrier;
		}



		////////////////

		private void AnimateBarrierFx_If() {
			if( this.Barrier == null || !this.Barrier.IsActive ) {
				return;
			}

			int particles = this.Barrier.ComputeCappedNormalParticleCount();

			this.Barrier.Animate( particles );
		}
	}
}
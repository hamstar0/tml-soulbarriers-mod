using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.NPCHooks;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-soulbarriers-mod";


		////////////////

		public static SoulBarriersMod Instance => ModContent.GetInstance<SoulBarriersMod>();



		////////////////

		public override void PostSetupContent() {
			NPCHooks.AddSpawnNPCHook( ( npcWho ) => SoulBarriersNPC.ApplySpawnBarrierIf(npcWho) );
		}


		////////////////

		public override void PreSaveAndQuit() {
			BarrierManager.Instance.RemoveAllNPCBarriers( false );
			BarrierManager.Instance.RemoveAllPlayerBarriers( false );
		}


		////////////////

		public override void MidUpdateTimeWorld() {
			BarrierManager.Instance.UpdateAllTrackedBarriers();

			this.AnimateWorldBarrierFx();
		}
	}
}
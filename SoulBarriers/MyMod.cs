using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-soulbarriers-mod";


		////////////////

		public static SoulBarriersMod Instance { get; private set; }



		////////////////

		public override void Load() {
			SoulBarriersMod.Instance = this;
		}

		public override void Unload() {
			SoulBarriersMod.Instance = null;
		}


		////////////////

		public override void PreSaveAndQuit() {
			BarrierManager.Instance.RemoveAllPlayerBarriers();
		}


		////////////////

		public override void MidUpdateTimeWorld() {
			BarrierManager.Instance.UpdateAllTrackedBarriers();

			this.AnimateWorldBarrierFx();
		}
	}
}
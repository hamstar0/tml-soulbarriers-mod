using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public class SoulBarriersMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-soulbarriers-mod";


		////////////////

		public static SoulBarriersMod Instance { get; private set; }

		public static BarrierManager BarrierMngr { get; private set; }



		////////////////

		public override void Load() {
			SoulBarriersMod.Instance = this;
			SoulBarriersMod.BarrierMngr = new BarrierManager();
		}

		public override void Unload() {
			SoulBarriersMod.Instance = null;
			SoulBarriersMod.BarrierMngr = null;
		}


		public override void PostUpdateEverything() {
			SoulBarriersMod.BarrierMngr.UpdateAllTrackedBarriers();
		}
	}
}
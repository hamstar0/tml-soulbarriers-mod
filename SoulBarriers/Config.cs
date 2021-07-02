using ModLibsCore.Classes.UI.ModConfig;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace SoulBarriers {
	class MyFloatInputElement : FloatInputElement { }




	public partial class SoulBarriersConfig : ModConfig {
		public static SoulBarriersConfig Instance => ModContent.GetInstance<SoulBarriersConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		[DefaultValue(true)]
		public bool PBGRecipeEnabled { get; set; } = true;



		[Range(10f, 500f)]
		[DefaultValue(48f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DefaultPlayerBarrierRadius { get; set; } = 48f;
	}
}
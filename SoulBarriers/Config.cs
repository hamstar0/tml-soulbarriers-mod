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


		////////////////

		[DefaultValue(true)]
		public bool PBGRecipeEnabled { get; set; } = true;


		[Range(0f, 10f)]
		[DefaultValue(1f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PBGBarrierStrengthScale { get; set; } = 10f;


		[Range(0, 60 * 60)]
		[DefaultValue( 30 )]
		public int PBGOverheatDurationSeconds { get; set; } = 30;


		[Range(10f, 500f)]
		[DefaultValue(48f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DefaultPlayerBarrierRadius { get; set; } = 48f;


		[Range(0, 500)]
		[DefaultValue(20)]
		public int BarrierDebuffRemovalCost { get; set; } = 48;
	}
}
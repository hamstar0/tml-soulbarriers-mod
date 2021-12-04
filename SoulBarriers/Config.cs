using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.UI.ModConfig;


namespace SoulBarriers {
	class MyFloatInputElement : FloatInputElement { }




	public partial class SoulBarriersConfig : ModConfig {
		public static SoulBarriersConfig Instance => ModContent.GetInstance<SoulBarriersConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		public bool DebugModeInfo { get; set; } = false;

		public bool DebugModeNetInfo { get; set; } = false;

		public bool DebugModeWorldBarrierTest { get; set; } = false;
		

		////

		[DefaultValue(true)]
		public bool PBGRecipeEnabled { get; set; } = true;

		////

		[Range(0f, 10f)]
		[DefaultValue(1f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PBGBarrierStrengthScale { get; set; } = 1f;

		//
		
		[Range(0f, 1f)]
		[DefaultValue(-1f / (60f * 3f))]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PBGBarrierDefaultRegenPercentPerTick { get; set; } = -1f / (60f * 3f);
		
		[Range(0f, 1f)]
		[DefaultValue(-1f / 60f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PBGBarrierJungleRegenPercentPerTick { get; set; } = -1f / 60f;
		
		[Range(0f, 1f)]
		[DefaultValue(-1f / 60f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PBGBarrierUnderworldRegenPercentPerTick { get; set; } = -1f / 60f;

		//
		
		[Range(0f, 1f)]
		[DefaultValue(1f / 60f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float NPCBarrierDefaultRegenPercentPerTick { get; set; } = 1f / 60f;

		//

		[Range(0, 60 * 60)]
		[DefaultValue( 20 )]
		public int PBGOverheatDurationSeconds { get; set; } = 20;

		////


		[Range( 0f, 100f )]
		[DefaultValue( 8f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float RectangleBarrierParticleMultiplier { get; set; } = 8f;

		[Range( 0f, 100f )]
		[DefaultValue( 4f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SphericalBarrierParticleMultiplier { get; set; } = 4f;


		////

		[Range(10f, 500f)]
		[DefaultValue(48f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float DefaultPlayerBarrierRadius { get; set; } = 48f;

		////

		[Range(0f, 500f)]
		[DefaultValue(20f)]
		public float BarrierDebuffRemovalCost { get; set; } = 20f;

		////

		public HashSet<ProjectileDefinition> BarrierProjectileWhitelist { get; set; }
				= new HashSet<ProjectileDefinition> {
			new ProjectileDefinition( ProjectileID.Boulder ),
			new ProjectileDefinition( ProjectileID.SandBallFalling ),
			new ProjectileDefinition( ProjectileID.PearlSandBallFalling ),
			new ProjectileDefinition( ProjectileID.CrimsandBallFalling ),
			new ProjectileDefinition( ProjectileID.EbonsandBallFalling ),
			new ProjectileDefinition( ProjectileID.SiltBall ),
			new ProjectileDefinition( ProjectileID.SlushBall ),
		};
	}
}
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "P.B.G" );
			this.Tooltip.SetDefault(
				"Erects a temporary magical protective barrier upon the wielder"
				+ "\nBarriers protect against projectiles and spirital attacks"
				+ "\nThe P.B.G can only be activated with full mana"
				+ "\nBarrier strength is determined by available mana"
				+ "\nBarrier activation causes the P.B.G to overheat briefly"
			);
		}

		public override void SetDefaults() {
			item.width = 20;
			item.height = 12;
			item.useTime = 16;
			item.useAnimation = 16;
			item.autoReuse = false;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice( gold: 2 );
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item8;
		}

		////

		public override void AddRecipes() {
			var recipe = new PBGItemRecipe( this );
			recipe.AddRecipe();
		}
	}
}
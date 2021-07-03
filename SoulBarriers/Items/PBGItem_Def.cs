using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public int HeatBuildup { get; internal set; } = 0;

		public bool IsOverheated { get; private set; } = false;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "P.B.G" );
			this.Tooltip.SetDefault(
				"Erects a temporary magical protective barrier upon the wielder"
				+ "\nBarriers protect against projectiles and spirital attacks"
				+ "\nThe P.B.G consumes all mana to activate its barrier"
				+ "\nBarrier strength is based on available mana upon activation"
				+ "\nBarrier activation causes the P.B.G to heat up"
				+ "\nThe P.B.G locks up if too hot, but cools down when inactive"
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


		////////////////

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( this.HeatBuildup );
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.HeatBuildup = reader.ReadInt32();
		}
	}
}
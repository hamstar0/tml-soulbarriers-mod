using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public class PBGItem : ModItem {
		public int PostChargeSafetyTimer { get; internal set; } = 0;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "P.B.G" );
			this.Tooltip.SetDefault(
				"Erects a temporary magical protective barrier upon the wielder"
				+"\nBarriers protect against projectiles and spirital attacks"
				+"\nThe P.B.G draws mana to activate its barrier"
				+"\nBarrier strengthens the longer the button is (continously) held"
			);
		}

		public override void SetDefaults() {
			item.width = 20;
			item.height = 12;
			item.useTime = 8;
			item.useAnimation = 8;
			item.autoReuse = true;
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

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			int barrierStr = myplayer.GetBarrierStrength();
			var tip = new TooltipLine(
				this.mod,
				"PlayerBarrierStrength",
				"Current barrier strength: "+barrierStr
			);

			if( barrierStr <= 0 ) {
				tip.overrideColor = Color.DarkGray;
			} else if( barrierStr >= Main.LocalPlayer.statManaMax2 ) {
				tip.overrideColor = Main.DiscoColor;
			} else {
				tip.overrideColor = Color.OrangeRed;
			}

			tooltips.Add( tip );
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			if( this.PostChargeSafetyTimer >= 1 ) {
				this.PostChargeSafetyTimer = 30;
			}

			return player.statMana >= 1
				&& this.PostChargeSafetyTimer <= 0;
		}

		public override bool UseItem( Player player ) {
			if( player.statMana >= 1 ) {
				int mana = player.statMana > 3
					? 3
					: player.statMana > 2
					? 2
					: 1;
				player.statMana -= mana;

				player.GetModPlayer<SoulBarriersPlayer>().AddBarrier( mana );
			} else {
				this.PostChargeSafetyTimer = 30;
			}

			return true;
		}


		public override void UpdateInventory( Player player ) {
			if( this.PostChargeSafetyTimer > 0 ) {
				this.PostChargeSafetyTimer--;
			}
		}
	}




	class PBGItemRecipe : ModRecipe {
		public PBGItemRecipe( PBGItem myitem ) : base( SoulBarriersMod.Instance ) {
			this.AddIngredient( ItemID.Nanites, 10 );
			this.AddIngredient( ItemID.Umbrella, 1 );
			this.AddTile( TileID.Anvils );
			this.SetResult( myitem );
		}


		public override bool RecipeAvailable() {
			var config = SoulBarriersConfig.Instance;
			return config.Get<bool>( nameof(config.PBGRecipeEnabled) );
		}
	}
}
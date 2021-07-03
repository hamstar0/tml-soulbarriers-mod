using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public class PBGItem : ModItem {
		public int HeatBuildup { get; internal set; } = 0;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "P.B.G" );
			this.Tooltip.SetDefault(
				"Erects a temporary magical protective barrier upon the wielder"
				+"\nBarriers protect against projectiles and spirital attacks"
				+"\nThe P.B.G consumes all mana to activate its barrier"
				+"\nBarrier strength is based on available mana upon activation"
				+"\nBarrier activation causes the P.B.G to heat up"
				+"\nThe P.B.G locks up if too hot, but cools down when inactive"
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

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			int barrierStr = myplayer.GetBarrierStrength();
			var tip1 = new TooltipLine(
				this.mod,
				"PlayerBarrierStrength",
				"Current barrier strength: "+barrierStr
			);

			if( barrierStr <= 0 ) {
				tip1.overrideColor = Color.DarkGray;
			} else if( barrierStr >= Main.LocalPlayer.statManaMax2 ) {
				tip1.overrideColor = Main.DiscoColor;
			} else {
				tip1.overrideColor = Color.OrangeRed;
			}

			//

			var tip2 = new TooltipLine(
				this.mod,
				"PlayerBarrierHear",
				"Current barrier overheat: %" + this.HeatBuildup
			);

			if( this.HeatBuildup <= 0 ) {
				tip2.overrideColor = Color.Lime;
			} else if( this.HeatBuildup <= 75 ) {
				tip2.overrideColor = Color.White;
			} else if( this.HeatBuildup <= 99 ) {
				tip2.overrideColor = Color.Yellow;
			} else {
				tip2.overrideColor = Color.Red;
			}

			//

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}


		////////////////

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( this.HeatBuildup );
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.HeatBuildup = reader.ReadInt32();
		}


		////////////////

		public override bool CanUseItem( Player player ) {
			if( this.HeatBuildup >= 100 ) {
				return false;
			}

			return player.statMana >= 5;
		}

		public override bool UseItem( Player player ) {
			int mana = player.statMana;

			this.HeatBuildup += mana;

			player.GetModPlayer<SoulBarriersPlayer>().AddBarrier( mana );

			player.statMana = 0;

			return true;
		}


		////////////////

		public override void UpdateInventory( Player player ) {
			if( this.HeatBuildup >= 1 ) {
				var myplayer = player.GetModPlayer<SoulBarriersPlayer>();

				if( myplayer.GetBarrierStrength() <= 0 ) {
					this.HeatBuildup--;
				}
			}
		}

		public override void Update( ref float gravity, ref float maxFallSpeed ) {
			if( this.HeatBuildup >= 1 ) {
				this.HeatBuildup--;
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
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	class PBGItemRecipe : ModRecipe {
		public PBGItemRecipe( PBGItem myitem ) : base( SoulBarriersMod.Instance ) {
			this.AddIngredient( ItemID.Nanites, 10 );
			this.AddIngredient( ItemID.MeteoriteBar, 10 );
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
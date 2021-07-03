using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
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
}
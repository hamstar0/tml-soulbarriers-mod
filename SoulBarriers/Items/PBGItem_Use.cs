using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
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
	}
}
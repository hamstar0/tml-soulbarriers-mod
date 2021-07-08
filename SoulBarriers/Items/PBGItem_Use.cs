using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override bool CanUseItem( Player player ) {
			return player.statMana >= player.statManaMax2
				&& !player.HasBuff( ModContent.BuffType<PBGOverheatedDeBuff>() );
				//&& !player.HasBuff( BuffID.ManaSickness );
		}

		public override bool UseItem( Player player ) {
			var config = SoulBarriersConfig.Instance;

			float barrierStrScale = config.Get<float>( nameof(config.PBGBarrierStrengthScale) );
			int barrierStr = (int)(barrierStrScale * (float)player.statMana);

			player.GetModPlayer<SoulBarriersPlayer>()
				.AddBarrier( barrierStr );

			player.statMana = 0;

			int seconds = config.Get<int>( nameof(config.PBGOverheatDurationSeconds) );
			if( seconds > 0 ) {
				player.AddBuff( ModContent.BuffType<PBGOverheatedDeBuff>(), seconds * 60 );
			}

			return true;
		}
	}
}
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
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

			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			myplayer.Barrier.SetStrength( barrierStr, true, true );

			player.statMana = 0;

			int overheatDeBuff = ModContent.BuffType<PBGOverheatedDeBuff>();
			int seconds = config.Get<int>( nameof(config.PBGOverheatDurationSeconds) );
			if( seconds >= 1 ) {
				player.AddBuff( overheatDeBuff, seconds * 60 );
			}

			return true;
		}
	}
}
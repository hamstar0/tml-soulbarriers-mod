using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Buffs;
using SoulBarriers.Packets;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override bool CanUseItem( Player player ) {
			if( player.statMana < 10 ) {
				return false;
			}
			/*if( player.statMana < player.statManaMax2 ) {
				return false;
			}*/
			if( player.HasBuff( ModContent.BuffType<PBGOverheatedDeBuff>() ) ) {
				return false;
			}
			/*if( player.HasBuff( BuffID.ManaSickness ) ) {
				return false;
			}*/
			return true;
		}

		public override bool UseItem( Player player ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			var config = SoulBarriersConfig.Instance;

			float barrierStrScale = config.Get<float>( nameof(config.PBGBarrierStrengthScale) );
			int barrierStr = (int)(barrierStrScale * (float)player.statMana);

			//

			myplayer.Barrier.SetStrength( barrierStr, true, true, true );

			//

			player.statMana = 0;

			//

			int overheatDeBuff = ModContent.BuffType<PBGOverheatedDeBuff>();
			int seconds = config.Get<int>( nameof(config.PBGOverheatDurationSeconds) );
			if( seconds >= 1 ) {
				player.AddBuff( overheatDeBuff, seconds * 60 );
			}

			//

			if( Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer ) {
				BarrierStrengthPacket.SyncToServerForEveryone(
					barrier: myplayer.Barrier,
					strength: barrierStr,
					applyHitFx: false,
					clearRegenBuffer: true
				);
			}

			//

			return true;
		}
	}
}
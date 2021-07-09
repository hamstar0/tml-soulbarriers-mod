using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers;
using Terraria.ID;
using ModLibsCore.Classes.Errors;

namespace SoulBarriers {
	public static class SoulBarriersAPI {
		public static Barrier GetPlayerBarrier( Player player ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			return myplayer.Barrier;
		}

		//public static SphericalBarrier GetNpcBarrier( NPC npc ) { }

		public static Barrier GetWorldBarrier( Rectangle worldArea ) {
			return BarrierManager.Instance.GetWorldBarrier( worldArea );
		}

		public static Barrier CreateWorldBarrier(
					Rectangle worldArea,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					BarrierColor color ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			return BarrierManager.Instance.CreateAndDeclareWorldBarrier(
				hostType: BarrierHostType.None,
				hostWhoAmI: -1,
				worldArea: worldArea,
				strength: strength,
				maxRegenStrength: maxRegenStrength,
				strengthRegenPerTick: strengthRegenPerTick,
				color: color,
				syncFromServer: true
			);
		}

		public static bool RemoveWorldBarrier( Rectangle worldArea ) {
			return BarrierManager.Instance.RemoveWorldBarrier( worldArea );
		}
	}
}

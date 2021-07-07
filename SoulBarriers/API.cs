using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers;


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
			return BarrierManager.Instance.CreateAndDeclareWorldBarrier(
				worldArea,
				strength,
				maxRegenStrength,
				strengthRegenPerTick,
				color
			);
		}

		public static bool RemoveWorldBarrier( Rectangle worldArea ) {
			return BarrierManager.Instance.RemoveWorldBarrier( worldArea );
		}
	}
}

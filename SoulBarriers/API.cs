using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public static class SoulBarriersAPI {
		public static SphericalBarrier GetPlayerBarrier( Player player ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			return myplayer.Barrier;
		}

		////

		//public static SphericalBarrier GetNpcBarrier( NPC npc ) { }

		////

		public static RectangularBarrier GetWorldBarrier( Rectangle worldArea ) {
			return BarrierManager.Instance.GetWorldBarrier( worldArea );
		}

		public static bool RemoveWorldBarrier( Rectangle worldArea ) {
			return BarrierManager.Instance.RemoveWorldBarrier( worldArea );
		}
	}
}

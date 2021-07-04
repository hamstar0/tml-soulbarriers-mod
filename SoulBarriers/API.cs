using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public static class SoulBarriersAPI {
		public static Barrier GetPlayerBarrier( Player player ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			return myplayer.Barrier;
		}

		public static void HitPlayerBarrier( Player player, Entity entity ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			myplayer.Barrier.ApplyCollisionHit( player, entity );
		}

		public static void HitPlayerBarrier( Player player, int damage ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			myplayer.Barrier.ApplyRawHit( damage );
		}

		////

		//public static Barrier GetNpcBarrier( NPC npc ) { }

		//public static void HitNpcBarrier( NPC npc, Entity entity ) { }

		//public static void HitNpcBarrier( NPC npc, int damage ) { }

		////

		public static Barrier GetAreaBarrier( Rectangle area ) {
			var myworld = ModContent.GetInstance<SoulBarriersWorld>();
			return myworld.GetBarrierAt( area );
		}

		public static void RemoveAreaBarrier( Rectangle area ) {
			var myworld = ModContent.GetInstance<SoulBarriersWorld>();
			myworld.RemoveBarrierAt( area );
		}

		public static void HitAreaBarrier( Rectangle area, Entity entity ) {
			var myworld = ModContent.GetInstance<SoulBarriersWorld>();
			Barrier barrier = myworld.GetBarrierAt( area );
			barrier.ApplyCollisionHit( area, entity );
		}

		public static void HitAreaBarrier( Rectangle area, int damage ) {
			var myworld = ModContent.GetInstance<SoulBarriersWorld>();
			Barrier barrier = myworld.GetBarrierAt( area );
			barrier.ApplyRawHit( area, damage );
		}
	}
}

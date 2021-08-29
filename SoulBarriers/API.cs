using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers;
using System.Linq;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


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


		////

		public static Barrier[] GetWorldBarriers() {
			return BarrierManager.Instance.GetWorldBarriers()
				.Values
				.ToArray();
		}


		////

		public static bool DeclareWorldBarrier( RectangularBarrier barrier ) {
					/*Rectangle worldArea,
					double strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					BarrierColor color,
					bool isSaveable ) {*/
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			/*return BarrierManager.Instance.CreateAndDeclareWorldBarrier(
				hostType: BarrierHostType.None,
				hostWhoAmI: -1,
				worldArea: worldArea,
				strength: strength,
				maxRegenStrength: maxRegenStrength,
				strengthRegenPerTick: strengthRegenPerTick,
				color: color,
				isSaveable: isSaveable,
				syncFromServer: true
			);*/
			return BarrierManager.Instance.DeclareWorldBarrier( barrier, true );
		}

		public static void RemoveWorldBarrier( Rectangle worldArea ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			BarrierManager.Instance.RemoveWorldBarrier( worldArea, true );
		}
	}
}

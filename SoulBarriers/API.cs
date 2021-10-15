using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;
using SoulBarriers.Packets;


namespace SoulBarriers {
	public partial class SoulBarriersAPI : ILoadable {
		public static Barrier GetPlayerBarrier( Player player ) {
			var myplayer = player.GetModPlayer<SoulBarriersPlayer>();
			return myplayer.Barrier;
		}

		//public static SphericalBarrier GetNpcBarrier( NPC npc ) { }

		public static Barrier GetWorldBarrier( Rectangle tileArea ) {
			return BarrierManager.Instance.GetWorldBarrier( tileArea );
		}


		////

		public static Barrier[] GetWorldBarriers() {
			return BarrierManager.Instance.GetTileBarriers()
				.Values
				.ToArray();
		}


		////

		public static void DeclareWorldAccessBarrier( AccessBarrier barrier ) {
			BarrierManager.Instance.DeclareWorldBarrierUnsynced( barrier );

			if( Main.netMode == NetmodeID.Server ) {
				AccessBarrierCreatePacket.BroadcastToClients( barrier );
			}
		}

		////
		
		public static void DeclareWorldBarrierUnsynced( RectangularBarrier barrier ) {
			BarrierManager.Instance.DeclareWorldBarrierUnsynced( barrier );
		}

		////

		public static void RemoveWorldBarrier( Rectangle tileArea ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			BarrierManager.Instance.RemoveWorldBarrier( tileArea, true );
		}


		////////////////
		
		public static ISet<(int tileX, int tileY)> GetTilesUponBarrier( Barrier barrier, float padding ) {
			return barrier.GetTilesUponBarrier( padding );
		}



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}

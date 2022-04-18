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
			return BarrierManager.Instance.GetWorldBarriers()
				.Values
				.ToArray();
		}


		////

		public static void DeclareWorldBarrier( RectangularBarrier barrier, bool syncIfServer ) {
			BarrierManager.Instance.DeclareWorldBarrier_Unsynced( barrier );

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				WorldBarrierCreatePacket.BroadcastToClients( barrier );
			}
		}
		
		[Obsolete( "use `DeclareWorldBarrier`)", true )]
		public static void DeclareWorldAccessBarrier( AccessBarrier barrier ) {
			SoulBarriersAPI.DeclareWorldBarrier( barrier, true );
		}

		[Obsolete( "use `DeclareWorldBarrier`)", true )]
		public static void DeclareWorldBarrierUnsynced( RectangularBarrier barrier ) {
			BarrierManager.Instance.DeclareWorldBarrier_Unsynced( barrier );
		}

		////

		public static void RemoveWorldBarrier( Rectangle tileArea ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			BarrierManager.Instance.RemoveWorldBarrier( tileArea, true );
		}


		////////////////
		
		public static ISet<(int tileX, int tileY)> GetTilesUponBarrier( Barrier barrier, float worldPadding ) {
			return barrier.GetTilesUponBarrier( worldPadding );
		}



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}

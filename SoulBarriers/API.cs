﻿using System;
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

		public static void DeclareWorldAccessBarrier( AccessBarrier barrier, bool syncFromServer ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			BarrierManager.Instance.DeclareWorldBarrierUnsynced( barrier );

			//

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				AccessBarrierCreatePacket.BroadcastToClients( barrier );
			}
		}

		////
		
		public static void DeclareWorldBarrierUnsynced( RectangularBarrier barrier ) {
			BarrierManager.Instance.DeclareWorldBarrierUnsynced( barrier );
		}

		////

		public static void RemoveWorldBarrier( Rectangle worldArea ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not available for clients." );
			}

			BarrierManager.Instance.RemoveWorldBarrier( worldArea, true );
		}



		////////////////

		private IList<Action<Barrier>> BarrierCreateHooks = new List<Action<Barrier>>();

		private IList<Action<Barrier>> BarrierRemoveHooks = new List<Action<Barrier>>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager : ILoadable {
		public static BarrierManager Instance => ModContent.GetInstance<BarrierManager>();



		////////////////

		private IDictionary<int, Barrier> PlayerBarriers = new Dictionary<int, Barrier>();



		////////////////

		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}

		////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }



		////////////////

		public IDictionary<int, Barrier> GetPlayerBarriers() {
			return this.PlayerBarriers
				.ToDictionary( kv=>kv.Key, kv=>kv.Value );
		}


		////////////////

		internal void UpdateAllTrackedBarriers() {
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				Player plr = Main.player[plrWho];

				if( plr?.active != true ) {
					this.PlayerBarriers.Remove( plrWho );   // Garbage collection
				} else {
					this.PlayerBarriers[ plrWho ].UpdateForPlayer( plr );
				}
			}
		}

		////

		internal void TrackBarrier( Player hostPlayer, Barrier barrier ) {
			if( !this.PlayerBarriers.ContainsKey(hostPlayer.whoAmI) ) {
				this.PlayerBarriers[ hostPlayer.whoAmI ] = barrier;
			}
		}
	}
}
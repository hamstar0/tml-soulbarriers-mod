using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager {
		private IDictionary<int, Barrier> PlayerBarriers = new Dictionary<int, Barrier>();



		////////////////

		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}


		////////////////
		
		public IDictionary<int, Barrier> GetPlayerBarriers() {
			return this.PlayerBarriers
				.ToDictionary( kv=>kv.Key, kv=>kv.Value );
		}


		////////////////

		internal void UpdateAllBarriers() {
			// Garbage collection
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				if( Main.player[plrWho]?.active != true ) {
					this.PlayerBarriers.Remove( plrWho );
				}
			}
		}

		////

		internal void UpdateBarrier( Player hostPlayer, Barrier barrier ) {
			if( !this.PlayerBarriers.ContainsKey(hostPlayer.whoAmI) ) {
				this.PlayerBarriers[ hostPlayer.whoAmI ] = barrier;
			}
		}
	}
}
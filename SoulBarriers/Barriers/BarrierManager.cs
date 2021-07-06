using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager : ILoadable {
		public static BarrierManager Instance => ModContent.GetInstance<BarrierManager>();



		////////////////

		private IDictionary<int, Barrier> PlayerBarriers = new Dictionary<int, Barrier>();

		private IDictionary<Rectangle, Barrier> WorldBarriers = new Dictionary<Rectangle, Barrier>();



		////////////////

		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}

		public int GetWorldBarrierCount() {
			return this.WorldBarriers.Count();
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

		public IDictionary<Rectangle, Barrier> GetWorldBarriers() {
			return this.WorldBarriers
				.ToDictionary( kv=>kv.Key, kv=>kv.Value );
		}


		////////////////

		internal void UpdateAllTrackedBarriers() {
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				Player plr = Main.player[plrWho];

				if( plr?.active != true ) {
					this.PlayerBarriers.Remove( plrWho );   // Garbage collection
				} else {
					this.PlayerBarriers[ plrWho ].UpdateWithContext( plr );
				}
			}

			foreach( Rectangle rect in this.WorldBarriers.Keys.ToArray() ) {
				this.WorldBarriers[rect].UpdateWithContext( null );
			}
		}

		////

		internal void TrackPlayerBarrier( Player hostPlayer, Barrier barrier ) {
			if( !this.PlayerBarriers.ContainsKey(hostPlayer.whoAmI) ) {
				this.PlayerBarriers[ hostPlayer.whoAmI ] = barrier;
			}
		}
	}
}
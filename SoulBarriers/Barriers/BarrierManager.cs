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

		private IDictionary<string, Barrier> BarriersByID = new Dictionary<string, Barrier>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////
		
		internal void UpdateAllTrackedBarriers() {
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				Barrier barrier = this.PlayerBarriers[plrWho];
				Player plr = Main.player[plrWho];
				string id = barrier.GetID();

				if( plr?.active != true ) {
					this.PlayerBarriers.Remove( plrWho );   // Garbage collection

					this.BarriersByID.Remove( id );
				} else {
					barrier.Update_Internal();

					// New barrier found
					if( !this.BarriersByID.ContainsKey(id) ) {
						this.BarriersByID[id] = barrier;
					}
				}
			}

			foreach( Rectangle rect in this.WorldBarriers.Keys.ToArray() ) {
				Barrier barrier = this.WorldBarriers[ rect ];
				string id = barrier.GetID();

				barrier.Update_Internal();

				// New barrier found
				if( !this.BarriersByID.ContainsKey(id) ) {
					this.BarriersByID[id] = barrier;
				}
			}

			this.CheckCollisionsAgainstAllBarriers();
		}
	}
}
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

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		internal void UpdateAllTrackedBarriers() {
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				Barrier barrier = this.PlayerBarriers[plrWho];
				Player plr = Main.player[plrWho];

				if( plr?.active != true ) {
					this.PlayerBarriers.Remove( plrWho );   // Garbage collection
				} else {
					barrier.Update_Internal();
				}
			}

			foreach( Rectangle rect in this.WorldBarriers.Keys.ToArray() ) {
				Barrier barrier = this.WorldBarriers[rect];

				barrier.Update_Internal();
			}

			this.CheckCollisionsAgainstAllBarriers();
		}
	}
}
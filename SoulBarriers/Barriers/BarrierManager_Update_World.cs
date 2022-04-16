using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		private void UpdateTrackedBarrierOfWorld( Barrier barrier ) {
			barrier.Update_Internal();

			// New barrier found
			if( !this.BarriersByID.ContainsKey( barrier.ID ) ) {
				this.BarriersByID[ barrier.ID ] = barrier;

				LogLibraries.Warn( "New world barrier unexpectedly discovered: " + barrier.ID );
			}
		}
	}
}
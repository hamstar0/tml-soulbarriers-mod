using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		private int _FailsafeSyncTimer = 0;



		////////////////

		private void UpdatedTrackedBarrierOfEntity( Entity entity ) {
			if( this.GarbageCollectEntityBarrier_If(entity) ) {
				this.RemoveEntityBarrier( entity, true );

				return;
			}

			//

			Barrier barrier = this.GetBarrierOfEntity( entity );

			barrier.Update_Internal();

			//

			// New barrier found
			if( !this.BarriersByID.ContainsKey(barrier.ID) ) {
				this.BarriersByID[barrier.ID] = barrier;
			}

			//

			if( Main.netMode == NetmodeID.Server ) {
				if( this._FailsafeSyncTimer-- <= 0 ) {
					this._FailsafeSyncTimer = 60 * 30;

					BarrierStrengthPacket.SendToClient( -1, barrier, barrier.Strength, false, false );
				}
			}
		}


		////////////////

		private bool GarbageCollectEntityBarrier_If( Entity entity ) {
			if( entity?.active != true ) {
				return true;
			}

			//

			Barrier barrier = this.GetBarrierOfEntity( entity );
			Barrier entBarrier = null;

			if( entity is Player ) {
				entBarrier = ((Player)entity)
					.GetModPlayer<SoulBarriersPlayer>()
					.Barrier;
			} else if( entity is NPC ) {
				entBarrier = ((NPC)entity)
					.GetGlobalNPC<SoulBarriersNPC>()
					.Barrier;
			}

			return entBarrier != barrier;
		}
	}
}
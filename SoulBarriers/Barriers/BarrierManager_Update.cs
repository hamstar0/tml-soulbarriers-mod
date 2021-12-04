using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		internal void UpdateAllTrackedBarriers() {
			this.UpdateAllTrackedBarriersOfEachContext();

			//

			this.CheckCollisionsAgainstAllBarriers();

			//

			if( SoulBarriersConfig.Instance.DebugModeInfo ) {
				//DebugLibraries.Print( "player pos", Main.LocalPlayer.position.ToPoint().ToString() ) ;

				foreach( string id in this.BarriersByID.Keys ) {
					Barrier barrier = this.BarriersByID[id];
					double str = barrier.Strength;
					string maxStrStr = barrier.MaxRegenStrength.HasValue
						? ((int)barrier.MaxRegenStrength.Value).ToString()
						: "null";

					if( str > 0d ) {
						int dusts = barrier.ParticleOffsets.Keys.Count( d => d.active );
						int maxDusts = barrier.ComputeCappedNormalParticleCount();

						DebugLibraries.Print( "barrier:["+id+"]",
							"str:("+str+":"+maxStrStr+") - "
							+"dusts:"+dusts+" of "+maxDusts
						);
					}
				}
			}
		}

		private void UpdateAllTrackedBarriersOfEachContext() {
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				this.UpdatedTrackedBarrierOfEntity( Main.player[plrWho] );
			}

			foreach( int npcWho in this.NPCBarriers.Keys.ToArray() ) {
				this.UpdatedTrackedBarrierOfEntity( Main.npc[npcWho] );
			}

			foreach( Barrier barrier in this.TileBarriers.Values.ToArray() ) {
				this.UpdateTrackedBarrierOfWorld( barrier );
			}
		}

		
		////////////////

		 private int _FailsafeSyncTimer = 0;

		private void UpdatedTrackedBarrierOfEntity( Entity entity ) {
			if( entity?.active != true ) {
				this.RemoveEntityBarrier( entity, true );	// Garbage collection

				return;
			}

			//

			Barrier barrier = this.GetBarrierOfEntity( entity );

			barrier.Update_Internal();

			//

			string id = barrier.ID;

			// New barrier found
			if( !this.BarriersByID.ContainsKey(id) ) {
				this.BarriersByID[id] = barrier;
			}

			//

			if( Main.netMode == NetmodeID.Server ) {
				if( this._FailsafeSyncTimer-- <= 0 ) {
					this._FailsafeSyncTimer = 60 * 30;

					BarrierStrengthPacket.SendToClient( -1, barrier, barrier.Strength, false, false );
				}
			}
		}


		private void UpdateTrackedBarrierOfWorld( Barrier barrier ) {
			string id = barrier.ID;

			barrier.Update_Internal();

			// New barrier found
			if( !this.BarriersByID.ContainsKey(id) ) {
				this.BarriersByID[id] = barrier;
			}
		}
	}
}
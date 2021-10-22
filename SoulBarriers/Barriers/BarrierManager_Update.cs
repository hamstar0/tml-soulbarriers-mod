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
			foreach( int plrWho in this.PlayerBarriers.Keys.ToArray() ) {
				this.UpdatedTrackedBarrierOfPlayer( plrWho );
			}

			//

			foreach( Barrier barrier in this.TileBarriers.Values.ToArray() ) {
				this.UpdateTrackedBarrierOfWorld( barrier );
			}

			//

			this.CheckCollisionsAgainstAllBarriers();

			//

			if( SoulBarriersConfig.Instance.DebugModeInfo ) {
				DebugLibraries.Print( "player pos", Main.LocalPlayer.position.ToPoint().ToString() ) ;

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

		
		////////////////

		 private int _FailsafeSyncTimer = 0;

		private void UpdatedTrackedBarrierOfPlayer( int plrWho ) {
			Barrier barrier = this.PlayerBarriers[plrWho];
			Player plr = Main.player[plrWho];
			string id = barrier.GetID();

			//

			if( plr?.active != true ) {
				this.PlayerBarriers.Remove( plrWho );   // Garbage collection

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}

				return;
			}

			//

			barrier.Update_Internal();

			//

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
			string id = barrier.GetID();

			barrier.Update_Internal();

			// New barrier found
			if( !this.BarriersByID.ContainsKey(id) ) {
				this.BarriersByID[id] = barrier;
			}
		}
	}
}
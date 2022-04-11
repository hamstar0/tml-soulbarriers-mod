using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers.Packets {
	class BarrierRemovePacket : SimplePacketPayload {
		public static void BroadcastToClients( Barrier barrier ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new BarrierRemovePacket( barrier );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierID;



		////////////////

		private BarrierRemovePacket() { }

		private BarrierRemovePacket( Barrier barrier ) {
			this.BarrierID = barrier.ID;
		}

		////////////////

		public override void ReceiveOnClient() {
			var barrierMngr = BarrierManager.Instance;
			Barrier barrier = barrierMngr.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				return;
			}

			if( barrier is RectangularBarrier ) {
				barrierMngr.RemoveWorldBarrier( ((RectangularBarrier)barrier).TileArea, false );
			} else {    // if( barrier is SphericalBarrier )
				barrierMngr.RemoveNonWorldBarrier( barrier, false );
				//throw new NotImplementedException( "Removal of non-`RectangularBarrier`s not yet implemented." );
			}

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier removed: "+ barrier.ID );
			}
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server cannot be told to remove barriers." );
		}
	}
}

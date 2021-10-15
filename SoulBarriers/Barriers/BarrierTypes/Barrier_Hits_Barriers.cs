using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyBarrierCollisionHitIf( Barrier intruder, bool syncIfServer ) {
			if( this.OnPreBarrierBarrierCollision.All( f=>f.Invoke(intruder) ) ) {
				double thisBarrierStrength = this.Strength;
				double thatBarrierStrength = intruder.Strength;
				
//LogLibraries.Log( "B V B ApplyBarrierCollisionHitIf - "+this.GetType().Name+" - "+this.OnBarrierBarrierCollision.Count );
				foreach( BarrierBarrierCollisionHook e in this.OnBarrierBarrierCollision ) {
					e.Invoke( intruder );
				}
				
				this.ApplyBarrierCollisionHit( intruder, thisBarrierStrength, thatBarrierStrength, syncIfServer );
			}
		}


		////////////////

		private void ApplyBarrierCollisionHit(
					Barrier intruder,
					double prevBarrierStrength,
					double prevIntruderBarrierStrength,
					bool syncIfServer ) {
			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitBarrierPacket.BroadcastToClients(
					this,
					intruder,
					prevBarrierStrength,
					prevIntruderBarrierStrength
				);
			}
		}
	}
}
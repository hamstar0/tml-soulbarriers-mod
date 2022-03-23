using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public double ComputeCollisionDamage( Barrier otherBarrier ) {
			double damage = this.Strength > otherBarrier.Strength
				? otherBarrier.Strength
				: this.Strength;
			return Math.Ceiling( damage );
		}


		////////////////

		public bool ApplyBarrierCollisionHit_If(
					Barrier intruder,
					bool defaultCollisionAllowed,
					double damage,
					bool syncIfServer ) {
			bool isDefaultCollision = this.OnPreBarrierBarrierCollision
				.All( f => f.Invoke(intruder, ref damage) );
			bool canDefaultCollide = defaultCollisionAllowed && isDefaultCollision;

			//

			if( canDefaultCollide ) {
				if( damage > 0f ) {
					this.ApplyBarrierCollisionHit( intruder, damage, syncIfServer );
				}
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitBarrierPacket.BroadcastToClients(
					sourceBarrier: this,
					otherBarrier: intruder,
					defaultCollisionAllowed: canDefaultCollide,
					newSourceBarrierStrength: this.Strength,
					newOtherBarrierStrength: intruder.Strength
				);
			}

			//

			foreach( PostBarrierBarrierCollisionHook e in this.OnPostBarrierBarrierCollision ) {
				e.Invoke( intruder, canDefaultCollide, damage );
			}

			return isDefaultCollision;
		}


		////////////////

		private void ApplyBarrierCollisionHit( Barrier intruder, double damage, bool syncIfServer ) {
			if( damage > 0d ) {
				var toHitData = new BarrierHitContext( intruder, damage );
				var froHitData = new BarrierHitContext( this, damage );

				//

				this.ApplyRawHit( null, damage, false, toHitData );
				intruder.ApplyRawHit( null, damage, false, froHitData );
			}
		}
	}
}
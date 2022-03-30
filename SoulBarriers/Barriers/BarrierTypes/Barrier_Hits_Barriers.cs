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

		public void ApplyBarrierCollisionHit(
					Barrier intruder,
					bool defaultCollisionAllowed,
					bool syncIfServer ) {
			double damage = this.ComputeCollisionDamage( intruder );

			this.ApplyBarrierCollisionHit(
				intruder: intruder,
				defaultCollisionAllowed: defaultCollisionAllowed,
				damage: damage,
				syncIfServer: syncIfServer
			);
		}

		////

		public bool ApplyBarrierCollisionHit(
					Barrier intruder,
					bool defaultCollisionAllowed,
					double damage,
					bool syncIfServer ) {
			// Pre
			bool isDefaultCollision = this.OnPreBarrierBarrierHit
				.All( f => f.Invoke(intruder, ref damage) );

			//

			bool isDefaultCollisionHappening = defaultCollisionAllowed && isDefaultCollision;

			//
			
			if( isDefaultCollisionHappening ) {
				if( damage > 0f ) {
					this.ApplyBarrierCollisionDefaultHit( intruder, damage );
				}
			}

			//

			// Post
			foreach( PostBarrierBarrierHitHook e in this.OnPostBarrierBarrierHit ) {
				e.Invoke( intruder, isDefaultCollisionHappening, damage );
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitBarrierPacket.BroadcastToClients(
					sourceBarrier: this,
					otherBarrier: intruder,
					defaultCollisionAllowed: isDefaultCollisionHappening,
					newSourceBarrierStrength: this.Strength,
					newOtherBarrierStrength: intruder.Strength
				);
			}

			return isDefaultCollision;
		}


		////////////////

		private void ApplyBarrierCollisionDefaultHit( Barrier intruder, double damage ) {
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
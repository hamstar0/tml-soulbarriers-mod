using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyRawHit(
					Vector2? hitAt,
					double damage,
					bool syncIfServer,
					BarrierHitContext context ) {
			if( !this.OnPreBarrierRawHit.All( f=>f.Invoke(ref damage) ) ) {
				return;
			}

			//

			if( SoulBarriersConfig.Instance.DebugModeHitInfo ) {
				if( context == null ) {
					context = new BarrierHitContext( damage );
				}
				context.Output( this );
			}

			//

			double oldStr = this.Strength;

			this.SetStrength( this.Strength - damage, false, false, syncIfServer );

			//

			foreach( PostBarrierRawHitHook e in this.OnPostBarrierRawHit ) {
				e.Invoke( oldStr, damage );
			}

			//
			
			if( Main.netMode != NetmodeID.Server ) {
				if( hitAt.HasValue ) {
					this.ApplyHitFx( hitAt.Value, 0, 1f, damage, !this.IsActive );
				} else {
					this.ApplyHitFx( 0, 1f, damage, !this.IsActive );
				}
			}

			//

			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients(
					barrier: this,
					hasHitPosition: hitAt.HasValue,
					hitPosition: hitAt ?? default,
					damage: damage,
					context: context
				);
			}
		}
	}
}
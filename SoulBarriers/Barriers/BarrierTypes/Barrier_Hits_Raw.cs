using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyRawHit( Vector2? hitAt, double damage, bool syncFromServerOnly ) {
			if( syncFromServerOnly && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			//

			if( !this.OnPreBarrierRawHit.All( f=>f.Invoke(ref damage) ) ) {
				return;
			}

			//

			double oldStr = this.Strength;

			this.SetStrength( this.Strength - damage );

			//

			foreach( BarrierRawHitHook e in this.OnBarrierRawHit ) {
				e.Invoke( oldStr, damage );
			}

			//

			if( hitAt.HasValue ) {
				this.ApplyHitFx( hitAt.Value, damage );
			} else {
				this.ApplyHitFx( damage );
			}

			if( syncFromServerOnly && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients(
					barrier: this,
					hasHitPosition: hitAt.HasValue, hitAt ?? default,
					damage: damage
				);
			}
		}
	}
}
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyHitAgainstSelf( Vector2? hitAt, int damage, bool syncFromServerOnly ) {
			if( syncFromServerOnly && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( !this.OnPreBarrierRawHit.All( f=>f.Invoke(ref damage) ) ) {
				return;
			}

			int oldStr = this.Strength;

			this.SetStrength( this.Strength - damage );

			foreach( BarrierRawHitEvent e in this.OnBarrierRawHit ) {
				e.Invoke( oldStr, damage );
			}

			if( hitAt.HasValue ) {
				this.ApplyHitFx( hitAt.Value, damage );
			} else {
				this.ApplyHitFx( damage );
			}

			if( syncFromServerOnly && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients(
					this,
					hitAt.HasValue, hitAt ?? default,
					damage,
					-1
				);
			}
		}
	}
}
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyRawHit( Vector2? hitAt, int damage, bool syncFromServerOnly ) {
			if( syncFromServerOnly && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( !this.OnPreBarrierRawHit.All( f=>f.Invoke(ref damage) ) ) {
				return;
			}

			/*if( damage >= 1 && this.Strength >= 1 ) {
				this.Strength = 0;
			} else {
				this.Strength -= damage;

				// Saved from total destruction
				if( this.Strength <= 0 ) {
					this.Strength = 1;
				}
			}*/
			this.Strength -= damage;

			if( this.Strength < 0 ) {
				this.Strength = 0;
			}

			foreach( BarrierRawHitEvent e in this.OnBarrierRawHit ) {
				e.Invoke( damage );
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
using System;
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

			if( !this.OnPreBarrierRawHit?.Invoke(ref damage) ?? false ) {
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

			this.OnBarrierRawHit?.Invoke( damage );

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
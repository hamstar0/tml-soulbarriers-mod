using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyRawHit( Vector2 hitAt, int damage, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			if( !BarrierManager.Instance.OnPreBarrierRawHitEvent(this, ref damage) ) {
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

			BarrierManager.Instance.OnBarrierRawHitEvent( this, damage );

			this.ApplyHitFx( hitAt, damage );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitRawPacket.BroadcastToClients( this, hitAt, damage, -1 );
			}
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyMetaphysicalHit( Vector2? hitAt, double damage, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			this.SetStrength( this.Strength - damage );

			this.ApplyHitFx( (int)(damage * 4d) );

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitMetaphysicalPacket.BroadcastToClients(
					barrier: this,
					hasHitPosition: false,
					hitPosition: default,
					damage: damage
				);
			}
		}
	}
}
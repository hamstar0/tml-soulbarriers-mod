using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyMetaphysicalHit( Vector2? hitAt, double damage, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			this.SetStrength( this.Strength - damage, false, false );

			if( Main.netMode != NetmodeID.Server ) {
				if( damage > 0d ) {
					this.ApplyHitFx( 0, 4f, damage, !this.IsActive );
//LogLibraries.Log( "ApplyMetaphysicalHit "+damage+" ("+(int)(damage * 4d)+")" );
				}
			}

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				BarrierHitMetaphysicalPacket.BroadcastToClients(
					barrier: this,
					hasHitPosition: hitAt.HasValue,
					hitPosition: hitAt.HasValue ? hitAt.Value : default,
					damage: damage
				);
			}
		}
	}
}
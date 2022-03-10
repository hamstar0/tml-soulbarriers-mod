using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private void RefreshBuffForPlayerHost( bool syncs ) {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			Player plr = (Player)this.Host;

			if( this.Strength > 0d ) {
				plr.AddBuff( soulBuffType, 2, !syncs );
			} else {
				plr.ClearBuff( soulBuffType );
			}
		}

		private void RefreshBuffForNpcHost( bool syncs ) {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			NPC host = (NPC)this.Host;

			if( this.Strength > 0d ) {
				host.AddBuff( soulBuffType, 2, !syncs );
			} else {
				int buffIdx = host.FindBuffIndex( soulBuffType );
				if( buffIdx >= 0 ) {
					host.DelBuff( buffIdx );
				}
			}
		}
	}
}
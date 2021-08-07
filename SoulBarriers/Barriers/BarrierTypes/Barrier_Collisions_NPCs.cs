using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private bool CanCollideVsNpc( NPC intruder ) {
			switch( this.HostType ) {
			case BarrierHostType.None:
				return this.CanCollideWorldVsNpc( intruder );
			case BarrierHostType.Player:
				return this.CanCollidePlayerVsNpc( (Player)this.Host, intruder );
			case BarrierHostType.NPC:
				return this.CanCollideNpcVsNpc( (NPC)this.Host, intruder );
			}

			return false;
		}

		private bool CanCollideWorldVsNpc( NPC intruder ) {
			return !intruder.boss;
		}

		private bool CanCollidePlayerVsNpc( Player hostPlayer, NPC intruder ) {
			if( !hostPlayer.active || hostPlayer.dead /*|| hostPlayer.immune || intruder.playerImmune[hostPlayer.whoAmI] >= 1*/ ) {
				return false;
			}

			return !intruder.boss && !intruder.friendly;
		}

		private bool CanCollideNpcVsNpc( NPC hostNpc, NPC intruder ) {
			if( !hostNpc.active || hostNpc.immortal ) {
				return false;
			}

			return !intruder.boss && hostNpc.friendly != intruder.friendly;
		}
	}
}
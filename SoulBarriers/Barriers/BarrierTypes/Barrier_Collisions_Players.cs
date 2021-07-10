using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private bool CanCollideVsPlayer( Player player ) {
			switch( this.HostType ) {
			case BarrierHostType.None:
				return true;
			case BarrierHostType.Player:
				return this.CanCollidePlayerVsPlayer( (Player)this.Host, player );
			case BarrierHostType.NPC:
				return this.CanCollideNpcVsPlayer( (NPC)this.Host, player );
			}

			return false;
		}

		private bool CanCollidePlayerVsPlayer( Player hostPlayer, Player intruderPlayer ) {
			if( !hostPlayer.active || hostPlayer.dead ) {
				return false;
			}

			if( intruderPlayer?.active == true && intruderPlayer.hostile ) {    // player is pvp
				if( intruderPlayer.team == 0 || hostPlayer.team == 0 ) {
					return true;
				} else if( intruderPlayer.team != hostPlayer.team ) {
					return true;
				}
			}

			return false;
		}

		private bool CanCollideNpcVsPlayer( NPC hostNpc, Player intruderPlayer ) {
			if( !hostNpc.active ) {
				return false;
			}
			if( hostNpc.friendly ) {
				return false;
			}

			return true;
		}
	}
}
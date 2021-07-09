using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private bool CanCollideVsProjectile( Projectile proj ) {
			switch( this.HostType ) {
			case BarrierHostType.Player:
				return this.CanCollidePlayerVsProjectile( (Player)this.Host, proj );
			case BarrierHostType.NPC:
				return this.CanCollideNpcVsProjectile( (NPC)this.Host, proj );
			}

			return false;
		}

		private bool CanCollidePlayerVsProjectile( Player hostPlayer, Projectile proj ) {
			if( !hostPlayer.active || hostPlayer.dead /*|| hostPlayer.immune || proj.playerImmune[hostPlayer.whoAmI] >= 1*/ ) {
				return false;
			}

			if( proj.hostile ) {
				return true;
			}

			if( !proj.npcProj ) {    // player owned
				Player intruderPlayer = Main.player[proj.owner];

				if( intruderPlayer?.active == true && intruderPlayer.hostile ) {    // player is pvp
					if( intruderPlayer.team == 0 || hostPlayer.team == 0 ) {
						return true;
					} else if( intruderPlayer.team != hostPlayer.team ) {
						return true;
					}
				}
			}

			return false;
		}

		private bool CanCollideNpcVsProjectile( NPC hostNpc, Projectile proj ) {
			if( !hostNpc.active || hostNpc.immortal ) {
				return false;
			}

			if( proj.hostile ) {
				if( hostNpc.friendly ) {
					return true;
				}
			} else {
				if( !hostNpc.friendly ) {
					return true;
				}
			}

			return false;
		}
	}
}
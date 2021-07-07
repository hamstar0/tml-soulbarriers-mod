using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private bool CanHostedCollide( Entity intruder ) {
			if( this.Host == null ) {
				LogLibraries.WarnOnce( "Hostless barrier collision detected ("+this.HostType+", "+this.HostWhoAmI+")" );
				return false;
			}

			if( intruder is Projectile ) {
				return this.CanCollideHostedVsProjectile( (Projectile)intruder );
			} else if( intruder is Player ) {
				//return this.CanCollideHostedVsPlayer( (Player)intruder );
			} else if( intruder is NPC ) {
				//return this.CanCollideHostedVsNpc( (NPC)intruder );
			}

			return false;
		}

		////////////////

		private bool CanCollideHostedVsProjectile( Projectile proj ) {
			switch( this.HostType ) {
			case BarrierHostType.Player:
				return this.CanCollidePlayerHostedVsProjectile( (Player)this.Host, proj );
			case BarrierHostType.NPC:
				return this.CanCollideNpcHostedVsProjectile( (NPC)this.Host, proj );
			}

			return false;
		}

		private bool CanCollidePlayerHostedVsProjectile( Player hostPlayer, Projectile proj ) {
			if( !hostPlayer.active || hostPlayer.dead || hostPlayer.immune || proj.playerImmune[hostPlayer.whoAmI] >= 1 ) {
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

		private bool CanCollideNpcHostedVsProjectile( NPC hostNpc, Projectile proj ) {
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


		////////////////

		public abstract bool IsHostedCollidingDirectly( Entity intruder );
	}
}
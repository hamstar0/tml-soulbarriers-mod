using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public bool CanHostedCollide( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				return this.CanCollideHostedVsProjectile( host, (Projectile)intruder );
			} else if( intruder is Player ) {
				//return this.CanCollideHostedVsPlayer( host, (Player)intruder );
			} else if( intruder is NPC ) {
				//return this.CanCollideHostedVsNpc( host, (NPC)intruder );
			}

			return false;
		}

		////////////////

		private bool CanCollideHostedVsProjectile( Entity host, Projectile proj ) {
			if( host is Player ) {
				return this.CanCollidePlayerHostedVsProjectile( (Player)host, proj );
			} else if( host is NPC ) {
				return this.CanCollideNpcHostedVsProjectile( (NPC)host, proj );
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

		public bool IsHostedColliding( Entity host, Entity intruder ) {
			if( !this.CanHostedCollide( host, intruder ) ) {
				return false;
			}

			if( this.Strength <= 0 ) {
				return false;
			}

			return this.IsHostedCollidingDirectly( host, intruder );
		}

		////

		public abstract bool IsHostedCollidingDirectly( Entity host, Entity intruder );
	}
}
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.Config;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private bool CanCollideVsProjectile( Projectile intruder ) {
			var config = SoulBarriersConfig.Instance;
			var wl = config.Get<HashSet<ProjectileDefinition>>( nameof(config.BarrierProjectileWhitelist) );
			if( wl.Contains(new ProjectileDefinition(intruder.type)) ) {
				return false;
			}
			
			switch( this.HostType ) {
			case BarrierHostType.None:
				return this.CanCollideWorldVsProjectile( intruder );
			case BarrierHostType.Player:
				return this.CanCollidePlayerVsProjectile( (Player)this.Host, intruder );
			case BarrierHostType.NPC:
				return this.CanCollideNpcVsProjectile( (NPC)this.Host, intruder );
			}

			return false;
		}

		private bool CanCollideWorldVsProjectile( Projectile intruder ) {
			return true;
		}

		private bool CanCollidePlayerVsProjectile( Player hostPlayer, Projectile intruder ) {
			if( !hostPlayer.active || hostPlayer.dead /*|| hostPlayer.immune || intruder.playerImmune[hostPlayer.whoAmI] >= 1*/ ) {
				return false;
			}

			if( intruder.hostile ) {
				return true;
			}

			if( !intruder.npcProj ) {    // player owned
				Player intruderPlayer = Main.player[intruder.owner];

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

		private bool CanCollideNpcVsProjectile( NPC hostNpc, Projectile intruder ) {
			if( !hostNpc.active || hostNpc.immortal ) {
				return false;
			}

			if( intruder.hostile ) {
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
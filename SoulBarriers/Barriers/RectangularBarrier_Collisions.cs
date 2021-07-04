using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers {
	public partial class RectangularBarrier {
		public bool CanCollide( Entity host, Entity intruder ) {
			if( intruder is Projectile ) {
				return this.CanCollideVsProjectile( host, (Projectile)intruder );
			}

			return false;
		}

		////

		private bool CanCollideVsProjectile( Entity host, Projectile proj ) {
			if( host is Player ) {
				return this.CanCollidePlayerVsProjectile( (Player)host, proj );
			} else if( host is NPC ) {
				return this.CanCollideNpcVsProjectile( (NPC)host, proj );
			}

			return false;
		}

		private bool CanCollidePlayerVsProjectile( Player hostPlayer, Projectile proj ) {
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


		////////////////

		public bool IsColliding( Entity host, Entity intruder ) {
			if( !this.CanCollide(host, intruder) ) {
				return false;
			}

			if( this.Strength <= 0 ) {
				return false;
			}

			return this.IsCollidingEntities( host, intruder );
		}

		////

		public bool IsCollidingEntities( Entity host, Entity intruder ) {
			//bool intersects = host.GetRectangle()
			//	.Intersects( intruder.GetRectangle() );
			//if( intersects ) {
			//	return true;
			//}
			
			Vector2 origin = this.GetEntityBarrierOrigin( host );
			int leastDim = intruder.width < intruder.height
				? intruder.width
				: intruder.height;

			float dist = (origin - intruder.Center).Length() - (float)(leastDim/2);

//Main.NewText("3 "+((Projectile)intruder).Name+" "+intersects );
			return dist < this.Radius;
		}
	}
}
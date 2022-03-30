using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public virtual bool CanHitPlayer( Player intruder ) {
			return intruder.GetModPlayer<SoulBarriersPlayer>()
				.BarrierImmunityTimer <= 0;
		}


		public virtual bool CanHitNPC( NPC intruder ) {
			if( intruder.friendly ) {	// Townsfolk are fucking stupid and I can't be arsed to program a nanny
				return false;
			}
			if( intruder.realLife >= 1 ) {	// Hopefully eliminates some debugging woes
				return false;
			}
			if( Main.invasionSize >= 1 ) {	// Disabled against NPCs during invasions
				return false;
			}
			if( intruder.netID == NPCID.DungeonGuardian ) {	// Guess these guys are special
				return false;
			}
			if( Main.npc.Any(n => n?.active == true && n.boss) ) {	// Free boss grinder! /s
				return false;
			}
			return true;
		}


		public virtual bool CanHitProjectile( Projectile intruder ) {
			return true;
		}


		////////////////

		public void ApplyAccessPlayerHit( Player intruder ) {
			if( intruder.dead ) {
				return;
			}
			if( !this.CanHitPlayer(intruder) ) {
				return;
			}

			intruder.KillMe(
				damageSource: PlayerDeathReason.ByCustomReason( "Access denied." ),
				dmg: 999999999,
				hitDirection: 0
			);

			var myplayer = intruder.GetModPlayer<SoulBarriersPlayer>();
			myplayer.BarrierImmunityTimer = 60 * 2;

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.PlayerHealth, -1, -1, null, intruder.whoAmI );
			}
		}


		public void ApplyAccessNpcHit( NPC intruder ) {
			if( !this.CanHitNPC(intruder) ) {
				return;
			}

			var mynpc = intruder.GetGlobalNPC<SoulBarriersNPC>();
			mynpc.KillFromBarrier = true;
		}


		public void ApplyAccessProjectileHit( Projectile intruder ) {
			if( !this.CanHitProjectile(intruder) ) {
				return;
			}

			intruder.Kill();

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.KillProjectile, -1, -1, null, intruder.whoAmI );
			}
		}
	}
}
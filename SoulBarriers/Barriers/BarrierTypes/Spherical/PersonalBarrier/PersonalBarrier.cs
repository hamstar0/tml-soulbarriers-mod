using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Services.ProjectileOwner;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical.Personal {
	public partial class PersonalBarrier : SphericalBarrier {
		public PersonalBarrier(
					string id,
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					float strengthRegenPerTick,
					float radius,
					Color color
				) : base(
					id: id,
					hostType: hostType,
					hostWhoAmI: hostWhoAmI,
					strength: strength,
					maxRegenStrength: null,
					strengthRegenPerTick: strengthRegenPerTick,
					radius: radius,
					color: color ) {
			this.AddBarrierEntityCanCollideHook( this.MyPreBarrierEntityCollision );
			this.AddPreBarrierBarrierCollisionHook( this.MyPreBarrierBarrierCollision );
		}


		////////////////

		public override bool CanSave() {
			return false;
		}


		////////////////

		private bool MyPreBarrierEntityCollision( ref Entity intruder ) {
			switch( this.HostType ) {
			case BarrierHostType.Player:
				return this.MyPreBarrierEntityCollision_HostPlayer( ref intruder );
			case BarrierHostType.NPC:
				return this.MyPreBarrierEntityCollision_HostNPC( ref intruder );
			}
			return false;
		}

		////

		private bool MyPreBarrierEntityCollision_HostPlayer( ref Entity intruder ) {
			if( intruder is Player ) {
				return false;

			} else if( intruder is NPC ) {
				return NPCID.Sets.ProjectileNPC[( (NPC)intruder ).type];

			} else if( intruder is Projectile ) {
				Entity projOwner = ( (Projectile)intruder ).GetOwner();

				return projOwner is NPC
					|| (projOwner is Player && ((Player)projOwner).hostile)
					|| projOwner == null;
			}

			return false;
		}

		private bool MyPreBarrierEntityCollision_HostNPC( ref Entity intruder ) {
			if( intruder is Projectile ) {
				return ( (Projectile)intruder ).GetOwner() is Player;
			}

			return false;
		}

		////////////////

		private bool MyPreBarrierBarrierCollision( Barrier intruder, ref double damage ) {
			switch( this.HostType ) {
			case BarrierHostType.Player:
				if( intruder.HostType == BarrierHostType.Player ) {
					return ((Player)intruder.Host).hostile;
				} else if( intruder.HostType == BarrierHostType.NPC ) {
					return !((NPC)intruder.Host).friendly;
				}
				break;
			case BarrierHostType.NPC:
				if( intruder.HostType == BarrierHostType.Player ) {
					return ((NPC)intruder.Host).friendly;
				} else if( intruder.HostType == BarrierHostType.NPC ) {
					return ((NPC)this.Host).friendly != ((NPC)intruder.Host).friendly;
				}
				break;
			}

			return false;
		}
	}
}
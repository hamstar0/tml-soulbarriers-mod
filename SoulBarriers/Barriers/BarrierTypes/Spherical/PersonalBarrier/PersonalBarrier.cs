using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
			this.OnPreBarrierEntityCollision.Add( this.PreBarrierEntityCollision );
		}


		////////////////
		
		public override bool CanSave() {
			return false;
		}


		////////////////

		private bool PreBarrierEntityCollision( ref Entity intruder ) {
			switch( this.HostType ) {
			case BarrierHostType.Player:
				return this.PreBarrierEntityCollision_HostPlayer( ref intruder );
			case BarrierHostType.NPC:
				return this.PreBarrierEntityCollision_HostNPC( ref intruder );
			}
			return false;
		}

		////

		private bool PreBarrierEntityCollision_HostPlayer( ref Entity intruder ) {
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

		private bool PreBarrierEntityCollision_HostNPC( ref Entity intruder ) {
			if( intruder is Projectile ) {
				return ( (Projectile)intruder ).GetOwner() is Player;
			}

			return false;
		}
	}
}
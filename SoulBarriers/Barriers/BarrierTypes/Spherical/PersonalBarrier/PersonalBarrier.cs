using System;
using Microsoft.Xna.Framework;
using ModLibsCore.Services.ProjectileOwner;
using Terraria;
using Terraria.ID;


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
			bool onPreBarrierEntityCollision( ref Entity intruder ) {
				switch( hostType ) {
				case BarrierHostType.Player:
					if( intruder is Player ) {
						return false;
					} else if( intruder is NPC ) {
						return NPCID.Sets.ProjectileNPC[( (NPC)intruder ).type];
					} else if( intruder is Projectile ) {
						return ((Projectile)intruder).GetOwner() is NPC;
					}
					break;
				case BarrierHostType.NPC:
					if( intruder is Projectile ) {
						return ((Projectile)intruder).GetOwner() is Player;
					}
					break;
				}

				return false;
			}

			//

			this.OnPreBarrierEntityCollision.Add( onPreBarrierEntityCollision );
		}


		////////////////

		public override bool CanSave() {
			return false;
		}
	}
}
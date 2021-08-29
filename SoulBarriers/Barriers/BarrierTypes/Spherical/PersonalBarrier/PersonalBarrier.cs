using System;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical.Personal {
	public partial class PersonalBarrier : SphericalBarrier {
		public PersonalBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					float radius,
					BarrierColor color
				) : base(
					hostType: hostType,
					hostWhoAmI: hostWhoAmI,
					strength: strength,
					maxRegenStrength: null,
					strengthRegenPerTick: -1f / (60f * 3f), // Decays slowly (1 hp / 3s)
					radius: radius,
					color: color ) {
			bool onPreBarrierEntityCollision( ref Entity intruder ) {
				if( intruder is Player ) {
					return false;
				}
				if( intruder is NPC ) {
					return NPCID.Sets.ProjectileNPC[ ((NPC)intruder).type ];
				}
				return intruder is Projectile;
			}

			//

			this.OnPreBarrierEntityCollision.Add( onPreBarrierEntityCollision );

			//

			// Decays slowly (1 hp / 3s)
			this.StrengthRegenPerTick = -1d / (60d * 3d);
		}


		////////////////

		public override bool CanSave() {
			return false;
		}


		////////////////

		public override string GetID() {
			return (int)this.HostType+":"+this.HostWhoAmI;
		}
	}
}
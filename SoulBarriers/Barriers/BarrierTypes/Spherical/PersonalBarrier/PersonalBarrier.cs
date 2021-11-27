using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical.Personal {
	public partial class PersonalBarrier : SphericalBarrier {
		public PersonalBarrier(
					string id,
					BarrierHostType hostType,
					int hostWhoAmI,
					int strength,
					float radius,
					Color color
				) : base(
					id: id,
					hostType: hostType,
					hostWhoAmI: hostWhoAmI,
					strength: strength,
					maxRegenStrength: null,
					strengthRegenPerTick: 0,
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

			var config = SoulBarriersConfig.Instance;
			
			// Decays slowly (1 hp / 3s)
			this.StrengthRegenPerTick = config.Get<float>( nameof(config.PersonalBarrierDefaultDecayPercentPerTick) );
		}


		////////////////

		public override bool CanSave() {
			return false;
		}
	}
}
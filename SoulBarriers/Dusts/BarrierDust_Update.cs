using System;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Dusts {
	public partial class BarrierDust : ModDust {
		public override bool Update( Dust dust ) {
			(bool isBarrierHit, float percentDuration, float durationPercentPerTick, float baseScale) data
					= BarrierDust.GetCustomDataOrDefault( dust );
			if( data.percentDuration <= 0f ) {
				dust.active = false;
				return false;
			}

			data.percentDuration = data.percentDuration - data.durationPercentPerTick;
			dust.customData = data;

			//

			dust.position += dust.velocity;
			//dust.rotation += Main.rand.NextFloat();

			if( !data.isBarrierHit ) {
				dust.velocity.X *= 0.7f;

				if( dust.velocity.Y > 0f ) {
					dust.velocity.Y *= -1f;
				}
			}

			dust.scale = data.baseScale * data.percentDuration;

			return false;
		}
	}
}
using System;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Dusts {
	public partial class BarrierDust : ModDust {
		public override bool Update( Dust dust ) {
			SoulBarrierDustData data = BarrierDust.GetCustomDataOrDefault( dust );
			if( data.PercentDuration <= 0f ) {
				dust.active = false;
				return false;
			}

			data.PercentDuration = data.PercentDuration - data.DurationPercentPerTick;
			dust.customData = data;

			//

			dust.position += dust.velocity;
			//dust.rotation += Main.rand.NextFloat();

			if( !data.IsBarrierHit ) {
				dust.velocity.X *= 0.7f;

				if( dust.velocity.Y > 0f ) {
					dust.velocity.Y *= -1f;
				}
			}

			dust.scale = data.BaseScale * data.PercentDuration;

			return false;
		}
	}
}
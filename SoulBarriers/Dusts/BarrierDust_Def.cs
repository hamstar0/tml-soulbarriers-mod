using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Dusts {
	public partial class BarrierDust : ModDust {
		public const float DefaultPercentDurationElapsedPerTick = 1f / 32f;



		////////////////
		
		public static (bool isBarrierHit, float percentDuration, float durationPercentPerTick, float baseScale)
					GetCustomDataOrDefault( Dust dust ) {
			if( dust.customData is ValueTuple<bool, float, float, float> ) {
				return ((bool, float, float, float))dust.customData;
			}
			return (false, 0f, 0f, 0f);
		}

		public static void SetCustomData(
					Dust dust,
					bool isFromBarrierHit,
					float percentDuration,
					float durationPercentPerTick,
					float? baseScale = null ) {
			if( !baseScale.HasValue ) {
				var data = BarrierDust.GetCustomDataOrDefault( dust );
				baseScale = data.baseScale;
			}

			dust.customData = (isFromBarrierHit, percentDuration, durationPercentPerTick, baseScale.Value);
		}



		////////////////

		public override void OnSpawn( Dust dust ) {
			dust.noLight = true;
			//dust.fadeIn = 2f;

			dust.noGravity = true;
			dust.velocity /= 2f;
		}


		////////////////

		public override Color? GetAlpha( Dust dust, Color lightColor ) {
			/*float alphaScale = (float)dust.alpha / 255f;
			alphaScale *= alphaScale * Main.rand.NextFloat();

			return dust.color * (1f - alphaScale);*/
			return dust.color;
		}
	}
}
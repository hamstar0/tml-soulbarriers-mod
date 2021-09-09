using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Dusts {
	public struct SoulBarrierDustData {
		public Barrier Source;
		public bool IsBarrierHit;
		public float PercentDuration;
		public float DurationPercentPerTick;
		public float BaseScale;


		public SoulBarrierDustData(
					Barrier source,
					bool isBarrierHit,
					float percentDuration,
					float durationPercentPerTick,
					float baseScale ) {
			this.Source = source;
			this.IsBarrierHit = isBarrierHit;
			this.PercentDuration = percentDuration;
			this.DurationPercentPerTick = durationPercentPerTick;
			this.BaseScale = baseScale;
		}
	}




	public partial class BarrierDust : ModDust {
		public const float DefaultPercentDurationElapsedPerTick = 1f / 32f;



		////////////////
		
		public static SoulBarrierDustData GetCustomDataOrDefault( Dust dust ) {
			if( dust.customData is SoulBarrierDustData ) {
				return (SoulBarrierDustData)dust.customData;
			}
			return default;
		}

		public static void SetCustomData(
					Dust dust,
					Barrier source,
					bool isFromBarrierHit,
					float percentDuration,
					float durationPercentPerTick,
					float? baseScale = null ) {
			if( !baseScale.HasValue ) {
				SoulBarrierDustData data = BarrierDust.GetCustomDataOrDefault( dust );
				baseScale = data.BaseScale;
			}

			dust.customData = new SoulBarrierDustData(
				source: source,
				isBarrierHit: isFromBarrierHit,
				percentDuration: percentDuration,
				durationPercentPerTick: durationPercentPerTick,
				baseScale: baseScale.Value
			);
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
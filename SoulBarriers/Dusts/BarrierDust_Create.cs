using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Dusts {
	public partial class BarrierDust : ModDust {
		public static Color ComputeAltColor( Color c ) {
			if( c.R > c.G && c.R > c.B ) {
				if( c.G > c.B ) {
					c.G = c.R;
				} else {
					c.B = c.R;
				}
			} else if( c.G > c.B ) {
				if( c.R > c.B ) {
					c.R = c.G;
				} else {
					c.B = c.G;
				}
			} else {
				if( c.R > c.G ) {
					c.R = c.B;
				} else {
					c.G = c.B;
				}
			}
			return c;
		}


		////////////////

		public static Dust Create( Vector2 position, Color color, bool isHit ) {
			float scaleScale = Main.rand.NextFloat();
			float scale = isHit
				? 1.5f + (scaleScale * scaleScale)
				: 1f + (scaleScale * scaleScale * scaleScale * 2f);

			Vector2 vel = isHit
				? new Vector2( Main.rand.NextFloat(-3f,3f), Main.rand.NextFloat(-3f,3f) )
				: new Vector2( 0f, scale * -0.6f );

			Color altColor = BarrierDust.ComputeAltColor( color );
			float altColorLerp = Main.rand.NextFloat();
			altColorLerp *= altColorLerp;

			Dust dust = Dust.NewDustPerfect(
				Position: position,
				Velocity: vel,
				Type: ModContent.DustType<BarrierDust>(),
				Scale: scale,
				Alpha: 0,
				newColor: Color.Lerp( color, altColor, altColorLerp )
			);

			BarrierDust.SetCustomData(
				dust: dust,
				isFromBarrierHit: isHit,
				percentDuration: 1f - (Main.rand.NextFloat() * 0.2f),
				baseScale: scale
			);

			if( isHit ) {
				dust.rotation += Main.rand.NextFloat() * MathHelper.Pi;
			}

			return dust;
		}
	}
}
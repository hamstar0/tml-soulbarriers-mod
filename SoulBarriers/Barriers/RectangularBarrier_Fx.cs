using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class RectangularBarrier {
		public static Color GetColor( BarrierColor color ) {
			switch( color ) {
			case BarrierColor.Red:
				return Color.Red;
			case BarrierColor.Green:
				return Color.Lime;
			case BarrierColor.Purple:
				return Color.Purple;
			case BarrierColor.Yellow:
				return Color.Yellow;
			case BarrierColor.BigBlue:
				return Color.Blue;
			case BarrierColor.White:
			default:
				return Color.White;
			}
		}



		////////////////

		public int GetParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 8 + (this.Strength / 4);
		}


		////////////////

		public void AnimateAt( int maxParticles, Vector2 position, Vector2 velocity ) {
			int i = 0, j = 0;

			foreach( Dust dust in this.ParticleOffsets.Keys.ToArray() ) {
				if( j++ > 5 ) {
					break;
				}

				if( dust.active && Enum.IsDefined(typeof(BarrierColor), dust.type) ) {
					dust.position = position + this.ParticleOffsets[dust];
					dust.velocity = velocity;

					i++;
				} else {
					this.ParticleOffsets.Remove( dust );
				}
			}

			while( i < maxParticles ) {
				(Dust dust, Vector2 offset) = this.DrawShieldParticle( position, this.Area );
				this.ParticleOffsets[dust] = offset;

				i++;
			}
		}

		////////////////
		
		public (Dust dust, Vector2 offset) DrawShieldParticle( Vector2 position, Rectangle area ) {
			float distScale = Main.rand.NextFloat();
			distScale = 1f - (distScale * distScale * distScale * distScale * distScale);
			distScale *= radius;

			Vector2 offset = Vector2.One.RotatedByRandom( 2d * Math.PI );
			offset *= distScale;

			Dust dust = Dust.NewDustPerfect(
				Position: position + offset,
				Type: (int)this.BarrierColor,
				Scale: 2f / 3f
			);
			dust.noGravity = true;
			dust.noLight = true;

			return (dust, offset);
		}


		////////////////

		public void ApplyHitFx( Vector2 position, int dispersal, int hitStrength ) {
			int particles = 8 + (hitStrength / 4);
			particles *= 2;

			for( int i=0; i<particles; i++ ) {
				int dustIdx = Dust.NewDust(
					Position: position,
					Width: dispersal,
					Height: dispersal,
					Type: (int)this.BarrierColor,
					Scale: 2f
				);
				Main.dust[dustIdx].noGravity = true;
				Main.dust[dustIdx].noLight = true;
			}
		}
	}
}
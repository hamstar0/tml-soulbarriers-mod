using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public enum BarrierColor {
		Red = 60,
		Green = 61,
		Purple = 62,
		White = 63,
		Yellow = 64,
		BigBlue = 206
	}




	public partial class Barrier {
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
				(Dust dust, Vector2 offset) = this.DrawShieldParticle( position, this.Radius );
				this.ParticleOffsets[dust] = offset;

				i++;
			}
		}

		////////////////
		
		public (Dust dust, Vector2 offset) DrawShieldParticle( Vector2 position, float radius ) {
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
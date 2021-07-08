using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
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

		public static int GetHitParticleCount( int hitStrength ) {
			int particles = 8 + ( hitStrength / 4 );
			particles *= 2;

			return particles;
		}



		////////////////

		public int GetParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 8 + (this.Strength / 4);
		}


		////////////////

		public void Animate( int maxParticles ) {
			int i = 0, j = 0;
			Vector2 center = this.GetBarrierWorldCenter();

			foreach( Dust dust in this.ParticleOffsets.Keys.ToArray() ) {
				if( j++ > 5 ) {
					break;
				}

				if( dust.active && Enum.IsDefined(typeof(BarrierColor), dust.type) ) {
					dust.position = center + this.ParticleOffsets[ dust ];
					dust.velocity = this.Host?.velocity ?? dust.velocity;

					i++;
				} else {
					this.ParticleOffsets.Remove( dust );
				}
			}

			this.CreateBarrierParticlesForArea( center, maxParticles - i );
		}



		////////////////

		public void CreateBarrierParticlesForArea( Vector2 basePosition, int particles ) {
			for( int i=0; i<particles; i++ ) {
				(Dust dust, Vector2 offset) = this.CreateBarrierParticleForArea( basePosition );
				this.ParticleOffsets[ dust ] = offset;
			}
		}

		////

		public (Dust dust, Vector2 offset) CreateBarrierParticleForArea( Vector2 basePosition ) {
			Vector2 offset = this.GetRandomOffsetForArea();

			Dust dust = Dust.NewDustPerfect(
				Position: basePosition + offset,
				Type: (int)this.BarrierColor,
				Scale: 2f / 3f
			);
			dust.noGravity = true;
			dust.noLight = true;

			return (dust, offset);
		}


		////////////////

		public void CreateHitParticlesForArea( int particles, float dispersal ) {
			for( int i = 0; i < particles; i++ ) {
				(Dust dust, Vector2 offset) = this.CreateHitParticleForArea( dispersal );
				this.ParticleOffsets[dust] = offset;
			}
		}
		
		public void CreateHitParticlesAt( Vector2 position, int particles, float dispersal ) {
			Vector2 hitOffsetFromCenter = position - this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Dust dust = this.CreateHitParticle( position, dispersal );
				this.ParticleOffsets[ dust ] = hitOffsetFromCenter;
			}
		}

		////
		
		public (Dust dust, Vector2 offset) CreateHitParticleForArea( float dispersal ) {
			Vector2 offset = this.GetRandomOffsetForArea();
			Dust dust = this.CreateHitParticle( this.GetBarrierWorldCenter() + offset, dispersal );

			return (dust, offset);
		}
		
		public Dust CreateHitParticle( Vector2 position, float dispersal ) {
			float dispersalDir = dispersal * 0.5f;

			int dustIdx = Dust.NewDust(
				Position: position,
				Width: 0,
				Height: 0,
				SpeedX: dispersalDir > 0f ? Main.rand.NextFloat(-dispersalDir, dispersalDir) : 0f,
				SpeedY: dispersalDir > 0f ? Main.rand.NextFloat(-dispersalDir, dispersalDir) : 0f,
				Type: (int)this.BarrierColor,
				Scale: 2f
			);
			Main.dust[dustIdx].noGravity = true;
			Main.dust[dustIdx].noLight = true;

			return Main.dust[dustIdx];
		}
	}
}
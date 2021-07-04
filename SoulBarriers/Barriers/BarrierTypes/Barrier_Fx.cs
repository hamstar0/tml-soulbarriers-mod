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

		public void Animate( int maxParticles, Vector2 center, Vector2 velocity ) {
			int i = 0, j = 0;

			foreach( Dust dust in this.ParticleOffsets.Keys.ToArray() ) {
				if( j++ > 5 ) {
					break;
				}

				if( dust.active && Enum.IsDefined(typeof(BarrierColor), dust.type) ) {
					dust.position = center + this.ParticleOffsets[ dust ];
					dust.velocity = velocity;

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

		public abstract (Dust dust, Vector2 offset) CreateBarrierParticleForArea( Vector2 basePosition );

		////

		public Dust CreateBarrierParticle( Vector2 position ) {
			Dust dust = Dust.NewDustPerfect(
				Position: position,
				Type: (int)this.BarrierColor,
				Scale: 2f / 3f
			);
			dust.noGravity = true;
			dust.noLight = true;

			return dust;
		}


		////////////////

		public void CreateHitParticlesForArea( Vector2 basePosition, int hitStrength ) {
			int particles = Barrier.GetHitParticleCount( hitStrength );

			for( int i = 0; i < particles; i++ ) {
				(Dust dust, Vector2 offset) = this.CreateHitParticleForArea( basePosition );
				this.ParticleOffsets[dust] = offset;
			}
		}

		////
		
		public abstract (Dust dust, Vector2 offset) CreateHitParticleForArea( Vector2 basePosition );

		////

		public Dust CreateHitParticle( Vector2 position, int dispersal = 16 ) {
			int dustIdx = Dust.NewDust(
				Position: position,
				Width: dispersal,
				Height: dispersal,
				Type: (int)this.BarrierColor,
				Scale: 2f
			);
			Main.dust[dustIdx].noGravity = true;
			Main.dust[dustIdx].noLight = true;

			return Main.dust[dustIdx];
		}
	}
}
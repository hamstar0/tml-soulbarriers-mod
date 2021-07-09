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

		public void ApplyHitFx( int damage ) {
			int particles = Barrier.GetHitParticleCount( damage );

			this.CreateHitParticlesForArea( particles, 4f );
		}

		public void ApplyHitFx( Vector2 hitAt, int damage ) {
			int particles = Barrier.GetHitParticleCount( damage );

			this.CreateHitParticlesAt( hitAt, particles, 4f );
		}

		////////////////

		public virtual int GetParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 8 + (this.Strength / 4);
		}


		////////////////

		internal void Animate( int maxParticles ) {
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
				(Dust dust, Vector2 offset)? dustData = this.CreateBarrierParticleForArea( basePosition );
				if( !dustData.HasValue ) { continue; }

				this.ParticleOffsets[ dustData.Value.dust ] = dustData.Value.offset;
			}
		}

		////

		public (Dust dust, Vector2 offset)? CreateBarrierParticleForArea( Vector2 basePosition ) {
			Vector2? offset = this.GetRandomOffsetForArea( basePosition, true );
			if( !offset.HasValue ) {
				return null;
			}

			Dust dust = Dust.NewDustPerfect(
				Position: basePosition + offset.Value,
				Type: (int)this.BarrierColor,
				Scale: 2f / 3f
			);
			dust.noGravity = true;
			dust.noLight = true;

			return (dust, offset.Value);
		}


		////////////////

		public void CreateHitParticlesForArea( int particles, float dispersal ) {
			Vector2 pos = this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Vector2? offset = this.GetRandomOffsetForArea( pos, true );
				if( !offset.HasValue ) { continue; }

				Dust dust = this.CreateHitParticle( pos + offset.Value, dispersal );

				this.ParticleOffsets[dust] = offset.Value;
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
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


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

			Main.PlaySound( SoundID.Item10, this.GetBarrierWorldCenter() );
		}

		public void ApplyHitFx( Vector2 hitAt, int damage ) {
			int particles = Barrier.GetHitParticleCount( damage );

			this.CreateHitParticlesAt( hitAt, particles, 4f );

			Main.PlaySound( SoundID.Item10, hitAt );
		}

		////////////////

		public virtual int GetParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 16 + (this.Strength / 3);
		}


		////////////////

		internal void Animate( int maxParticles ) {
			Vector2 center = this.GetBarrierWorldCenter();

			foreach( Dust dust in this._ParticleOffsets.Keys.ToArray() ) {
				if( dust.active && Enum.IsDefined(typeof(BarrierColor), dust.type) ) {
					//dust.position = center + this._ParticleOffsets[ dust ];
					//dust.velocity = this.Host?.velocity ?? (dust.velocity * 0.99f);

					maxParticles--;
				} else {
					this._ParticleOffsets.Remove( dust );
				}
			}

			int particles = maxParticles;	//Math.Min( 5, maxParticles );
			this.CreateBarrierParticlesForArea( center, particles  );
		}


		////////////////

		public void CreateBarrierParticlesForArea( Vector2 worldCenterPos, int particles ) {
			for( int i=0; i<particles; i++ ) {
				(Dust dust, Vector2 offset)? dustData = this.CreateBarrierParticleForArea( worldCenterPos );
				if( !dustData.HasValue ) { continue; }

				this._ParticleOffsets[ dustData.Value.dust ] = dustData.Value.offset;
			}
		}

		////

		public (Dust dust, Vector2 offset)? CreateBarrierParticleForArea( Vector2 worldCenterPos ) {
			Vector2 offset = this.GetRandomOffsetForArea( worldCenterPos, true, out bool isFarAway );
			if( isFarAway ) {
				return null;
			}

			Dust dust = this.CreateBarrierParticleAt( worldCenterPos + offset );

			return (dust, offset);
		}

		////

		public virtual Dust CreateBarrierParticleAt( Vector2 position, Vector2? velocity = null, float scale = 2f / 3f ) {
			Dust dust;
			
			if( velocity.HasValue ) {
				dust = Dust.NewDustPerfect(
					Position: position,
					Velocity: velocity.Value,
					Type: (int)this.BarrierColor,
					Scale: scale
				);
			} else {
				dust = Dust.NewDustPerfect(
					Position: position,
					Type: (int)this.BarrierColor,
					Scale: scale
				);
			}
			dust.noGravity = true;
			dust.noLight = true;

			return dust;
		}


		////////////////

		public void CreateHitParticlesForArea( int particles, float dispersal ) {
			Vector2 pos = this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Vector2 offset = this.GetRandomOffsetForArea( pos, true, out bool isFarAway );
				if( isFarAway ) {
					continue;
				}

				Dust dust = this.CreateHitParticle( pos + offset, dispersal );

				this._ParticleOffsets[dust] = offset;
			}
		}
		
		public void CreateHitParticlesAt( Vector2 position, int particles, float dispersal ) {
			Vector2 hitOffsetFromCenter = position - this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Dust dust = this.CreateHitParticle( position, dispersal );
				this._ParticleOffsets[ dust ] = hitOffsetFromCenter;
			}
		}

		////
		
		public virtual Dust CreateHitParticle( Vector2 position, float dispersal, float scale = 2f ) {
			float dispersalDir = dispersal * 0.5f;

			int dustIdx = Dust.NewDust(
				Position: position,
				Width: 0,
				Height: 0,
				SpeedX: dispersalDir > 0f ? Main.rand.NextFloat(-dispersalDir, dispersalDir) : 0f,
				SpeedY: dispersalDir > 0f ? Main.rand.NextFloat(-dispersalDir, dispersalDir) : 0f,
				Type: (int)this.BarrierColor,
				Scale: scale
			);
			Main.dust[dustIdx].noGravity = true;
			Main.dust[dustIdx].noLight = true;

			return Main.dust[dustIdx];
		}
	}
}
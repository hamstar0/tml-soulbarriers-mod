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

		public static int GetHitParticleCount( double damage ) {
			if( damage <= 0d ) {
				return 0;
			}

			int particles = 8 + (int)(damage / 4d);
			particles *= 2;

			return particles;
		}



		////////////////

		public void ApplyHitFx( double damage ) {
			int particles = Barrier.GetHitParticleCount( damage );
			if( particles >= 1 ) {
				this.CreateHitParticlesForArea( particles, 4f );
			}

			if( damage != 0d ) {
				Vector2 pos = this.GetBarrierWorldCenter();

				if( damage > 0d ) {
					this.ApplyHitFx_Sound( pos );
				}
				this.ApplyHitFx_Text( pos, damage );
			}
		}

		public void ApplyHitFx( Vector2 hitAt, double damage ) {
			int particles = Barrier.GetHitParticleCount( damage );
			if( particles >= 1 ) {
				this.CreateHitParticlesAt( hitAt, particles, 4f );
			}
			
			if( damage != 0d ) {
				if( damage > 0d ) {
					this.ApplyHitFx_Sound( hitAt );
				}
				this.ApplyHitFx_Text( hitAt, damage );
			}
		}

		////

		private void ApplyHitFx_Sound( Vector2 hitAt ) {
			Main.PlaySound( SoundID.Item10, hitAt );
		}

		private void ApplyHitFx_Text( Vector2 hitAt, double damage ) {
			string fmtAmt = ((int)(damage * 100f)).ToString();

			Color color = Barrier.GetColor( this.BarrierColor );

			if( damage < 0f ) {
				fmtAmt = "+" + fmtAmt;
				color = Color.Lerp( color, Color.White, 0.25f );
			} else {
				fmtAmt = "-" + fmtAmt;
				color = Color.Lerp( color, Color.Black, 0.25f );
			}

			var area = new Rectangle( (int)hitAt.X, (int)hitAt.Y, 1, 1 );
			area.Y += 24;

			CombatText.NewText( area, color, fmtAmt, false, true );
		}


		////////////////

		public virtual int GetParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 16 + (int)(this.Strength / 3d);
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
			Vector2 offset = this.GetRandomOffsetWithinAreaForFx( worldCenterPos, true, out bool isFarAway );
			if( isFarAway ) {
				return null;
			}

			Dust dust = this.CreateBarrierParticleAt( worldCenterPos + offset );

			return (dust, offset);
		}

		////

		public virtual Dust CreateBarrierParticleAt( Vector2 position, Vector2? velocity = null, float scale = 1f ) {
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

			Entity host = this.Host;
			if( host != null ) {
				dust.velocity += host.velocity;
			}

			return dust;
		}


		////////////////

		public void CreateHitParticlesForArea( int particles, float dispersal ) {
			Vector2 pos = this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Vector2 offset = this.GetRandomOffsetWithinAreaForFx( pos, true, out bool isFarAway );
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
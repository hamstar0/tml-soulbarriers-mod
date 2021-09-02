using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public static int GetHitParticleCount( double maxParticles, double damage, double barrierStrength ) {
			if( damage <= 0d ) {
				return 0;
			}

			double percent = Math.Min( 1d, damage / barrierStrength );
			double minParticles = Math.Max( maxParticles / 8d, 6d );
			double addParticles = percent * ((7d * maxParticles) / 8d);

			return (int)(minParticles + addParticles);
		}



		////////////////

		public void ApplyHitFx( double damage ) {
			int particles = Barrier.GetHitParticleCount( this.GetMaxAnimationParticleCount(), damage, this.Strength );
			if( particles >= 1 ) {
				this.CreateHitParticlesForArea( particles );
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
			int particles = Barrier.GetHitParticleCount( this.GetMaxAnimationParticleCount(), damage, this.Strength );
			if( particles >= 1 ) {
				this.CreateHitParticlesAt( hitAt, particles );
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

			Color color = this.Color;
			
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

		public virtual int GetMaxAnimationParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 24 + (int)(this.Strength / 3d);
		}


		////////////////

		internal void Animate( int maxParticles ) {
			int dustType = ModContent.DustType<BarrierDust>();
			Vector2 center = this.GetBarrierWorldCenter();

			Entity host = null;
			switch( this.HostType ) {
			case BarrierHostType.NPC:
				host = Main.npc[this.HostWhoAmI];
				break;
			case BarrierHostType.Player:
				host = Main.player[this.HostWhoAmI];
				break;
			}
			
			foreach( Dust dust in this._ParticleOffsets.Keys.ToArray() ) {
				if( dust.active && dust.type == dustType ) {
					if( host != null ) {
						//dust.position += host.velocity;
						dust.position += host.position - host.oldPosition;
					}

					maxParticles--;
				} else {
					this._ParticleOffsets.Remove( dust );
				}
			}

			int particles = maxParticles / 4;
			this.CreateBarrierParticlesForArea( center, particles );
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

			Dust dust = this.CreateBarrierParticleAt( worldCenterPos + offset, false );

			return (dust, offset);
		}


		////////////////

		public void CreateHitParticlesForArea( int particles ) {
			Vector2 pos = this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Vector2 offset = this.GetRandomOffsetWithinAreaForFx( pos, true, out bool isFarAway );
				if( isFarAway ) {
					continue;
				}

				Dust dust = this.CreateBarrierParticleAt( pos + offset, true );

				this._ParticleOffsets[dust] = offset;
			}
		}
		
		public void CreateHitParticlesAt( Vector2 position, int particles ) {
			Vector2 hitOffsetFromCenter = position - this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Dust dust = this.CreateBarrierParticleAt( position, true );
				this._ParticleOffsets[ dust ] = hitOffsetFromCenter;
			}
		}


		////////////////

		public virtual Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create( position, this.Color, isHit, BarrierDust.DefaultPercentDurationElapsedPerTick );
		}
	}
}
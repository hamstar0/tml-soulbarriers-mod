using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public void ApplyHitFx( int minParticles, float particleQuantityScale, double damage, bool isCrit ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			int maxParticles = this.ComputeAreaHitParticleCountMax();

			int particles = Barrier.GetHitParticleCount(
				maxParticles: maxParticles,
				damage: isCrit ? maxParticles : damage,
				barrierStrength: this.Strength
			);

			particles = (int)((float)particles * particleQuantityScale);
			particles = (int)MathHelper.Clamp( particles, minParticles, maxParticles );

//Main.NewText( "HIT FX "+particles+" ("+maxParticles+") - "+this.ToString() );
			if( particles >= 1 ) {
				this.CreateHitParticlesForArea( particles );
			}

			Vector2 pos = this.GetBarrierWorldCenter();

			if( damage >= 1d ) {
				if( damage > 1d ) {
					this.ApplyHitFx_Sound( pos );
				}
				this.ApplyHitFx_Text( pos, damage );
			}
		}

		public void ApplyHitFx( Vector2 hitAt, int minParticles, float particleQuantityScale, double damage, bool isCrit ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			int maxParticles = this.ComputePointHitParticleCountMax();
			int particles = Barrier.GetHitParticleCount(
				maxParticles: maxParticles,
				damage: isCrit ? maxParticles : damage,
				barrierStrength: this.Strength
			);

			particles = (int)((float)particles * particleQuantityScale);
			particles = (int)MathHelper.Clamp( particles, minParticles, maxParticles );

			if( particles >= 1 ) {
				this.CreateHitParticlesAt( hitAt, particles );
			}
			
			if( Math.Abs(damage) > 1d ) {
				if( damage > 0d ) {
					this.ApplyHitFx_Sound( hitAt );
				}
				this.ApplyHitFx_Text( hitAt, damage );
			}
		}


		////////////////

		private void ApplyHitFx_Sound( Vector2 hitAt ) {
			Main.PlaySound( SoundID.Item10, hitAt );
		}

		private void ApplyHitFx_Text( Vector2 hitAt, double damage ) {
			string fmtAmt = ((int)damage).ToString();

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
	}
}
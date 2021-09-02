using System;
using Microsoft.Xna.Framework;
using Terraria;
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
					i--;
					continue;
				}

				this.CreateBarrierParticleAt( pos + offset, true );
			}
		}
		
		public void CreateHitParticlesAt( Vector2 position, int particles ) {
			for( int i = 0; i < particles; i++ ) {
				this.CreateBarrierParticleAt( position, true );
			}
		}


		////////////////

		public virtual Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create( position, this.Color, isHit, BarrierDust.DefaultPercentDurationElapsedPerTick );
		}
	}
}
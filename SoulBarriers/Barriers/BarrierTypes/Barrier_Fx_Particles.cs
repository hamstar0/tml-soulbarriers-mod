using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public static int GetHitParticleCount( double maxParticles, double damage, double barrierStrength ) {
			if( damage <= 1d ) {
				return 0;
			}

			double percentDmg = Math.Min( 1d, damage / barrierStrength );

			double min = maxParticles / 12d;
			double add = percentDmg * (11d * min);

			return (int)Math.Max( min + add, Barrier.MinimumAreaHitParticles );
		}



		////////////////

		public void CreateBarrierParticlesForArea( Vector2 worldCenterPos, int particles ) {
			for( int i=0; i<particles; i++ ) {
				(Dust dust, Vector2 offset)? dustData = this.CreateBarrierParticleForArea( worldCenterPos );
				if( !dustData.HasValue ) {
					continue;
				}

				this._ParticleOffsets[ dustData.Value.dust ] = dustData.Value.offset;
			}
		}


		////////////////

		public (Dust dust, Vector2 offset)? CreateBarrierParticleForArea( Vector2 worldCenterPos ) {
			Vector2? pos = this.GetRandomWorldPositionWithinAreaOnScreen( worldCenterPos, true, out bool isFar );
			if( !pos.HasValue || isFar ) {
				return null;
			}

			Dust dust = this.CreateBarrierParticleAt( pos.Value, false );

			return (dust, pos.Value);
		}


		////////////////

		public void CreateHitParticlesForArea( int particles ) {
			Vector2 wldPosCenter = this.GetBarrierWorldCenter();

			for( int i = 0; i < particles; i++ ) {
				Vector2? pos = this.GetRandomWorldPositionWithinAreaOnScreen( wldPosCenter, false, out _ );
				if( !pos.HasValue ) {
					break;
				}

				this.CreateBarrierParticleAt( pos.Value, true );
			}
		}
		
		public void CreateHitParticlesAt( Vector2 position, int particles ) {
			for( int i = 0; i < particles; i++ ) {
				this.CreateBarrierParticleAt( position, true );
			}
		}


		////////////////

		public virtual Dust CreateBarrierParticleAt( Vector2 position, bool isHit ) {
			return BarrierDust.Create(
				source: this,
				position: position,
				color: this.Color,
				isHit: isHit,
				durationPercentPerTick: isHit
					? BarrierDust.DefaultPercentDurationElapsedPerTick * 0.5f
					: BarrierDust.DefaultPercentDurationElapsedPerTick
			);
		}
	}
}
using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public const float ParticlesPerStrengthUnit = 1f / 3f;

		////
		
		public const int MinimumNormalParticles = 18;

		public const int MaximumNormalParticles = 300;

		public const int MinimumAreaHitParticles = 24;

		public const int MaximumAreaHitParticles = 200;



		////////////////

		public int ComputeCappedNormalParticleCount() {
			return Math.Min( Barrier.MaximumNormalParticles, this.ComputeNormalParticleCount() );
			//return this.ComputeNormalParticleCount();
		}

		////

		public int ComputeCappedAreaHitParticleCountMax() {
			return Math.Min( Barrier.MaximumAreaHitParticles, this.ComputeAreaHitParticleCountMax() );
			//return this.ComputeAreaHitParticleCountMax();
		}

		public int ComputeCappedPointHitParticleCountMax() {
			return Math.Min( Barrier.MaximumAreaHitParticles, this.ComputePointHitParticleCountMax() );
			//return this.ComputePointHitParticleCountMax();
		}

		////////////////

		public virtual int ComputeNormalParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			int particlesViaStr = (int)(this.Strength * Barrier.ParticlesPerStrengthUnit );

			return Math.Max( Barrier.MinimumNormalParticles, particlesViaStr );
		}

		////

		public virtual int ComputeAreaHitParticleCountMax() {
			double maxStr = !this.MaxRegenStrength.HasValue || this.MaxRegenStrength.Value <= 0d
				? this.InitialStrength
				: this.MaxRegenStrength.Value;
			
			int particlesViaStr = (int)(maxStr * Barrier.ParticlesPerStrengthUnit);

			return Math.Max( Barrier.MinimumAreaHitParticles, particlesViaStr );
		}

		public virtual int ComputePointHitParticleCountMax() {
			return this.ComputeAreaHitParticleCountMax();
		}
	}
}
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

		public virtual int ComputeNormalParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			int particlesViaStr = (int)(this.Strength * Barrier.ParticlesPerStrengthUnit );
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumNormalParticles );

			return Math.Max( Barrier.MinimumNormalParticles, maxParticles );
		}

		////

		public virtual int ComputeAreaHitParticleCountMax() {
			double maxStr = !this.MaxRegenStrength.HasValue || this.MaxRegenStrength.Value <= 0d
				? this.InitialStrength
				: this.MaxRegenStrength.Value;
			
			int particlesViaStr = (int)(maxStr * Barrier.ParticlesPerStrengthUnit);
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumAreaHitParticles );

			return Math.Max( Barrier.MinimumAreaHitParticles, maxParticles );
		}

		public virtual int ComputePointHitParticleCountMax() {
			return this.ComputeAreaHitParticleCountMax();
		}
	}
}
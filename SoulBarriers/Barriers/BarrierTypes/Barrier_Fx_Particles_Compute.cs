using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public virtual int ComputeNormalParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			int particlesViaStr = (int)(this.Strength / 3d);
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumNormalParticles );

			return Math.Max( Barrier.MinimumNormalParticles, maxParticles );
		}

		////

		public virtual int ComputeAreaHitParticleCountMax() {
			double maxStr = !this.MaxRegenStrength.HasValue || this.MaxRegenStrength.Value <= 0d
				? this.InitialStrength
				: this.MaxRegenStrength.Value;
			
			int particlesViaStr = (int)(maxStr / 3d);
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumHitParticles );

			return Math.Max( Barrier.MinimumHitParticles, maxParticles );
		}

		public virtual int ComputePointHitParticleCountMax() {
			return this.ComputeAreaHitParticleCountMax();
		}
	}
}
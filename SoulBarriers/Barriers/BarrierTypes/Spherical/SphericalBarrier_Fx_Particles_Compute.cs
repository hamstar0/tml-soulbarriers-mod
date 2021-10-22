using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		public override int ComputeNormalParticleCount() {
			var config = SoulBarriersConfig.Instance;
			float particleMulPerChunk = config.Get<float>( nameof(config.SphericalBarrierParticleMultiplier) );

			float particles = (float)base.ComputeNormalParticleCount();
			particles *= particleMulPerChunk;

			return Math.Min( Barrier.MaximumNormalParticles, (int)particles );
		}
	}
}
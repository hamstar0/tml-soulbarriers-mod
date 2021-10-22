using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public const int BarrierTileChunkSizePerAxis = 12;

		////

		public const float NormalParticlesMultipliedPerChunk = 4f;

		public const float AreaHitParticlesMultipliedPerChunk = 1f;



		////////////////

		public override int ComputeNormalParticleCount() {
			float chunkSize = RectangularBarrier.BarrierTileChunkSizePerAxis;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeNormalParticleCount();
			
			var config = SoulBarriersConfig.Instance;
			float particleMulPerChunk = config.Get<float>( nameof(config.RectangleBarrierParticleMultiplier) );

			int count = (int)( strengthBasedAmt * chunks * particleMulPerChunk );

			return Math.Min( count, 300 );
		}

		////
		
		public override int ComputeAreaHitParticleCountMax() {
			float chunkSize = RectangularBarrier.BarrierTileChunkSizePerAxis;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeAreaHitParticleCountMax();

			int count = (int)( strengthBasedAmt * chunks * RectangularBarrier.AreaHitParticlesMultipliedPerChunk );

			return Math.Min( count, 300 );
		}
	}
}
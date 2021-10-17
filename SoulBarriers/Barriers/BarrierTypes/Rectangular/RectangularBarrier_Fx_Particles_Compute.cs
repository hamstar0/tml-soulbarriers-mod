using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public const int BarrierAreaTileChunkSize = 12;



		////////////////

		public override int ComputeNormalParticleCount() {
			float chunkSize = RectangularBarrier.BarrierAreaTileChunkSize;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeNormalParticleCount();

			int count = (int)( strengthBasedAmt * chunks * 3f );

			return Math.Min( count, 300 );
		}

		////
		
		public override int ComputeAreaHitParticleCountMax() {
			float chunkSize = RectangularBarrier.BarrierAreaTileChunkSize;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeAreaHitParticleCountMax();

			int count = (int)( strengthBasedAmt * chunks * 1f );

			return Math.Min( count, 300 );
		}
	}
}
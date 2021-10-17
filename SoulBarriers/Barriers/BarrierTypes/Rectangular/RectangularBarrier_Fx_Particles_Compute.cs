using System;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int ComputeNormalParticleCount() {
			float chunkSize = 12f;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeNormalParticleCount();

			int count = (int)( strengthBasedAmt * chunks * 3f );

			return Math.Min( count, 300 );
		}

		
		public override int ComputeHitParticleCountMax() {
			float chunkSize = 12f;
			float chunksX = (float)this.TileArea.Width / chunkSize;
			float chunksY = (float)this.TileArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			float strengthBasedAmt = (float)base.ComputeHitParticleCountMax();

			int count = (int)( strengthBasedAmt * chunks * 1f );

			return Math.Min( count, 300 );
		}
	}
}
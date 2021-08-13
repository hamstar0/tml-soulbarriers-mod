using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular {
	public partial class RectangularBarrier : Barrier {
		public override int GetParticleCount() {
			float chunkSize = 12f * 16f;
			float chunksX = (float)this.WorldArea.Width / chunkSize;
			float chunksY = (float)this.WorldArea.Height / chunkSize;
			float chunks = chunksX * chunksY;

			int count = (int)((float)base.GetParticleCount() * (int)chunks * 2);

			return Math.Max( count, 100 );
		}


		////////////////

		public override Vector2 GetBarrierWorldCenter() {
			return this.WorldArea.Center.ToVector2();
		}

		public override Vector2 GetRandomOffsetForArea( Vector2 origin, bool isFxOnly, out bool isFarAway ) {
			var randOffset = new Vector2(
				Main.rand.Next( -this.WorldArea.Width/2, this.WorldArea.Width/2 ),
				Main.rand.Next( -this.WorldArea.Height/2, this.WorldArea.Height/2 )
			);

			if( isFxOnly ) {
				isFarAway = this.DecideIfParticleTooFarAway( randOffset );
			} else {
				isFarAway = false;
			}

			return randOffset;
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();


		////////////////

		public Rectangle Area { get; private set; }



		////////////////

		public RectangularBarrier( Rectangle area, BarrierColor color ) : base( color ) {
			this.Area = area;
		}
	}
}
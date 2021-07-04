using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class RectangularBarrier : Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();


		////////////////

		public int Strength { get; private set; } = 0;

		public Rectangle Area { get; private set; }

		public BarrierColor BarrierColor { get; private set; }



		////////////////

		public RectangularBarrier( Rectangle area, BarrierColor color ) {
			this.Area = area;
			this.BarrierColor = color;
		}


		////////////////

		public void SetStrength( int strength ) {
			if( strength < 0 ) {
				strength = 0;
			}
			this.Strength = strength;
		}
	}
}
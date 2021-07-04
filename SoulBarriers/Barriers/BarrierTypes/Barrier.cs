using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();


		////////////////

		public int Strength { get; private set; } = 0;

		public BarrierColor BarrierColor { get; private set; }



		////////////////

		public Barrier( BarrierColor color ) {
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
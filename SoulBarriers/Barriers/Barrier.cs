using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public partial class Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();

		private BarrierColor BarrierColor;


		////////////////

		public int Strength { get; private set; } = 0;

		public float Radius { get; private set; }



		////////////////

		public Barrier( float radius, BarrierColor color ) {
			this.Radius = radius;
			this.BarrierColor = color;
		}


		////////////////

		public void SetStrength( int strength ) {
			this.Strength = strength;
		}

		public void SetRadius( float radius ) {
			this.Radius = radius;
		}


		////////////////

		public Vector2 GetEntityBarrierOrigin( Entity host ) {
			if( host is Player ) {
				return ((Player)host).MountedCenter;
			} else {
				return host.Center;
			}
		}
	}
}
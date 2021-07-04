using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public static Vector2 GetEntityBarrierOrigin( Entity host ) {
			if( host is Player ) {
				return ( (Player)host ).MountedCenter;
			} else {
				return host.Center;
			}
		}



		////////////////

		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();


		////////////////

		public int Strength { get; protected set; } = 0;

		public BarrierColor BarrierColor { get; protected set; }



		////////////////

		public Barrier( BarrierColor color ) {
			this.BarrierColor = color;
		}
	}
}
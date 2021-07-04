using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace SoulBarriers.Barriers {
	public delegate bool EntityBarrierCollisionEvent( SpherericalBarrier barrier, Entity host, ref Entity intruder );

	public delegate bool AreaBarrierCollisionEvent( SpherericalBarrier barrier, Rectangle area, ref Entity intruder );
	
	public delegate bool BarrierRawHitEvent( SpherericalBarrier barrier, ref int damage );





	public partial class BarrierManager : ILoadable {
		public event EntityBarrierCollisionEvent OnEntityBarrierCollision;

		public event AreaBarrierCollisionEvent OnAreaBarrierCollision;

		public event BarrierRawHitEvent OnBarrierRawHit;



		////////////////

		internal bool OnEntityBarrierCollisionEvent( SpherericalBarrier barrier, Entity host, ref Entity intruder ) {
			return this.OnEntityBarrierCollision?.Invoke(barrier, host, ref intruder) ?? true;
		}

		internal bool OnAreaBarrierCollisionEvent( SpherericalBarrier barrier, Rectangle area, ref Entity intruder ) {
			return this.OnAreaBarrierCollision?.Invoke(barrier, area, ref intruder) ?? true;
		}

		internal bool OnBarrierRawHitEvent( SpherericalBarrier barrier, ref int damage ) {
			return this.OnBarrierRawHit?.Invoke(barrier, ref damage) ?? true;
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Classes.Loadable;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	public delegate bool EntityBarrierCollisionEvent( Barrier barrier, ref Entity intruder );

	public delegate bool AreaBarrierCollisionEvent( Barrier barrier, Rectangle area, ref Entity intruder );
	
	public delegate bool BarrierRawHitEvent( Barrier barrier, ref int damage );





	public partial class BarrierManager : ILoadable {
		public event EntityBarrierCollisionEvent OnEntityBarrierCollision;

		public event AreaBarrierCollisionEvent OnAreaBarrierCollision;

		public event BarrierRawHitEvent OnBarrierRawHit;



		////////////////

		internal bool OnEntityBarrierCollisionEvent( Barrier barrier, ref Entity intruder ) {
			return this.OnEntityBarrierCollision?.Invoke(barrier, ref intruder) ?? true;
		}

		internal bool OnAreaBarrierCollisionEvent( Barrier barrier, Rectangle area, ref Entity intruder ) {
			return this.OnAreaBarrierCollision?.Invoke(barrier, area, ref intruder) ?? true;
		}

		internal bool OnBarrierRawHitEvent( Barrier barrier, ref int damage ) {
			return this.OnBarrierRawHit?.Invoke(barrier, ref damage) ?? true;
		}
	}
}
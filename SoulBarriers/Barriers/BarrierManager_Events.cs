using System;
using Terraria;
using ModLibsCore.Classes.Loadable;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	public delegate bool PreBarrierEntityCollisionEvent( Barrier barrier, ref Entity intruder );

	public delegate void BarrierEntityCollisionEvent( Barrier barrier, Entity intruder );


	public delegate bool PreBarrierBarrierCollisionEvent( Barrier barrier, Barrier otherBarrier );

	public delegate void BarrierBarrierCollisionEvent( Barrier barrier, Barrier otherBarrier );

	
	public delegate bool PreBarrierRawHitEvent( Barrier barrier, ref int damage );
	
	public delegate void BarrierRawHitEvent( Barrier barrier, int damage );





	public partial class BarrierManager : ILoadable {
		public event PreBarrierEntityCollisionEvent OnPreBarrierEntityCollision;
		
		public event BarrierEntityCollisionEvent OnBarrierEntityCollision;


		public event PreBarrierBarrierCollisionEvent OnPreBarrierBarrierCollision;

		public event BarrierBarrierCollisionEvent OnBarrierBarrierCollision;


		public event PreBarrierRawHitEvent OnPreBarrierRawHit;

		public event BarrierRawHitEvent OnBarrierRawHit;



		////////////////

		internal bool OnPreBarrierEntityCollisionEvent( Barrier barrier, ref Entity intruder ) {
			return this.OnPreBarrierEntityCollision?.Invoke(barrier, ref intruder) ?? true;
		}

		internal void OnBarrierEntityCollisionEvent( Barrier barrier, Entity intruder ) {
			this.OnBarrierEntityCollision?.Invoke( barrier, intruder );
		}

		////
		
		internal bool OnPreBarrierBarrierCollisionEvent( Barrier barrier, Barrier otherBarrier ) {
			return this.OnPreBarrierBarrierCollision?.Invoke(barrier, otherBarrier) ?? true;
		}
		
		internal void OnBarrierBarrierCollisionEvent( Barrier barrier, Barrier otherBarrier ) {
			this.OnBarrierBarrierCollision?.Invoke( barrier, otherBarrier );
		}

		////

		internal bool OnPreBarrierRawHitEvent( Barrier barrier, ref int damage ) {
			return this.OnPreBarrierRawHit?.Invoke(barrier, ref damage) ?? true;
		}

		internal void OnBarrierRawHitEvent( Barrier barrier, int damage ) {
			this.OnBarrierRawHit?.Invoke( barrier, damage );
		}
	}
}
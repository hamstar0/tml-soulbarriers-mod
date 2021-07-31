using System;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool PreBarrierEntityCollisionEvent( ref Entity intruder );

	public delegate void BarrierEntityCollisionEvent( Entity intruder );


	public delegate bool PreBarrierBarrierCollisionEvent( Barrier otherBarrier );

	public delegate void BarrierBarrierCollisionEvent( Barrier otherBarrier );

	
	public delegate bool PreBarrierRawHitEvent( ref int damage );
	
	public delegate void BarrierRawHitEvent( int damage );




	public abstract partial class Barrier {
		public event PreBarrierEntityCollisionEvent OnPreBarrierEntityCollision;
		
		public event BarrierEntityCollisionEvent OnBarrierEntityCollision;


		public event PreBarrierBarrierCollisionEvent OnPreBarrierBarrierCollision;

		public event BarrierBarrierCollisionEvent OnBarrierBarrierCollision;


		public event PreBarrierRawHitEvent OnPreBarrierRawHit;

		public event BarrierRawHitEvent OnBarrierRawHit;
	}
}
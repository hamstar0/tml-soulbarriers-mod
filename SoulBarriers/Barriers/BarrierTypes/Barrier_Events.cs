using System;
using System.Collections.Generic;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool PreBarrierEntityCollisionEvent( ref Entity intruder );

	public delegate void BarrierEntityCollisionEvent( Entity intruder );


	public delegate bool PreBarrierBarrierCollisionEvent( Barrier otherBarrier );

	public delegate void BarrierBarrierCollisionEvent( Barrier otherBarrier );

	
	public delegate bool PreBarrierRawHitEvent( ref int damage );
	
	public delegate void BarrierRawHitEvent( int previousStrength, int attemptedDamage );




	public abstract partial class Barrier {
		internal ISet<PreBarrierEntityCollisionEvent> OnPreBarrierEntityCollision = new HashSet<PreBarrierEntityCollisionEvent>();

		internal ISet<BarrierEntityCollisionEvent> OnBarrierEntityCollision = new HashSet<BarrierEntityCollisionEvent>();


		internal ISet<PreBarrierBarrierCollisionEvent> OnPreBarrierBarrierCollision = new HashSet<PreBarrierBarrierCollisionEvent>();

		internal ISet<BarrierBarrierCollisionEvent> OnBarrierBarrierCollision = new HashSet<BarrierBarrierCollisionEvent>();


		internal ISet<PreBarrierRawHitEvent> OnPreBarrierRawHit = new HashSet<PreBarrierRawHitEvent>();

		internal ISet<BarrierRawHitEvent> OnBarrierRawHit = new HashSet<BarrierRawHitEvent>();
	}
}
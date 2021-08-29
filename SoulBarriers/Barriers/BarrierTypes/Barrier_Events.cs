using System;
using System.Collections.Generic;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool PreBarrierEntityCollisionEvent( ref Entity intruder );

	public delegate void BarrierEntityCollisionEvent( Entity intruder );


	public delegate bool PreBarrierBarrierCollisionEvent( Barrier otherBarrier );

	public delegate void BarrierBarrierCollisionEvent( Barrier otherBarrier );

	
	public delegate bool PreBarrierRawHitEvent( ref double damage );
	
	public delegate void BarrierRawHitEvent( double previousStrength, double attemptedDamage );




	public abstract partial class Barrier {
		protected ISet<PreBarrierEntityCollisionEvent> OnPreBarrierEntityCollision = new HashSet<PreBarrierEntityCollisionEvent>();

		protected ISet<BarrierEntityCollisionEvent> OnBarrierEntityCollision = new HashSet<BarrierEntityCollisionEvent>();


		protected ISet<PreBarrierBarrierCollisionEvent> OnPreBarrierBarrierCollision = new HashSet<PreBarrierBarrierCollisionEvent>();

		protected ISet<BarrierBarrierCollisionEvent> OnBarrierBarrierCollision = new HashSet<BarrierBarrierCollisionEvent>();


		protected ISet<PreBarrierRawHitEvent> OnPreBarrierRawHit = new HashSet<PreBarrierRawHitEvent>();

		protected ISet<BarrierRawHitEvent> OnBarrierRawHit = new HashSet<BarrierRawHitEvent>();
	}
}
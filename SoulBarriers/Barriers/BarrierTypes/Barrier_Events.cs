using System;
using System.Collections.Generic;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool PreBarrierEntityCollisionHook( ref Entity intruder );

	public delegate void BarrierEntityCollisionHook( Entity intruder );


	public delegate bool PreBarrierBarrierCollisionHook( Barrier otherBarrier );

	public delegate void BarrierBarrierCollisionHook( Barrier otherBarrier );

	
	public delegate bool PreBarrierRawHitHook( ref double damage );
	
	public delegate void BarrierRawHitHook( double previousStrength, double attemptedDamage );




	public abstract partial class Barrier {
		protected ISet<PreBarrierEntityCollisionHook> OnPreBarrierEntityCollision = new HashSet<PreBarrierEntityCollisionHook>();

		protected ISet<BarrierEntityCollisionHook> OnBarrierEntityCollision = new HashSet<BarrierEntityCollisionHook>();


		protected ISet<PreBarrierBarrierCollisionHook> OnPreBarrierBarrierCollision = new HashSet<PreBarrierBarrierCollisionHook>();

		protected ISet<BarrierBarrierCollisionHook> OnBarrierBarrierCollision = new HashSet<BarrierBarrierCollisionHook>();


		protected ISet<PreBarrierRawHitHook> OnPreBarrierRawHit = new HashSet<PreBarrierRawHitHook>();

		protected ISet<BarrierRawHitHook> OnBarrierRawHit = new HashSet<BarrierRawHitHook>();



		////////////////

		public void AddPreBarrierEntityCollisionHook( PreBarrierEntityCollisionHook hook ) {
			this.OnPreBarrierEntityCollision.Add( hook );
		}

		public void AddBarrierEntityCollisionHook( BarrierEntityCollisionHook hook ) {
			this.OnBarrierEntityCollision.Add( hook );
		}


		public void AddPreBarrierBarrierCollisionHook( PreBarrierBarrierCollisionHook hook ) {
			this.OnPreBarrierBarrierCollision.Add( hook );
		}

		public void AddBarrierBarrierCollisionHook( BarrierBarrierCollisionHook hook ) {
			this.OnBarrierBarrierCollision.Add( hook );
		}


		public void AddPreBarrierRawHitHook( PreBarrierRawHitHook hook ) {
			this.OnPreBarrierRawHit.Add( hook );
		}

		public void AddBarrierRawHit( BarrierRawHitHook hook ) {
			this.OnBarrierRawHit.Add( hook );
		}
	}
}
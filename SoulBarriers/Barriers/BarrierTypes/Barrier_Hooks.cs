using System;
using System.Collections.Generic;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool BarrierEntityCanCollideHook( ref Entity intruder );
	
	public delegate bool PreBarrierEntityCollisionHook( ref Entity intruder, ref double damage );

	public delegate void PostBarrierEntityCollisionHook( Entity intruder, bool isDefaultHit, double damage );


	public delegate bool PreBarrierBarrierCollisionHook( Barrier thatBarrier, ref double damage );

	public delegate void PostBarrierBarrierCollisionHook( Barrier otherBarrier, bool isDefaultHit, double damage );

	
	public delegate bool PreBarrierRawHitHook( ref double damage );
	
	public delegate void PostBarrierRawHitHook( double previousStrength, double attemptedDamage );




	public abstract partial class Barrier {
		protected ISet<BarrierEntityCanCollideHook> OnBarrierEntityCanCollide = new HashSet<BarrierEntityCanCollideHook>();
		
		protected ISet<PreBarrierEntityCollisionHook> OnPreBarrierEntityCollision = new HashSet<PreBarrierEntityCollisionHook>();

		protected ISet<PostBarrierEntityCollisionHook> OnPostBarrierEntityCollision = new HashSet<PostBarrierEntityCollisionHook>();


		protected ISet<PreBarrierBarrierCollisionHook> OnPreBarrierBarrierCollision = new HashSet<PreBarrierBarrierCollisionHook>();

		protected ISet<PostBarrierBarrierCollisionHook> OnPostBarrierBarrierCollision = new HashSet<PostBarrierBarrierCollisionHook>();


		protected ISet<PreBarrierRawHitHook> OnPreBarrierRawHit = new HashSet<PreBarrierRawHitHook>();

		protected ISet<PostBarrierRawHitHook> OnPostBarrierRawHit = new HashSet<PostBarrierRawHitHook>();



		////////////////

		public void AddBarrierEntityCanCollideHook( BarrierEntityCanCollideHook hook ) {
			this.OnBarrierEntityCanCollide.Add( hook );
		}

		public void AddPreBarrierEntityCollisionHook( PreBarrierEntityCollisionHook hook ) {
			this.OnPreBarrierEntityCollision.Add( hook );
		}

		public void AddPostBarrierEntityCollisionHook( PostBarrierEntityCollisionHook hook ) {
			this.OnPostBarrierEntityCollision.Add( hook );
		}


		public void AddPreBarrierBarrierCollisionHook( PreBarrierBarrierCollisionHook hook ) {
			this.OnPreBarrierBarrierCollision.Add( hook );
		}

		public void AddPostBarrierBarrierCollisionHook( PostBarrierBarrierCollisionHook hook ) {
			this.OnPostBarrierBarrierCollision.Add( hook );
		}


		public void AddPreBarrierRawHitHook( PreBarrierRawHitHook hook ) {
			this.OnPreBarrierRawHit.Add( hook );
		}

		public void AddPostBarrierRawHit( PostBarrierRawHitHook hook ) {
			this.OnPostBarrierRawHit.Add( hook );
		}
	}
}
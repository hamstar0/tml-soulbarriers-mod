using System;
using System.Collections.Generic;
using Terraria;


namespace SoulBarriers.Barriers.BarrierTypes {
	public delegate bool BarrierEntityCanCollideHook( ref Entity intruder );
	
	////

	public delegate bool PreBarrierEntityHitHook( ref Entity intruder, ref double damage );

	public delegate void PostBarrierEntityHitHook( Entity intruder, bool isDefaultHit, double damage );


	public delegate bool PreBarrierBarrierHitHook( Barrier thatBarrier, ref double damage );

	public delegate void PostBarrierBarrierHitHook( Barrier otherBarrier, bool isDefaultHit, double damage );


	////

	public delegate bool PreBarrierRawHitHook( ref double damage );
	
	public delegate void PostBarrierRawHitHook( double previousStrength, double attemptedDamage );




	public abstract partial class Barrier {
		protected ISet<BarrierEntityCanCollideHook> OnBarrierEntityCanCollide = new HashSet<BarrierEntityCanCollideHook>();

		////

		protected ISet<PreBarrierEntityHitHook> OnPreBarrierEntityHit = new HashSet<PreBarrierEntityHitHook>();

		protected ISet<PostBarrierEntityHitHook> OnPostBarrierEntityHit = new HashSet<PostBarrierEntityHitHook>();


		protected ISet<PreBarrierBarrierHitHook> OnPreBarrierBarrierHit = new HashSet<PreBarrierBarrierHitHook>();

		protected ISet<PostBarrierBarrierHitHook> OnPostBarrierBarrierHit = new HashSet<PostBarrierBarrierHitHook>();

		////

		protected ISet<PreBarrierRawHitHook> OnPreBarrierRawHit = new HashSet<PreBarrierRawHitHook>();

		protected ISet<PostBarrierRawHitHook> OnPostBarrierRawHit = new HashSet<PostBarrierRawHitHook>();



		////////////////

		public void AddBarrierEntityCanCollideHook( BarrierEntityCanCollideHook hook ) {
			this.OnBarrierEntityCanCollide.Add( hook );
		}

		public void AddPreBarrierEntityCollisionHook( PreBarrierEntityHitHook hook ) {
			this.OnPreBarrierEntityHit.Add( hook );
		}

		public void AddPostBarrierEntityCollisionHook( PostBarrierEntityHitHook hook ) {
			this.OnPostBarrierEntityHit.Add( hook );
		}


		public void AddPreBarrierBarrierCollisionHook( PreBarrierBarrierHitHook hook ) {
			this.OnPreBarrierBarrierHit.Add( hook );
		}

		public void AddPostBarrierBarrierCollisionHook( PostBarrierBarrierHitHook hook ) {
			this.OnPostBarrierBarrierHit.Add( hook );
		}


		public void AddPreBarrierRawHitHook( PreBarrierRawHitHook hook ) {
			this.OnPreBarrierRawHit.Add( hook );
		}

		public void AddPostBarrierRawHit( PostBarrierRawHitHook hook ) {
			this.OnPostBarrierRawHit.Add( hook );
		}
	}
}
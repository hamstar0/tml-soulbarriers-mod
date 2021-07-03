using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace SoulBarriers.Barriers {
	public partial class Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();

		private BarrierColor BarrierColor;


		////////////////

		public int Strength { get; private set; } = 0;

		public float Radius { get; private set; }



		////////////////

		public Barrier( float radius, BarrierColor color ) {
			this.Radius = radius;
			this.BarrierColor = color;
		}


		////////////////

		public void SetStrength( int strength ) {
			this.Strength = strength;
		}

		public void SetRadius( float radius ) {
			this.Radius = radius;
		}


		////////////////

		public Vector2 GetEntityBarrierOrigin( Entity host ) {
			if( host is Player ) {
				return ((Player)host).MountedCenter;
			} else {
				return host.Center;
			}
		}


		////////////////

		internal void Update( Entity host ) {
			if( host is Player ) {
				this.UpdateForPlayer( (Player)host );
			}
		}

		private void UpdateForPlayer( Player playerHost ) {
			var config = SoulBarriersConfig.Instance;
			int maxBuffs = playerHost.buffType.Length;

			for( int i=0; i<maxBuffs; i++ ) {
				switch( playerHost.buffType[i] ) {
				case BuffID.Cursed:
				case BuffID.Silenced:
				case BuffID.Stoned:
					playerHost.ClearBuff( playerHost.buffType[i] );

					int dmg = config.Get<int>( nameof(config.BarrierDebuffRemovalCost) );
					this.Strength -= dmg;

					this.ApplyHitFx( this.GetEntityBarrierOrigin(playerHost), (int)this.Radius, dmg * 4 );
					break;
				}
			}
		}
	}
}
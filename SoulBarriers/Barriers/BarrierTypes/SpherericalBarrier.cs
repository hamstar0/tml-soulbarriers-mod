using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SpherericalBarrier : Barrier {
		private IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();


		////////////////

		public int Strength { get; private set; } = 0;

		public float Radius { get; private set; }

		public BarrierColor BarrierColor { get; private set; }



		////////////////

		public SpherericalBarrier( float radius, BarrierColor color ) {
			this.Radius = radius;
			this.BarrierColor = color;
		}


		////////////////

		public void SetStrength( Player hostPlayer, int strength ) {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();

			if( strength < 0 ) {
				strength = 0;
			}

			if( strength >= 1 ) {
				hostPlayer.AddBuff( soulBuffType, 2 );
			} else {
				hostPlayer.ClearBuff( soulBuffType );
			}

			this.Strength = strength;
		}

		public void SetStrength( NPC hostNpc, int strength ) {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();

			if( strength < 0 ) {
				strength = 0;
			}

			if( strength >= 1 ) {
				hostNpc.AddBuff( soulBuffType, 2 );
			} else {
				int buffIdx = hostNpc.FindBuffIndex( soulBuffType );
				if( buffIdx >= 0 ) {
					hostNpc.DelBuff( buffIdx );
				}
			}

			this.Strength = strength;
		}

		////

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
	}
}
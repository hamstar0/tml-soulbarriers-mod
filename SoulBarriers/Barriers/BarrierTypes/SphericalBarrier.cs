using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		public float Radius { get; private set; }



		////////////////

		public SphericalBarrier( float radius, BarrierColor color ) : base( color ) {
			this.Radius = radius;
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

		public override Vector2 GetRandomOffsetForArea() {
			float distScale = Main.rand.NextFloat();
			distScale = 1f - ( distScale * distScale * distScale * distScale * distScale );
			distScale *= this.Radius;

			Vector2 offset = Vector2.One.RotatedByRandom( 2d * Math.PI );
			offset *= distScale;

			return offset;
		}
	}
}
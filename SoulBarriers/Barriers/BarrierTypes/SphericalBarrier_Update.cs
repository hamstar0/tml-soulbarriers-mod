using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		internal void UpdateForPlayer( Player hostPlayer ) {
			if( this.Strength <= 0 ) {
				return;
			}

			int maxBuffs = hostPlayer.buffType.Length;

			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			var badBuffIdxs = new List<int>();
			bool hasSoulBuff = false;

			for( int i=0; i<maxBuffs; i++ ) {
				if( hostPlayer.buffTime[i] <= 0 ) {
					continue;
				}

				int buffType = hostPlayer.buffType[i];

				switch( buffType ) {
				case BuffID.Cursed:
				case BuffID.Silenced:
				case BuffID.Stoned:
					badBuffIdxs.Add( i );
					break;
				default:
					hasSoulBuff = hasSoulBuff || buffType == soulBuffType;
					break;
				}
			}

			if( !hasSoulBuff ) {
				this.SetStrength( hostPlayer, 0 );
			} else {
				foreach( int debuffIdx in badBuffIdxs ) {
					this.ApplyDebuffHit( hostPlayer, debuffIdx );
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public partial class SphericalBarrier : Barrier {
		internal override void UpdateWithContext( Entity host ) {
			if( host == null || !(host is Player) || this.Strength <= 0 ) {
				return;
			}

			Player hostPlayer = host as Player;

			this.UpdateForPlayerForBuffs( hostPlayer, out bool hasSoulBuff );

			if( !hasSoulBuff ) {
				this.SetStrength( hostPlayer, 0 );

				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					BarrierStrengthPacket.SyncFromClientToServer( hostPlayer );
				}
			}
		}


		private void UpdateForPlayerForBuffs( Player hostPlayer, out bool hasSoulBuff ) {
			hasSoulBuff = false;

			int maxBuffs = hostPlayer.buffType.Length;
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			var badBuffIdxs = new List<int>();

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

			if( hasSoulBuff ) {
				foreach( int debuffIdx in badBuffIdxs ) {
					this.ApplyDebuffHit( hostPlayer, debuffIdx, true );
				}
			}
		}
	}
}
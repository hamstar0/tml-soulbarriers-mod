using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Buffs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical {
	public partial class SphericalBarrier : Barrier {
		protected override void Update() {
			Entity host = this.Host;
			if( host == null || !(host is Player) || this.Strength <= 0 ) {
				return;
			}

			Player hostPlayer = host as Player;

			this.UpdateForPlayerForBuffs( hostPlayer, out bool hasSoulBuff );

			if( !hasSoulBuff ) {
				this.SetStrength( 0 );

				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					BarrierStrengthPacket.SyncFromClientToServer( this, 0, true );
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
					this.ApplyPlayerDebuffHit( hostPlayer.buffType[debuffIdx], true );
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Buffs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical.Personal {
	public partial class PersonalBarrier : SphericalBarrier {
		protected override void Update() {
			Entity host = this.Host;
			if( host == null || !(host is Player) || this.Strength <= 0d ) {
				return;
			}

			this.UpdateForPlayerForBuffs( (Player)host, out bool hasSoulBuff );

			if( !hasSoulBuff ) {
				this.SetStrength( 0, true, false );

				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					BarrierStrengthPacket.SyncToServerForEveryone( this, 0, false, true );
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
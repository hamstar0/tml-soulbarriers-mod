using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;
using SoulBarriers.Buffs;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers.BarrierTypes.Spherical.Personal {
	public partial class PersonalBarrier : SphericalBarrier {
		protected override void Update() {
			if( this.Strength <= 0d ) {
				return;
			}

			Entity host = this.Host;
			if( host != null ) {
				if( host is Player ) {
					this.UpdateForPlayer( this.Host as Player );
				} else if( host is NPC ) {
					this.UpdateForNPC( this.Host as NPC );
				}
			}
		}


		////////////////

		private void UpdateForPlayer( Player plr ) {
			if( plr.dead ) {
				this.SetStrength( 0, true, true, false );	// TODO: Confirm no sync

				return;
			}

			//

			this.UpdateForBiomes( plr, out _ );
			this.UpdateForPlayerForBuffs( plr, out bool hasSoulBuff );

			if( !hasSoulBuff ) {
				this.SetStrength( 0, true, false, false );

				//

				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					if( plr.whoAmI == Main.myPlayer ) {
						BarrierStrengthPacket.SyncToServerToEveryone_Local( this, 0, false, true );
					}
				}
			}
		}

		private void UpdateForNPC( NPC npc ) {
			if( npc.life <= 0 || !npc.active ) {
				this.SetStrength( 0, true, true, false );

				return;
			}
		}


		////////////////

		protected virtual void UpdateForBiomes( Player hostPlayer, out bool isDefault ) {
			var config = SoulBarriersConfig.Instance;
			int tileY = (int)hostPlayer.position.Y / 16;

			isDefault = false;
			
			if( hostPlayer.ZoneJungle ) {
				this.StrengthRegenPerTick = config.Get<float>( nameof(config.PBGBarrierJungleRegenPercentPerTick) );
			} else if( tileY >= WorldLocationLibraries.UnderworldLayerTopTileY ) {
				this.StrengthRegenPerTick = config.Get<float>( nameof(config.PBGBarrierUnderworldRegenPercentPerTick) );
			} else {
				// Decays slowly (1 hp / 3s)
				this.StrengthRegenPerTick = config.Get<float>( nameof(config.PBGBarrierDefaultRegenPercentPerTick) );

				isDefault = true;
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
					if( !this.ApplyPlayerDebuffHit_If(hostPlayer.buffType[debuffIdx], true) ) {
						LogLibraries.Alert( "Personal barrier failed to remove player debuff" );
					}
				}
			}
		}
	}
}
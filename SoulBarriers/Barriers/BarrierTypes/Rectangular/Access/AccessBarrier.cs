using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public AccessBarrier(
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					Rectangle worldArea,
					BarrierColor color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base(
					strength,
					maxRegenStrength,
					strengthRegenPerTick,
					worldArea,
					color,
					isSaveable,
					hostType,
					hostWhoAmI ) {
			void onBarrierEntityCollide( Entity intruder ) {
//DebugLibraries.ChatOnce( "b_col_ent_"+this.GetID(), "ent: "+intruder );
				if( intruder is Player ) {
					var plrIntrud = intruder as Player;

					if( !plrIntrud.dead ) {
						plrIntrud.KillMe(
							damageSource: PlayerDeathReason.ByCustomReason( "Access denied." ),
							dmg: 999999999,
							hitDirection: 0
						);
					}
				} else if( intruder is NPC ) {
					var npcIntrud = intruder as NPC;

					NPCLibraries.Kill( npcIntrud, Main.netMode == NetmodeID.Server );
				} else if( intruder is Projectile ) {
					var projIntrud = intruder as Projectile;

					projIntrud.Kill();
				}
			}
			
			void onBarrierBarrierCollide( Barrier otherBarrier ) {
				if( otherBarrier is AccessBarrier ) {
					return;
				}

				int damage = this.Strength > otherBarrier.Strength
					? otherBarrier.Strength
					: this.Strength;

				if( damage > 0 ) {
					this.ApplyRawHit( null, damage, true );
					otherBarrier.ApplyRawHit( null, damage, true );
				}
			}

			//

			//this.OnPreBarrierEntityCollision += ( ref Entity intruder ) => true;
			this.OnBarrierEntityCollision.Add( onBarrierEntityCollide );
			this.OnBarrierBarrierCollision.Add( onBarrierBarrierCollide );
		}
	}
}
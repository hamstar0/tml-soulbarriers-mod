using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
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
			this.OnPreBarrierEntityCollision += ( ref Entity intruder ) => true;
			
			this.OnBarrierEntityCollision += ( Entity intruder ) => {
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
			};

			this.OnBarrierBarrierCollision += ( Barrier otherBarrier ) => {
				int damage = this.Strength > otherBarrier.Strength
					? otherBarrier.Strength
					: this.Strength;

				if( damage > 0 ) {
					this.ApplyRawHit( null, damage, true );
					otherBarrier.ApplyRawHit( null, damage, true );
				}
			};
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public AccessBarrier(
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle worldArea,
					BarrierColor color,
					bool isSaveable,
					BarrierHostType hostType = BarrierHostType.None,
					int hostWhoAmI = -1
				) : base(
					strength: strength,
					maxRegenStrength: maxRegenStrength,
					strengthRegenPerTick: strengthRegenPerTick,
					worldArea: worldArea,
					color: color,
					isSaveable: isSaveable,
					hostType: hostType,
					hostWhoAmI: hostWhoAmI ) {
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
					var intruderNpc = intruder as NPC;

					if( !intruderNpc.friendly && !intruderNpc.boss && intruderNpc.realLife == -1 ) {
						var mynpc = intruderNpc.GetGlobalNPC<SoulBarriersNPC>();
						mynpc.KillFromBarrier = true;
					}
				} else if( intruder is Projectile ) {
					var projIntrud = intruder as Projectile;

					projIntrud.Kill();
				}
			}
			
			void onBarrierBarrierCollide( Barrier otherBarrier ) {
				if( otherBarrier is AccessBarrier ) {
					return;
				}

				double damage = this.Strength > otherBarrier.Strength
					? otherBarrier.Strength
					: this.Strength;
				damage = Math.Round( damage );

				if( damage > 0d ) {
					this.ApplyRawHit( null, damage, false );
					otherBarrier.ApplyRawHit( null, damage, false );
				}
			}

			//

			//this.OnPreBarrierEntityCollision += ( ref Entity intruder ) => true;
			this.OnBarrierEntityCollision.Add( onBarrierEntityCollide );
			this.OnBarrierBarrierCollision.Add( onBarrierBarrierCollide );
		}
	}
}
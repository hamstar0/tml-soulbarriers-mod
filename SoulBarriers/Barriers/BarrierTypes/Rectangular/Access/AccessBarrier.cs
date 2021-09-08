using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes.Rectangular.Access {
	public partial class AccessBarrier : RectangularBarrier {
		public AccessBarrier(
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					Rectangle worldArea,
					Color color,
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
				if( !intruder.active ) {
					return;
				}

//DebugLibraries.ChatOnce( "b_col_ent_"+this.GetID(), "ent: "+intruder );
				if( intruder is Player ) {
					this.ApplyAccessPlayerHit( intruder as Player );
				} else if( intruder is NPC ) {
					this.ApplyAccessNpcHit( intruder as NPC );
				} else if( intruder is Projectile ) {
					this.ApplyAccessProjectileHit( intruder as Projectile );
				}
			}
			
			void onBarrierBarrierCollide( Barrier otherBarrier ) {
				if( !otherBarrier.IsActive || otherBarrier is AccessBarrier ) {
					return;
				}

				double damage = this.Strength > otherBarrier.Strength
					? otherBarrier.Strength
					: this.Strength;
				damage = Math.Ceiling( damage );

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


		////

		public void ApplyAccessPlayerHit( Player intruder ) {
			if( intruder.dead ) {
				return;
			}

			intruder.KillMe(
				damageSource: PlayerDeathReason.ByCustomReason( "Access denied." ),
				dmg: 999999999,
				hitDirection: 0
			);

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.PlayerHealth, -1, -1, null, intruder.whoAmI );
			}
		}

		public void ApplyAccessNpcHit( NPC intruder ) {
			if( intruder.friendly ) {
				return;
			}
			if( intruder.boss ) {
				return;
			}
			if( intruder.realLife >= 1 ) {
				return;
			}

			var mynpc = intruder.GetGlobalNPC<SoulBarriersNPC>();
			mynpc.KillFromBarrier = true;
		}

		public void ApplyAccessProjectileHit( Projectile intruder ) {
			intruder.Kill();

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.KillProjectile, -1, -1, null, intruder.whoAmI );
			}
		}
	}
}
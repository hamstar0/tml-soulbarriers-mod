using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;

namespace SoulBarriers.Barriers {
	public class BarrierHitContext {
		public double Damage { get; private set; }

		public object DamageSource { get; private set; } = null;



		////////////////

		public BarrierHitContext( double damage ) {
			this.Damage = damage;
		}

		public BarrierHitContext( object source, double damage ) {
			this.DamageSource = source;
			this.Damage = damage;
		}


		////////////////

		public string SourceToString() {
			string src;

			if( this.DamageSource is SphericalBarrier ) {
				src = "SB_"+this.DamageSource.GetType().Name[0];
			} else if( this.DamageSource is RectangularBarrier ) {
				src = "RB_"+this.DamageSource.GetType().Name[0];
			} else if( this.DamageSource is NPC ) {
				NPC npc = (NPC)this.DamageSource;
				src = "NPC_"+ npc.netID;
			} else if( this.DamageSource is Item ) {
				Item item = (Item)this.DamageSource;
				src = "Item_"+item.type+"_"+item.whoAmI;
			} else if( this.DamageSource is Projectile ) {
				Projectile proj = (Projectile)this.DamageSource;
				src = "Proj_"+proj.type+"_"+proj.whoAmI;
			} else if( this.DamageSource is Player ) {
				Player plr = (Player)this.DamageSource;
				src = "Plr_"+plr.whoAmI;
			} else {
				src = "Sync";
			}

			return src;
		}

		public override string ToString() {
			return this.SourceToString() + "." + this.Damage;
		}


		////////////////

		public void Output( Barrier damageRecipient ) {
			string output = damageRecipient.ToString() + " " + this.ToString();

			LogLibraries.Log( output );

			if( Main.netMode != NetmodeID.Server ) {
				Main.NewText( output );
			}
		}
	}
}
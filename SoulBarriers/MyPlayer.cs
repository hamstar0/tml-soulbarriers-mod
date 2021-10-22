using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers.BarrierTypes;
using ModLibsCore.Services.Timers;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		public static string GetGracePeriodTimerNAme( Player player ) {
			return "SoulBarriers_PlayerSpawnGracePeriod_" + player.whoAmI;
		}



		////////////////

		public Barrier Barrier { get; private set; }


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			string timerName = SoulBarriersPlayer.GetGracePeriodTimerNAme( this.player );

			Timers.SetTimer( timerName, 60 * 3, false, () => false );
		}


		////////////////

		public override void PreUpdate() {
			this.UpdatePersonalBarrier();
		}


		////////////////

		public override void DrawEffects(
					PlayerDrawInfo drawInfo,
					ref float r,
					ref float g,
					ref float b,
					ref float a,
					ref bool fullBright ) {
			if( this.Barrier != null && this.Barrier.IsActive ) {
				int particles = this.Barrier.ComputeCappedNormalParticleCount();

				this.Barrier.Animate( particles );
			}
		}
	}
}
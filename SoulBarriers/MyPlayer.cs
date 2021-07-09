using System;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	partial class SoulBarriersPlayer : ModPlayer {
		public Barrier Barrier { get; private set; }


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void DrawEffects(
					PlayerDrawInfo drawInfo,
					ref float r,
					ref float g,
					ref float b,
					ref float a,
					ref bool fullBright ) {
			if( this.Barrier.Strength >= 1 ) {
				int particles = this.Barrier.GetParticleCount();

				this.Barrier.Animate( particles );
			}
		}


		////////////////

		public override void PreUpdate() {
			this.UpdatePersonalBarrier();

			BarrierManager.Instance.CheckCollisionsAgainstEntity( this.player );
		}
	}
}
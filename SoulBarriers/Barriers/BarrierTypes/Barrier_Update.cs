using System;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private float BufferedStrengthRegen = 0f;



		////////////////

		internal void Update_Internal() {
			this.UpdateRegen();
			this.Update();
		}

		private void UpdateRegen() {
			if( this.Strength <= 0 ) {
				return;
			}
			if( this.Strength >= this.MaxRegenStrength ) {
				return;
			}

			this.UpdateRegenBuffered();
		}

		private void UpdateRegenBuffered() {
			this.BufferedStrengthRegen += this.StrengthRegenPerTick;

			this.Strength += (int)this.BufferedStrengthRegen;
			this.BufferedStrengthRegen -= (int)this.BufferedStrengthRegen;

			if( this.Strength > this.MaxRegenStrength ) {
				this.Strength = this.MaxRegenStrength;
			}
		}


		////////////////

		protected abstract void Update();
	}
}
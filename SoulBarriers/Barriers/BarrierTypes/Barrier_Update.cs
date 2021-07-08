using System;


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

			this.BufferedStrengthRegen += this.StrengthRegenPerTick;

			while( this.BufferedStrengthRegen >= 1f ) {
				this.BufferedStrengthRegen -= 1f;

				if( this.Strength < this.MaxRegenStrength ) {
					this.Strength++;
				}
			}
		}

		////

		protected abstract void Update();
	}
}
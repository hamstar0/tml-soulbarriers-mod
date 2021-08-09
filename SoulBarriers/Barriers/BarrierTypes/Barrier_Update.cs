using System;
using Terraria;
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

			this.UpdateRegenBuffered();
		}

		private void UpdateRegenBuffered() {
			if( this.Strength >= this.MaxRegenStrength ) {
				this.BufferedStrengthRegen = 0f;

				return;
			}

			if( ((double)this.BufferedStrengthRegen + (double)this.StrengthRegenPerTick) > (double)Int32.MaxValue ) {
				this.BufferedStrengthRegen = Int32.MaxValue;
			} else {
				this.BufferedStrengthRegen += this.StrengthRegenPerTick;
			}

			if( ((double)this.Strength + (double)this.BufferedStrengthRegen) > (double)Int32.MaxValue ) {
				this.Strength = Int32.MaxValue;
				this.BufferedStrengthRegen = 0f;
			} else {
				int wholeNumberBufferedRegen = (int)this.BufferedStrengthRegen;

				this.Strength += wholeNumberBufferedRegen;
				this.BufferedStrengthRegen -= wholeNumberBufferedRegen;
			}

			if( this.Strength > this.MaxRegenStrength ) {
				this.Strength = this.MaxRegenStrength;
			}
		}


		////////////////

		protected virtual void Update() { }
	}
}
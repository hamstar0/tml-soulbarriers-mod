using System;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		private double BufferedStrengthRegen = 0d;



		////////////////

		internal void Update_Internal() {
			this.UpdateRegen();
			this.Update();
		}

		private void UpdateRegen() {
			if( this.Strength > 0d ) {
				this.UpdateRegenBuffered();
			}
		}

		private void UpdateRegenBuffered() {
			if( this.MaxRegenStrength.HasValue
						&& this.Strength >= this.MaxRegenStrength
						&& this.StrengthRegenPerTick >= 0d ) {
				this.BufferedStrengthRegen = 0d;

				return;
			}

			//

			double nextRegen = this.BufferedStrengthRegen + this.StrengthRegenPerTick;
			if( nextRegen > (double)Int32.MaxValue ) {
				this.BufferedStrengthRegen = Int32.MaxValue;
			} else {
				this.BufferedStrengthRegen += this.StrengthRegenPerTick;
			}

			double nextStrength = this.Strength + this.BufferedStrengthRegen;
			if( nextStrength > (double)Int32.MaxValue ) {
				this.Strength = (double)Int32.MaxValue;
				this.BufferedStrengthRegen = 0d;
			} else {
				double wholeNumberBufferedRegen = this.BufferedStrengthRegen;

				this.Strength += wholeNumberBufferedRegen;
				this.BufferedStrengthRegen -= wholeNumberBufferedRegen;
			}
			
			if( this.MaxRegenStrength.HasValue ) {
				if( this.Strength > this.MaxRegenStrength.Value ) {
					this.Strength = this.MaxRegenStrength.Value;
				}
			}
		}


		////////////////

		protected virtual void Update() { }
	}
}
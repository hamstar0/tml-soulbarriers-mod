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
			if( this.Strength >= 1 ) {
				this.UpdateRegenBuffered();
			}
		}

		private void UpdateRegenBuffered() {
			if( this.MaxRegenStrength.HasValue
						&& this.Strength >= this.MaxRegenStrength
						&& this.StrengthRegenPerTick >= 0f ) {
				this.BufferedStrengthRegen = 0f;

				return;
			}

			//

			double nextRegen = (double)this.BufferedStrengthRegen + (double)this.StrengthRegenPerTick;
			if( nextRegen > (double)Int32.MaxValue ) {
				this.BufferedStrengthRegen = Int32.MaxValue;
			} else {
				this.BufferedStrengthRegen += this.StrengthRegenPerTick;
			}

			double nextStrength = (double)this.Strength + (double)this.BufferedStrengthRegen;
			if( nextStrength > (double)Int32.MaxValue ) {
				this.Strength = Int32.MaxValue;
				this.BufferedStrengthRegen = 0f;
			} else {
				int wholeNumberBufferedRegen = (int)this.BufferedStrengthRegen;

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
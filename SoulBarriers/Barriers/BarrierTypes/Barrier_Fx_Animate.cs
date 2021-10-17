using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		internal void Animate( int maxParticles ) {
			Vector2 center = this.GetBarrierWorldCenter();

			Entity host = null;
			switch( this.HostType ) {
			case BarrierHostType.NPC:
				host = Main.npc[this.HostWhoAmI];
				break;
			case BarrierHostType.Player:
				host = Main.player[this.HostWhoAmI];
				break;
			}
			
			foreach( Dust dust in this._ParticleOffsets.Keys.ToArray() ) {
				if( this.UpdateBarrierDust(dust, host) ) {
					maxParticles--;
				}
			}

			int particles = maxParticles / 4;
			this.CreateBarrierParticlesForArea( center, particles );
		}


		////////////////
		
		private bool UpdateBarrierDust( Dust dust, Entity host ) {
			if( !dust.active ) {
				this._ParticleOffsets.Remove( dust );

				return false;
			}

			SoulBarrierDustData dustData = BarrierDust.GetCustomDataOrDefault( dust );
			if( dustData.Source != this ) {
				this._ParticleOffsets.Remove( dust );

				return false;
			}

			if( host != null ) {
				dust.position += host.position - host.oldPosition;
			}

			return true;
		}


		////////////////

		public virtual int ComputeNormalParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			int particlesViaStr = (int)(this.Strength / 3d);
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumNormalParticles );

			return Math.Max( Barrier.MinimumNormalParticles, maxParticles );
		}

		public virtual int ComputeHitParticleCountMax() {
			double maxStr = !this.MaxRegenStrength.HasValue || this.MaxRegenStrength.Value <= 0d
				? this.InitialStrength
				: this.MaxRegenStrength.Value;
			
			int particlesViaStr = (int)(maxStr / 3d);
			int maxParticles = Math.Min( particlesViaStr, Barrier.MaximumHitParticles );

			return Math.Max( Barrier.MinimumHitParticles, maxParticles );
		}
	}
}
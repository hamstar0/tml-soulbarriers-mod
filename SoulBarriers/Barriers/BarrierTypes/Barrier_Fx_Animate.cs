using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SoulBarriers.Dusts;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		internal void Animate( int maxParticles ) {
			int dustType = ModContent.DustType<BarrierDust>();
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
				if( !dust.active || dust.type != dustType ) {
					this._ParticleOffsets.Remove( dust );

					continue;
				}

				SoulBarrierDustData dustData = BarrierDust.GetCustomDataOrDefault( dust );
				if( dustData.Source != this ) {
					this._ParticleOffsets.Remove( dust );

					continue;
				}

				if( host != null ) {
					//dust.position += host.velocity;
					dust.position += host.position - host.oldPosition;
				}

				maxParticles--;
			}

			int particles = maxParticles / 4;
			this.CreateBarrierParticlesForArea( center, particles );
		}


		////////////////

		public virtual int ComputeCurrentMaxAnimatedParticleCount() {
			if( this.Strength <= 0 ) {
				return 0;
			}

			return 24 + (int)( this.Strength / 3d );
		}

		public virtual int ComputeMaxAnimatableParticleCount() {
			double max = !this.MaxRegenStrength.HasValue || this.MaxRegenStrength.Value <= 0d
				? this.InitialStrength
				: this.MaxRegenStrength.Value;

			return 24 + (int)( max / 3d );
		}
	}
}
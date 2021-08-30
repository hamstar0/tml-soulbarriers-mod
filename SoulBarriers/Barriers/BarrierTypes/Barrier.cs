using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Buffs;


namespace SoulBarriers.Barriers.BarrierTypes {
	public abstract partial class Barrier {
		public static Vector2 GetEntityBarrierOrigin( Entity host ) {
			if( host is Player ) {
				return ( (Player)host ).MountedCenter;
			} else {
				return host.Center;
			}
		}



		////////////////

		protected IDictionary<Dust, Vector2> _ParticleOffsets { get; } = new Dictionary<Dust, Vector2>();

		public IReadOnlyDictionary<Dust, Vector2> ParticleOffsets { get; private set; }


		////

		public double InitialStrength { get; protected set; }

		////

		public double Strength { get; protected set; } = 0;

		public double? MaxRegenStrength { get; protected set; } = null;

		public double StrengthRegenPerTick { get; protected set; } = 0d;

		////

		public BarrierColor BarrierColor { get; protected set; }


		////////////////

		public BarrierHostType HostType { get; protected set; }

		public int HostWhoAmI { get; protected set; }

		////

		public Entity Host {
			get {
				switch( this.HostType ) {
				case BarrierHostType.Player:
					return Main.player[ this.HostWhoAmI ];
				case BarrierHostType.NPC:
					return Main.npc[ this.HostWhoAmI ];
				case BarrierHostType.None:
				default:
					return null;
				}
			}
		}

		public bool IsActive => this.Strength > 0d;



		////////////////

		public Barrier(
					BarrierHostType barrierHostType,
					int hostWhoAmI,
					double strength,
					double? maxRegenStrength,
					double strengthRegenPerTick,
					BarrierColor color ) {
			this.ParticleOffsets = new ReadOnlyDictionary<Dust, Vector2>( this._ParticleOffsets );

			this.HostType = barrierHostType;
			this.HostWhoAmI = hostWhoAmI;
			this.Strength = strength;
			this.MaxRegenStrength = maxRegenStrength;
			this.StrengthRegenPerTick = strengthRegenPerTick;
			this.BarrierColor = color;

			this.InitialStrength = strength;
		}


		////////////////

		public abstract bool CanSave();


		////////////////

		public abstract string GetID();


		////////////////

		public abstract Vector2 GetBarrierWorldCenter();

		////

		public Vector2 GetWorldPositionWithinBarrierArea( Vector2 offset, out bool isOoB ) {
			Vector2 pos = this.GetBarrierWorldCenter() + offset;
			isOoB = pos.X <= 0 || pos.X >= Main.maxTilesX * 16
				|| pos.Y <= 0 || pos.Y >= Main.maxTilesY * 16;
			return pos;
		}

		////

		public abstract Vector2 GetRandomOffsetWithinAreaForFx( Vector2 origin, bool isFxOnly, out bool isFarAway );


		////////////////

		public double GetStrengthPercent( double? maxStrength = null ) {
			if( this.MaxRegenStrength.HasValue && this.MaxRegenStrength > 0d ) {
				return this.Strength / this.MaxRegenStrength.Value;
			}

			if( maxStrength.HasValue ) {
				return this.Strength / maxStrength.Value;
			}

			return this.Strength / this.InitialStrength;
		}


		////////////////

		public void SetStrength( double strength, bool clearRegenBuffer ) {
			if( strength < 0d ) {
				strength = 0d;
			}

			this.Strength = strength;

			if( clearRegenBuffer ) {
				this.BufferedStrengthRegen = 0f;
			}

			if( this.HostType == BarrierHostType.Player ) {
				this.RefreshForPlayerHost();
			} else if( this.HostType == BarrierHostType.NPC ) {
				this.RefreshForNpcHost();
			}
		}

		////////////////

		private void RefreshForPlayerHost() {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			Player plr = (Player)this.Host;

			if( this.Strength > 0d ) {
				plr.AddBuff( soulBuffType, 2 );
			} else {
				plr.ClearBuff( soulBuffType );
			}
		}

		private void RefreshForNpcHost() {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			NPC host = (NPC)this.Host;

			if( this.Strength > 0d ) {
				host.AddBuff( soulBuffType, 2 );
			} else {
				int buffIdx = host.FindBuffIndex( soulBuffType );
				if( buffIdx >= 0 ) {
					host.DelBuff( buffIdx );
				}
			}
		}
	}
}
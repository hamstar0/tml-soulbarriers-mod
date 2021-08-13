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

		public int Strength { get; protected set; } = 0;

		public int? MaxRegenStrength { get; protected set; } = null;

		public float StrengthRegenPerTick { get; protected set; } = 0;

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

		public bool IsActive => this.Strength >= 1;



		////////////////

		public Barrier(
					BarrierHostType barrierHostType,
					int hostWhoAmI,
					int strength,
					int? maxRegenStrength,
					float strengthRegenPerTick,
					BarrierColor color ) {
			this.ParticleOffsets = new ReadOnlyDictionary<Dust, Vector2>( this._ParticleOffsets );

			this.HostType = barrierHostType;
			this.HostWhoAmI = hostWhoAmI;
			this.Strength = strength;
			this.MaxRegenStrength = maxRegenStrength;
			this.StrengthRegenPerTick = strengthRegenPerTick;
			this.BarrierColor = color;
		}


		////////////////

		public abstract bool CanSave();


		////////////////

		public abstract string GetID();


		////////////////

		public abstract Vector2 GetBarrierWorldCenter();
		
		public abstract Vector2 GetRandomOffsetForArea( Vector2 origin, bool isFxOnly, out bool isFarAway );

		public Vector2 GetWorldPositionWithinBarrierArea( Vector2 offset, out bool isOoB ) {
			Vector2 pos = this.GetBarrierWorldCenter() + offset;
			isOoB = pos.X <= 0 || pos.X >= Main.maxTilesX*16
				|| pos.Y <= 0 || pos.Y >= Main.maxTilesY*16;
			return pos;
		}


		////////////////

		public void SetStrength( int strength ) {
			if( strength < 0 ) {
				strength = 0;
			}

			this.Strength = strength;

			this.BufferedStrengthRegen = 0f;

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

			if( this.Strength >= 1 ) {
				plr.AddBuff( soulBuffType, 2 );
			} else {
				plr.ClearBuff( soulBuffType );
			}
		}

		private void RefreshForNpcHost() {
			int soulBuffType = ModContent.BuffType<SoulBarrierBuff>();
			NPC host = (NPC)this.Host;

			if( this.Strength >= 1 ) {
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
using System;
using System.Collections.Generic;
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

		protected IDictionary<Dust, Vector2> ParticleOffsets = new Dictionary<Dust, Vector2>();

		////

		protected BarrierHostType HostType;

		protected int HostWhoAmI;


		////////////////

		public int Strength { get; protected set; } = 0;

		public BarrierColor BarrierColor { get; protected set; }


		////////////////

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



		////////////////

		public Barrier( BarrierHostType barrierHostType, int hostWhoAmI, BarrierColor color ) {
			this.HostType = barrierHostType;
			this.HostWhoAmI = hostWhoAmI;
			this.BarrierColor = color;
		}


		////////////////
		
		public abstract Vector2 GetBarrierWorldCenter();
		
		public abstract Vector2 GetRandomOffsetForArea();


		////////////////

		public void SetStrength( int strength ) {
			if( strength < 0 ) {
				strength = 0;
			}

			this.Strength = strength;

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


		////////////////

		internal abstract void UpdateWithContext();
	}
}
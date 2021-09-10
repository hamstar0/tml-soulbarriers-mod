using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		public static (string stats, Vector2 dim) GetBarrierStatsData( Barrier barrier ) {
			string stats = (int)Math.Ceiling(barrier.Strength) + " hp";
			Vector2 statsDim = Main.fontMouseText.MeasureString( stats );

			return (stats, statsDim);
		}



		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Inventory" ) );
			if( idx == -1 ) {
				return;
			}

			var statsLayer = new LegacyGameInterfaceLayer(
				"SoulBarriers: Barrier Stats Popup",
				this.DrawBarrierStats,
				InterfaceScaleType.Game
			);
			layers.Insert( idx, statsLayer );
		}

		////

		private bool DrawBarrierStats() {
			foreach( (int plrWho, Barrier barrier) in BarrierManager.Instance.GetPlayerBarriers() ) {
				if( barrier.Strength <= 0d ) {
					continue;
				}

				Player plr = Main.player[plrWho];

				if( barrier is SphericalBarrier ) {
					this.DisplayPlayerBarrierStatsIf( Main.spriteBatch, barrier as SphericalBarrier, plr );
				}
			}

			foreach( Barrier barrier in BarrierManager.Instance.GetWorldBarriers().Values ) {
				if( barrier.Strength <= 0d ) {
					continue;
				}

				if( barrier is RectangularBarrier ) {
					this.DisplayRectangularBarrierStatsIf( Main.spriteBatch, barrier as RectangularBarrier );
				}
			}

			return true;
		}


		////////////////

		private void DisplayPlayerBarrierStatsIf( SpriteBatch sb, SphericalBarrier barrier, Player plr ) {
			Vector2 barrierPos = barrier.GetBarrierWorldCenter();
			float radius = ((SphericalBarrier)barrier).Radius;

			if( (barrierPos - Main.MouseWorld).LengthSquared() < (radius * radius) ) {
				this.DisplayPlayerSphereBarrierStats( sb, plr, barrier );
			}
		}
		
		private void DisplayRectangularBarrierStatsIf( SpriteBatch sb, RectangularBarrier barrier ) {
			if( !barrier.WorldArea.Contains(Main.MouseWorld.ToPoint()) ) {
				return;
			}

			Vector2 worldPos = Main.MouseWorld + new Vector2(0f, -28f);
			(string stats, Vector2 dim) stats = SoulBarriersMod.GetBarrierStatsData( barrier );

			this.DisplayBarrierStats( sb, worldPos, stats, barrier.Color );
		}


		////////////////

		private void DisplayPlayerSphereBarrierStats( SpriteBatch sb, Player player, SphericalBarrier barrier ) {
			(string stats, Vector2 dim) stats = SoulBarriersMod.GetBarrierStatsData( barrier );

			Vector2 worldPos = player.MountedCenter;
			worldPos.Y -= barrier.Radius + (stats.dim.Y * 1.5f);
			//pos.X -= statsDim.X * 0.5f;

			this.DisplayBarrierStats( sb, worldPos, stats, barrier.Color );
		}


		////////////////

		private void DisplayBarrierStats(
					SpriteBatch sb,
					Vector2 worldPos,
					(string stats, Vector2 dim) stats,
					Color color ) {
//DebugLibraries.Print( "barrier stats", worldPos.ToString()+", "+(worldPos-Main.screenPosition) );
			color = Color.Lerp( color, Color.White, 0.25f );

			Utils.DrawBorderStringFourWay(
				sb: sb,
				font: Main.fontMouseText,
				text: stats.stats,
				x: worldPos.X - Main.screenPosition.X,
				y: worldPos.Y - Main.screenPosition.Y,
				textColor: color * ((float)Main.mouseTextColor / 255f),
				borderColor: Color.Black,
				origin: stats.dim * 0.5f
			);
		}
	}
}
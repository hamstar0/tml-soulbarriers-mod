using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			foreach( (int plrWho, Barrier barrier) in BarrierManager.Instance.GetPlayerBarriers() ) {
				if( barrier.Strength <= 0 ) {
					continue;
				}

				Player plr = Main.player[ plrWho ];
				Vector2 barrierPos = barrier.GetBarrierWorldCenter();

				if( barrier is SphericalBarrier ) {
					float radius = ((SphericalBarrier)barrier).Radius;

					if( (barrierPos - Main.MouseWorld).LengthSquared() < (radius * radius) ) {
						this.DisplayBarrierStats( sb, plr, barrier, radius );
					}
				}
			}
		}


		////

		private void DisplayBarrierStats( SpriteBatch sb, Player player, Barrier barrier, float radius ) {
			string stats = barrier.Strength+" hp";
			Vector2 statsDim = Main.fontMouseText.MeasureString( stats );

			Vector2 pos = player.MountedCenter;
			pos.Y -= radius;
			pos.X -= statsDim.X * 0.5f;

			sb.DrawString(
				spriteFont: Main.fontMouseText,
				text: stats,
				position: pos,
				color: Barrier.GetColor( barrier.BarrierColor )
			);
		}
	}
}
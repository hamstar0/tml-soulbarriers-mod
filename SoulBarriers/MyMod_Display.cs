using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			foreach( (int plrWho, Barrier barrier) in BarrierManager.Instance.GetPlayerBarriers() ) {
				if( barrier.Strength <= 0 ) {
					continue;
				}

				Player plr = Main.player[ plrWho ];

				if( (plr.MountedCenter - Main.MouseWorld).LengthSquared() < (barrier.Radius * barrier.Radius) ) {
					this.DisplayBarrierStats( sb, plr, barrier );
				}
			}
		}


		////

		private void DisplayBarrierStats( SpriteBatch sb, Player player, Barrier barrier ) {
			string stats = barrier.Strength+" hp";
			Vector2 statsDim = Main.fontMouseText.MeasureString( stats );

			Vector2 pos = player.MountedCenter;
			pos.Y -= barrier.Radius;
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
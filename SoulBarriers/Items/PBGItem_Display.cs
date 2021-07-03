using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsGeneral.Services.AnimatedColor;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			int barrierStr = myplayer.GetBarrierStrength();
			var tip1 = new TooltipLine(
				this.mod,
				"PlayerBarrierStrength",
				"Current barrier strength: " + barrierStr
			);

			if( barrierStr <= 0 ) {
				tip1.overrideColor = Color.DarkGray;
			} else if( barrierStr >= Main.LocalPlayer.statManaMax2 ) {
				tip1.overrideColor = Main.DiscoColor;
			} else {
				tip1.overrideColor = Color.OrangeRed;
			}

			//

			var tip2 = new TooltipLine(
				this.mod,
				"PlayerBarrierHear",
				"Current barrier overheat: %" + this.HeatBuildup
			);

			if( this.HeatBuildup <= 0 ) {
				tip2.overrideColor = Color.Lime;
			} else if( this.HeatBuildup <= 75 ) {
				tip2.overrideColor = Color.White;
			} else if( this.HeatBuildup <= 99 ) {
				tip2.overrideColor = Color.Yellow;
			} else {
				tip2.overrideColor = Color.Red;
			}

			//

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}


		////////////////

		public override Color? GetAlpha( Color lightColor ) {
			if( this.HeatBuildup <= 0 ) {
				return null;
			}

			float percent = (float)this.HeatBuildup / 100f;
			percent = Math.Min( percent, 1f );

			Color color = Color.Red;
			if( percent >= 1f ) {
				color = AnimatedColors.Strobe.CurrentColor;
				color.G = 0;
				color.B = 0;
			}

			return Color.Lerp( lightColor, color, percent );
		}
	}
}
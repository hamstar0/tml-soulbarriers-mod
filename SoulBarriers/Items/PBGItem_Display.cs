using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsGeneral.Services.AnimatedColor;
using SoulBarriers.Buffs;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			int barrierStr = myplayer.Barrier.Strength;
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

			tooltips.Add( tip1 );
		}


		////////////////

		public override Color? GetAlpha( Color lightColor ) {
			if( !Main.LocalPlayer.HasBuff(ModContent.BuffType<PBGOverheatedDeBuff>()) ) {
				return null;
			}

			Color color = AnimatedColors.Strobe.CurrentColor;
			color.G = 0;
			color.B = 0;

			return color;
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Misc;
using ModLibsGeneral.Services.AnimatedColor;
using SoulBarriers.Buffs;


namespace SoulBarriers.Items {
	public partial class PBGItem : ModItem {
		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			double str = myplayer.Barrier.Strength;
			/*int maxStr = myplayer.Barrier.MaxRegenStrength;

			string msg = str.ToString();
			if( maxStr >= 1 && str > maxStr ) {
				msg = "+" + msg;
			}
			string postMsg = maxStr >= 1 && str <= maxStr
				? " of "+maxStr
				: "";*/

			string colorCode;
			if( str <= 0 ) {
				colorCode = "444444";
			} else if( str <= 50d ) {
				Color c = Color.Lerp( Color.Red, Color.Yellow, (float)(str / 50d) );
				colorCode = MiscLibraries.RenderColorHex( c );
			} else if( str <= 100d ) {
				double sStr = str - 50d;
				Color c = Color.Lerp( Color.Yellow, Color.Lime, (float)(sStr / 50d) );
				colorCode = MiscLibraries.RenderColorHex( c );
			} else if( str <= 150 ) {
				double sStr = str - 100d;
				Color c = Color.Lerp( Color.Lime, Color.Blue, (float)(sStr / 50d) );
				colorCode = MiscLibraries.RenderColorHex( c );
			} else if( str <= 200 ) {
				double sStr = str - 150d;
				Color c = Color.Lerp( Color.Lime, Color.Magenta, (float)(sStr / 50d) );
				colorCode = MiscLibraries.RenderColorHex( c );
			} else {
				colorCode = MiscLibraries.RenderColorHex( Color.Cyan );
			}

			/*} else if( str < maxStr ) {
				colorCode = MiscLibraries.RenderColorHex( Color.OrangeRed );
			} else if( maxStr >= 1 ) {
				colorCode = MiscLibraries.RenderColorHex( Main.DiscoColor );
			} else {
				colorCode = "44DDFF";//MiscLibraries.RenderColorHex( Color.White );
			}*/

			var tip1 = new TooltipLine(
				this.mod,
				"PlayerBarrierStrength",
				"Current barrier strength: [c/"+colorCode+":"+str+"]"
			);

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
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
		public static string GetColorCode( int range, float percent ) {
			Color c;

			switch( range ) {
			case 0:
				return "444444";
			case 1:
				c = Color.Lerp( Color.Red, Color.Yellow, percent );
				return MiscLibraries.RenderColorHex( c );
			case 2:
				c = Color.Lerp( Color.Yellow, Color.Lime, percent );
				return MiscLibraries.RenderColorHex( c );
			case 3:
				c = Color.Lerp( Color.Lime, Color.Blue, percent );
				return MiscLibraries.RenderColorHex( c );
			case 4:
				c = Color.Lerp( Color.Blue, Color.Magenta, percent );
				return MiscLibraries.RenderColorHex( c );
			default:
				return MiscLibraries.RenderColorHex( Color.Cyan );
			}
		}

		public static double GetDisplayStrength( double strength, out string colorCode ) {
			if( strength <= 0 ) {
				colorCode = PBGItem.GetColorCode( 0, 1f );
			} else if( strength <= 50d ) {
				double sStr = strength;
				colorCode = PBGItem.GetColorCode( 1, (float)(sStr / 50d) );
			} else if( strength <= 100d ) {
				double sStr = strength - 50d;
				colorCode = PBGItem.GetColorCode( 2, (float)(sStr / 50d) );
			} else if( strength <= 150d ) {
				double sStr = strength - 100d;
				colorCode = PBGItem.GetColorCode( 3, (float)(sStr / 50d) );
			} else if( strength <= 200d ) {
				double sStr = strength - 150d;
				colorCode = PBGItem.GetColorCode( 4, (float)(sStr / 50d) );
			} else {
				colorCode = PBGItem.GetColorCode( 5, 1f );
			}

			/*} else if( str < maxStr ) {
				colorCode = MiscLibraries.RenderColorHex( Color.OrangeRed );
			} else if( maxStr >= 1 ) {
				colorCode = MiscLibraries.RenderColorHex( Main.DiscoColor );
			} else {
				colorCode = "44DDFF";//MiscLibraries.RenderColorHex( Color.White );
			}*/

			//

			double dispStrength = (int)strength;
			int percent = (int)( ( strength % 1d ) * 100d );
			dispStrength += (double)percent / 100d;

			return dispStrength;
		}


		////////////////

		public static (bool success, string id, string text) GetBarrierStrengthTipTexts() {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			double strength = PBGItem.GetDisplayStrength( myplayer.Barrier.Strength, out string colorCode );

			return (
				true,
				"PlayerBarrierStrength",
				" Barrier strength: [c/" + colorCode + ":" + strength + "]"
			);
		}

		public static (bool success, string id, string text) GetBarrierCooldownTipTexts() {
			int buffIdx = Main.LocalPlayer.FindBuffIndex( ModContent.BuffType<PBGOverheatedDeBuff>() );
			if( buffIdx == -1 ) {
				return (false, null, null);
			}

			int buffTime = Main.LocalPlayer.buffTime[buffIdx];
			string buffTimeDisp = MiscLibraries.RenderTickDuration( buffTime );

			var config = SoulBarriersConfig.Instance;
			int seconds = config.Get<int>( nameof(config.PBGOverheatDurationSeconds) );
			int ticks = seconds * 60;
			string colorCode = PBGItem.GetColorCode( 1, (float)buffTime / (float)ticks );

			return (
				true,
				"PlayerBarrierCooldown",
				" Barrier cooldown: [c/" + colorCode + ":" + buffTimeDisp + "]"
			);
		}



		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			(bool success, string id, string text) tipData;

			//

			tipData = PBGItem.GetBarrierStrengthTipTexts();
			if( tipData.success ) {
				var tip = new TooltipLine( this.mod, tipData.id, tipData.text );
				tooltips.Add( tip );
			}

			tipData = PBGItem.GetBarrierCooldownTipTexts();
			if( tipData.success ) {
				var tip = new TooltipLine( this.mod, tipData.id, tipData.text );
				tooltips.Add( tip );
			}
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
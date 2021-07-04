using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace SoulBarriers.Barriers {
	public enum BarrierColor {
		Red = 60,
		Green = 61,
		Purple = 62,
		White = 63,
		Yellow = 64,
		BigBlue = 206
	}




	public class BarrierHelpers {
		public static Color GetColor( BarrierColor color ) {
			switch( color ) {
			case BarrierColor.Red:
				return Color.Red;
			case BarrierColor.Green:
				return Color.Lime;
			case BarrierColor.Purple:
				return Color.Purple;
			case BarrierColor.Yellow:
				return Color.Yellow;
			case BarrierColor.BigBlue:
				return Color.Blue;
			case BarrierColor.White:
			default:
				return Color.White;
			}
		}


		public static Vector2 GetEntityBarrierOrigin( Entity host ) {
			if( host is Player ) {
				return ((Player)host).MountedCenter;
			} else {
				return host.Center;
			}
		}
	}
}
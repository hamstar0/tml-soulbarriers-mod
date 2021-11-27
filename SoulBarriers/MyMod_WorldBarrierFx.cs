using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	public partial class SoulBarriersMod : Mod {
		private void AnimateWorldBarrierFx() {
			int tileDistBuffer = 8 * 16;

			Rectangle plrWldRect = Main.LocalPlayer.getRect();
			plrWldRect.X -= 80 * 16 + tileDistBuffer;
			plrWldRect.Y -= 60 * 16 + tileDistBuffer;
			plrWldRect.Width += 160 * 16 + (tileDistBuffer * tileDistBuffer);
			plrWldRect.Height += 120 * 16 + (tileDistBuffer * tileDistBuffer);

			Rectangle plrTileRect = new Rectangle(
				plrWldRect.X / 16,
				plrWldRect.Y / 16,
				plrWldRect.Width / 16,
				plrWldRect.Height / 16
			);

			foreach( (Rectangle tileRect, Barrier barrier) in BarrierManager.Instance.GetTileBarriers() ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( !plrTileRect.Intersects(tileRect) ) {
					continue;
				}

				int particles = barrier.ComputeCappedNormalParticleCount();

				barrier.Animate( particles );
//DebugLibraries.Print( "worldbarrier "+rect, "has:"+barrier.ParticleOffsets.Count+", of:"+particles );
			}
		}
	}
}

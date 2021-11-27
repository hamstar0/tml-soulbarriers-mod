using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.World;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;


namespace SoulBarriers.Commands {
	public class CreateTestWorldBarrierCommand : ModCommand {
		private static int TestBarrierCount = 0;



		////////////////

		public override CommandType Type => CommandType.World;

		public override string Command => "sb-debugworldbarrier";

		public override string Usage => "/"+this.Command+" <strength>";

		public override string Description => "Creates a test world barrier in front of the player.";



		////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( !SoulBarriersConfig.Instance.DebugModeWorldBarrierTest ) {
				caller.Reply( "Debug mode must be enabled.", Color.Yellow );
				return;
			}

			if( args.Length != 1 ) {
				caller.Reply( "Incorrect parameter quantity.", Color.Yellow );
				return;
			}

			if( !int.TryParse(args[0], out int str) ) {
				caller.Reply( args[0] + " is not an integer", Color.Yellow );
			}

			var rect = caller.Player.getRect();
			rect.X += caller.Player.direction * (32 * 16);
			rect.Y = 1;	//(int)caller.Player.MountedCenter.Y - (64 * 16);
			rect.Width = 8 * 16;
			rect.Height = WorldLocationLibraries.RockLayerTopTileY * 16;	//128 * 16;

			var barrier = new AccessBarrier(
				id: "TestBarrier_"+(CreateTestWorldBarrierCommand.TestBarrierCount++),
				strength: str,
				maxRegenStrength: str,
				strengthRegenPerTick: 5f / 60f,
				tileArea: rect,
				color: Color.Red,
				isSaveable: false
			);

			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				SoulBarriersAPI.DeclareWorldAccessBarrier( barrier );
			} else {
				Main.NewText( "Cannot call command in MP?" );
			}
		}
	}
}
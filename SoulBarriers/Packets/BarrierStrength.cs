using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class BarrierStrengthPacket : SimplePacketPayload {
		public static void SendToClient(
					int plrWho,
					Barrier barrier,
					double strength,
					bool applyHitFx,
					bool clearRegenBuffer ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			//

			var packet = new BarrierStrengthPacket( plrWho, barrier, strength, applyHitFx, clearRegenBuffer );

			SimplePacket.SendToClient( packet, plrWho, -1 );
		}


		public static void SyncToServerToEveryone_Local(
					Barrier barrier,
					double strength,
					bool applyHitFx,
					bool clearRegenBuffer ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not client." );
			}
			if( barrier.Host.whoAmI != Main.myPlayer ) {
				throw new ModLibsException( "Not local client." );
			}

			//

			var packet = new BarrierStrengthPacket(
				barrier.Host.whoAmI,
				barrier,
				strength,
				applyHitFx,
				clearRegenBuffer
			);

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlayerWho;

		public string BarrierID;

		public double Strength;

		public bool ApplyHitFx;

		public bool ClearRegenBuffer;



		////////////////

		private BarrierStrengthPacket() { }

		private BarrierStrengthPacket(
					int playerWho,
					Barrier barrier,
					double strength,
					bool applyHitFx,
					bool clearRegenBuffer ) {
			this.PlayerWho = playerWho;
			this.BarrierID = barrier.ID;
			this.Strength = strength;
			this.ApplyHitFx = applyHitFx;
			this.ClearRegenBuffer = clearRegenBuffer;
		}

		////////////////

		private void Receive( int fromWho ) {
			Barrier barrier = BarrierManager.Instance.GetBarrierByID( this.BarrierID );
			if( barrier == null ) {
				LogLibraries.Warn( $"No such barrier from {Main.player[fromWho].name} ({fromWho}) id'd: "
					+this.BarrierID );

				return;
			}

			//

			if( Main.netMode != NetmodeID.Server ) {
				double damage = barrier.Strength - this.Strength;

				if( this.ApplyHitFx ) {
					barrier.ApplyHitFx(
						damage > 0d ? (int)damage : 8,
						1f,
						damage,
						!barrier.IsActive
					);
				}
			}

			//

			barrier.SetStrength( this.Strength, this.ClearRegenBuffer, false, false );

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "Barrier strength set: "+barrier.ID
					+", Strength:"+this.Strength
					+", ApplyHitFx:"+this.ApplyHitFx
					+", ClearRegenBuffer:"+this.ClearRegenBuffer
				);
			}
		}

		////

		public override void ReceiveOnClient() {
			this.Receive( this.PlayerWho );
		}

		public override void ReceiveOnServer( int fromWho ) {
			this.Receive( fromWho );

			SimplePacket.SendToClient( this, -1, fromWho );
		}
	}
}

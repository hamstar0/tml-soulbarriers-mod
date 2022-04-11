using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers.BarrierTypes.Spherical.Personal;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Packets {
	class NPCBarrierCreatePacket : SimplePacketPayload {
		public static void BroadcastToClients( PersonalBarrier barrier ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new NPCBarrierCreatePacket( barrier );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string ID;

		public int HostWhoAmI;

		public double Strength;

		public double StrengthRegenPerTick;



		////////////////

		private NPCBarrierCreatePacket() { }

		private NPCBarrierCreatePacket( PersonalBarrier barrier ) {
			this.ID = barrier.ID;
			this.HostWhoAmI = barrier.HostWhoAmI;
			this.Strength = barrier.Strength;
			this.StrengthRegenPerTick = barrier.StrengthRegenPerTick;
		}

		////////////////

		public override void ReceiveOnClient() {
			Barrier barrier = SoulBarriersNPC.ApplySpawnBarrierIf(
				npcWho: this.HostWhoAmI,
				customStrength: (int?)this.Strength,
				customStrengthRegenPerTick: (int?)this.StrengthRegenPerTick
			);

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert( "NPC barrier created: "+ barrier.ID
					+", NPC Who:"+barrier.HostWhoAmI
					+", Radius:"+((PersonalBarrier)barrier).Radius
					+", Strength:"+barrier.Strength
					+", StrengthRegenPerTick:"+barrier.StrengthRegenPerTick
				);
			}
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server isn't synced new barriers." );
		}
	}
}
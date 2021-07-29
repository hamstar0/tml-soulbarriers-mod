using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers.Packets {
	class WorldBarrierCreatePacket : SimplePacketPayload {
		public static void BroadcastToClients( RectangularBarrier barrier ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new WorldBarrierCreatePacket( barrier );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		private int HostType;

		private int HostWhoAmI;

		private Rectangle WorldArea;

		private int Strength;

		private int MaxRegenStrength;

		private float StrengthRegenPerTick;

		private int Color;



		////////////////

		private WorldBarrierCreatePacket() { }

		private WorldBarrierCreatePacket( RectangularBarrier barrier ) {
			this.HostType = (int)barrier.HostType;
			this.HostWhoAmI = barrier.HostWhoAmI;
			this.WorldArea = barrier.WorldArea;
			this.Strength = barrier.Strength;
			this.MaxRegenStrength = barrier.MaxRegenStrength;
			this.StrengthRegenPerTick = barrier.StrengthRegenPerTick;
			this.Color = (int)barrier.BarrierColor;
		}

		////////////////

		public override void ReceiveOnClient() {
			BarrierManager.Instance.CreateAndDeclareWorldBarrier(
				hostType: (BarrierHostType)this.HostType,
				hostWhoAmI: this.HostWhoAmI,
				worldArea: this.WorldArea,
				strength: this.Strength,
				maxRegenStrength: this.MaxRegenStrength,
				strengthRegenPerTick: this.StrengthRegenPerTick,
				color: (BarrierColor)this.Color,
				isSaveable: true,
				syncFromServer: false
			);
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server isn't synced new barriers." );
		}
	}
}
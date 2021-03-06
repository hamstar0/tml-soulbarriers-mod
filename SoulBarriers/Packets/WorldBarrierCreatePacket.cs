using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers.Packets {
	class WorldBarrierCreatePacket : SimplePacketPayload {
		public static void BroadcastToClients( RectangularBarrier barrier ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new WorldBarrierCreatePacket( barrier );

			SimplePacket.SendToClient( packet );
		}



		////////////////

		public string BarrierType;

		public string ID;
		
		public int HostType;

		public int HostWhoAmI;

		public Rectangle TileArea;

		public double Strength;

		public double MaxRegenStrength;

		public double StrengthRegenPerTick;

		public byte ColorR;
		public byte ColorG;
		public byte ColorB;



		////////////////

		private WorldBarrierCreatePacket() { }

		private WorldBarrierCreatePacket( RectangularBarrier barrier ) {
			this.BarrierType = barrier.GetType().Name;
			this.ID = barrier.ID;
			this.HostType = (int)barrier.HostType;
			this.HostWhoAmI = barrier.HostWhoAmI;
			this.TileArea = barrier.TileArea;
			this.Strength = barrier.Strength;
			this.MaxRegenStrength = barrier.MaxRegenStrength.HasValue
				? -1d
				: barrier.MaxRegenStrength.Value;
			this.StrengthRegenPerTick = barrier.StrengthRegenPerTick;
			this.ColorR = barrier.Color.R;
			this.ColorG = barrier.Color.G;
			this.ColorB = barrier.Color.B;
		}

		////////////////

		public override void ReceiveOnClient() {
			var color = new Color( this.ColorR, this.ColorG, this.ColorB );

			Barrier barrier = BarrierManager.Instance.FactoryCreateBarrier(
				barrierTypeName: this.BarrierType,
				id: this.ID,
				hostType: (BarrierHostType)this.HostType,
				hostWhoAmI: this.HostWhoAmI,
				data: this.TileArea,
				strength: this.Strength,
				maxRegenStrength: this.MaxRegenStrength == -1d
					? 0d
					: this.MaxRegenStrength,
				strengthRegenPerTick: this.StrengthRegenPerTick,
				color: color,
				isSaveable: true
			);

			BarrierManager.Instance.DeclareWorldBarrier_Unsynced( barrier as RectangularBarrier );

			//

			if( SoulBarriersConfig.Instance.DebugModeNetInfo ) {
				LogLibraries.Alert(
					"Barrier created: "+ barrier.ID
					+", Host:"+this.HostType+" ("+this.HostWhoAmI+")"
					+", TileArea:"+this.TileArea
					+", Strength:"+this.Strength
					+", MaxRegenStrength:"+this.MaxRegenStrength
					+", StrengthRegenPerTick:"+this.StrengthRegenPerTick
					+", Color:"+color
				);
			}
		}


		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server isn't synced new barriers." );
		}
	}
}
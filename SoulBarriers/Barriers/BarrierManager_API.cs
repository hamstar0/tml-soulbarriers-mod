using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Spherical;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers {
	public partial class BarrierManager : ILoadable {
		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}

		public int GetWorldBarrierCount() {
			return this.WorldBarriers.Count();
		}


		////////////////

		public Barrier GetOrMakePlayerBarrier( int playerWho ) {
			if( !this.PlayerBarriers.TryGetValue( playerWho, out Barrier barrier ) ) {
				barrier = new SphericalBarrier(
					hostType: BarrierHostType.Player,
					hostWhoAmI: playerWho,
					strength: 0,
					maxRegenStrength: 0,
					strengthRegenPerTick: 0f,
					radius: 48f,
					color: BarrierColor.BigBlue
				);
				this.PlayerBarriers[playerWho] = barrier;
			}
			return barrier;
		}

		public Barrier GetWorldBarrier( Rectangle worldArea ) {
			return this.WorldBarriers.GetOrDefault( worldArea );
		}


		////////////////

		public Barrier CreateAndDeclareWorldBarrier(
					BarrierHostType hostType,
					int hostWhoAmI,
					Rectangle worldArea,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					BarrierColor color,
					bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return null;
			}

			var barrier = new RectangularBarrier(
				hostType: hostType,
				hostWhoAmI: hostWhoAmI,
				strength: strength,
				maxRegenStrength: maxRegenStrength,
				strengthRegenPerTick: strengthRegenPerTick,
				worldArea: worldArea,
				color: color
			);

			this.WorldBarriers[worldArea] = barrier;

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				WorldBarrierCreatePacket.BroadcastToClients( barrier );
			} 

			return barrier;
		}


		////

		public void RemoveWorldBarrier( Rectangle worldArea, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			Barrier barrier = this.WorldBarriers.GetOrDefault( worldArea );
			if( barrier != null ) {
				this.BarriersByID.Remove( barrier.GetID() );

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					BarrierRemovePacket.BroadcastToClients( barrier );
				}
			}

			this.WorldBarriers.Remove( worldArea );
		}


		////////////////
		
		public Barrier GetBarrierByID( string id ) {
			return this.BarriersByID.GetOrDefault( id );
		}


		////////////////

		public IDictionary<int, Barrier> GetPlayerBarriers() {
			return this.PlayerBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}

		public IDictionary<Rectangle, Barrier> GetWorldBarriers() {
			return this.WorldBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}

		////

		public void RemoveAllWorldBarriers() {
			foreach( string id in this.WorldBarriers.Values.Select(b=>b.GetID()) ) {
				this.BarriersByID.Remove( id );
			}

			this.WorldBarriers.Clear();
		}
	}
}
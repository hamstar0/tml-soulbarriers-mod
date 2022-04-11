using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;
using SoulBarriers.Packets;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public int GetWorldBarrierCount() {
			return this.TileBarriers.Count();
		}


		////////////////

		public IDictionary<Rectangle, Barrier> GetTileBarriers() {
			return this.TileBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}


		////////////////

		public Barrier GetWorldBarrier( Rectangle tileArea ) {
			return this.TileBarriers.GetOrDefault( tileArea );
		}


		////////////////
		
		public bool DeclareWorldBarrierUnsynced( RectangularBarrier barrier ) {
			this.TileBarriers[barrier.TileArea] = barrier;

			//

			SoulBarriersAPI.RunBarrierCreateHooks( barrier );

			return true;
		}


		////////////////

		public void RemoveWorldBarrier( Rectangle tileArea, bool syncIfServer ) {
			Barrier barrier = this.TileBarriers.GetOrDefault( tileArea );

			if( barrier != null ) {
				this.BarriersByID.Remove( barrier.ID );

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					BarrierRemovePacket.BroadcastToClients( barrier );
				}
			}

			this.TileBarriers.Remove( tileArea );

			//
			
			SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
		}

		public void RemoveNonWorldBarrier( Barrier barrier, bool syncIfServer ) {
			if( barrier.Host != null ) {
				this.RemoveEntityBarrier( barrier.Host, syncIfServer );

				return;
			}

			//

			this.BarriersByID.Remove( barrier.ID );

			//
			
			if( syncIfServer && Main.netMode == NetmodeID.Server ) {
				BarrierRemovePacket.BroadcastToClients( barrier );
			}

			//
			
			SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
		}

		////

		public void RemoveAllWorldBarriersNoSync() {
			foreach( string id in this.TileBarriers.Values.Select(b=>b.ID) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.TileBarriers.Clear();
		}
	}
}
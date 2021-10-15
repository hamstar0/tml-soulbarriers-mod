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
using SoulBarriers.Barriers.BarrierTypes.Spherical.Personal;
using SoulBarriers.Packets;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}

		public int GetWorldBarrierCount() {
			return this.TileBarriers.Count();
		}


		////////////////

		public Barrier GetBarrierByID( string id ) {
			return this.BarriersByID.GetOrDefault( id );
		}

		public Barrier GetPlayerBarrier( int playerWho ) {
			return this.PlayerBarriers.GetOrDefault( playerWho );
		}


		////////////////

		public IDictionary<int, Barrier> GetPlayerBarriers() {
			return this.PlayerBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}

		public IDictionary<Rectangle, Barrier> GetTileBarriers() {
			return this.TileBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}


		////////////////

		public Barrier CreateAndDeclarePlayerBarrier( int playerWho ) {
			if( this.PlayerBarriers.ContainsKey(playerWho) ) {
				throw new ModLibsException( "Player barrier already exists." );
			}

			var config = SoulBarriersConfig.Instance;
			float radius = config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) );

			Barrier barrier = new PersonalBarrier(
				hostType: BarrierHostType.Player,
				hostWhoAmI: playerWho,
				strength: 0,
				radius: radius,
				color: Color.Lime
			);

			this.PlayerBarriers[playerWho] = barrier;

			SoulBarriersAPI.RunBarrierCreateHooks( barrier );

			return barrier;
		}

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


		////

		public void RemoveWorldBarrier( Rectangle tileArea, bool syncIfServer ) {
			Barrier barrier = this.TileBarriers.GetOrDefault( tileArea );
			if( barrier != null ) {
				this.BarriersByID.Remove( barrier.GetID() );

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					BarrierRemovePacket.BroadcastToClients( barrier );
				}
			}

			this.TileBarriers.Remove( tileArea );

			//
			
			SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
		}


		////////////////
		
		public void RemoveAllPlayerBarriers() {
			foreach( string id in this.PlayerBarriers.Values.Select(b=>b.GetID()) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.PlayerBarriers.Clear();
		}
		
		public void RemoveAllWorldBarriers() {
			foreach( string id in this.TileBarriers.Values.Select(b=>b.GetID()) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.TileBarriers.Clear();
		}
	}
}
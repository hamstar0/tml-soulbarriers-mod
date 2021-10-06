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
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;
using SoulBarriers.Packets;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}

		public int GetWorldBarrierCount() {
			return this.WorldBarriers.Count();
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

		public IDictionary<Rectangle, Barrier> GetWorldBarriers() {
			return this.WorldBarriers
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
			return this.WorldBarriers.GetOrDefault( tileArea );
		}


		////////////////

		public bool DeclareWorldBarrierUnsynced( RectangularBarrier barrier ) {
			this.WorldBarriers[barrier.TileArea] = barrier;

			//

			SoulBarriersAPI.RunBarrierCreateHooks( barrier );

			return true;
		}


		////

		public void RemoveWorldBarrier( Rectangle tileArea, bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			Barrier barrier = this.WorldBarriers.GetOrDefault( tileArea );
			if( barrier != null ) {
				this.BarriersByID.Remove( barrier.GetID() );

				if( syncFromServer && Main.netMode == NetmodeID.Server ) {
					BarrierRemovePacket.BroadcastToClients( barrier );
				}
			}

			this.WorldBarriers.Remove( tileArea );

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
			foreach( string id in this.WorldBarriers.Values.Select(b=>b.GetID()) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.WorldBarriers.Clear();
		}
	}
}
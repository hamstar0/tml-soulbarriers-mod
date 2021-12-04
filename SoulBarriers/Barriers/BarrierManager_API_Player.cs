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


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public int GetPlayerBarrierCount() {
			return this.PlayerBarriers.Count();
		}


		////////////////

		public Barrier GetPlayerBarrier( int playerWho ) {
			return this.PlayerBarriers.GetOrDefault( playerWho );
		}


		////

		public IDictionary<int, Barrier> GetPlayerBarriers() {
			return this.PlayerBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}


		////////////////

		public Barrier CreateAndDeclarePlayerBarrier( int playerWho ) {
			if( this.PlayerBarriers.ContainsKey(playerWho) ) {
				throw new ModLibsException( "Player barrier already exists." );
			}

			var config = SoulBarriersConfig.Instance;
			float radius = config.Get<float>( nameof(config.DefaultPlayerBarrierRadius) );
			// Decays slowly (1 hp / 3s)
			float strengthRegenPerTick = config.Get<float>( nameof(config.PBGBarrierDefaultRegenPercentPerTick) );

			Barrier barrier = new PersonalBarrier(
				id: "PlayerPersonalBarrier_"+playerWho,
				hostType: BarrierHostType.Player,
				hostWhoAmI: playerWho,
				strength: 0,
				strengthRegenPerTick: strengthRegenPerTick,
				radius: radius,
				color: Color.Lime
			);

			this.PlayerBarriers[playerWho] = barrier;

			SoulBarriersAPI.RunBarrierCreateHooks( barrier );

			return barrier;
		}


		////////////////

		public void RemoveAllPlayerBarriers() {
			foreach( string id in this.PlayerBarriers.Values.Select(b=>b.ID) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.PlayerBarriers.Clear();
		}
	}
}
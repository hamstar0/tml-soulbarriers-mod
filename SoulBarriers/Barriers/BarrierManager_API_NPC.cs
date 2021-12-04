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


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public int GetNPCBarrierCount() {
			return this.NPCBarriers.Count();
		}


		////////////////

		public Barrier GetNPCBarrier( int playerWho ) {
			return this.NPCBarriers.GetOrDefault( playerWho );
		}


		////

		public IDictionary<int, Barrier> GetNPCBarriers() {
			return this.NPCBarriers
				.ToDictionary( kv => kv.Key, kv => kv.Value );
		}


		////////////////

		public Barrier CreateAndDeclareActiveNPCBarrier( int npcWho, int strength, float strengthRegenPerTick, float radius ) {
			if( this.NPCBarriers.ContainsKey(npcWho) ) {
				throw new ModLibsException( "NPC barrier already exists." );
			}

			Barrier barrier = new PersonalBarrier(
				id: "NPCPersonalBarrier_"+npcWho,
				hostType: BarrierHostType.NPC,
				hostWhoAmI: npcWho,
				strength: strength,
				strengthRegenPerTick: strengthRegenPerTick,
				radius: radius,
				color: Color.Lime
			);

			this.NPCBarriers[npcWho] = barrier;

			SoulBarriersAPI.RunBarrierCreateHooks( barrier );

			return barrier;
		}


		////////////////

		public void RemoveNPCBarrier( int npcWho, bool syncIfServer ) {
			Barrier barrier = this.NPCBarriers.GetOrDefault( npcWho );

			if( barrier != null ) {
				this.BarriersByID.Remove( barrier.ID );

				if( syncIfServer && Main.netMode == NetmodeID.Server ) {
					BarrierRemovePacket.BroadcastToClients( barrier );
				}
			}

			this.NPCBarriers.Remove( npcWho );

			//

			SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
		}

		////

		public void RemoveAllNPCBarriersNoSync() {
			foreach( string id in this.NPCBarriers.Values.Select(b=>b.ID) ) {
				Barrier barrier = this.BarriersByID[id];

				if( this.BarriersByID.Remove(id) ) {
					SoulBarriersAPI.RunBarrierRemoveHooks( barrier );
				}
			}

			this.NPCBarriers.Clear();
		}
	}
}
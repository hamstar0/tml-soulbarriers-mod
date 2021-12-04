using System;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public Barrier GetBarrierByID( string id ) {
			return this.BarriersByID.GetOrDefault( id );
		}


		////////////////

		public Barrier GetBarrierOfEntity( Entity ent ) {
			if( ent is Player ) {
				return this.GetPlayerBarrier( ent.whoAmI );
			} else if( ent is NPC ) {
				return this.GetNPCBarrier( ent.whoAmI );
			}
			return null;
		}

		////////////////

		public void RemoveEntityBarrier( Entity ent, bool syncIfServer ) {
			if( ent is Player ) {
				this.RemovePlayerBarrier( ent.whoAmI, syncIfServer );
			} else if( ent is NPC ) {
				this.RemoveNPCBarrier( ent.whoAmI, syncIfServer );
			}
		}
	}
}
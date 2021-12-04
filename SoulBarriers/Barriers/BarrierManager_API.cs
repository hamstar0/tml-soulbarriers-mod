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
	}
}
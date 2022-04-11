using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	public partial class SoulBarriersAPI {
		public static void AddBarrierCreateHook( Action<Barrier> hook ) {
			ModContent.GetInstance<SoulBarriersAPI>().BarrierCreateHooks.Add( hook );
		}

		
		public static void AddBarrierRemoveHook( Action<Barrier> hook ) {
			ModContent.GetInstance<SoulBarriersAPI>().BarrierRemoveHooks.Add( hook );
		}


		////////////////

		internal static void RunBarrierCreateHooks( Barrier barrier ) {
			var api = ModContent.GetInstance<SoulBarriersAPI>();

			foreach( Action<Barrier> hook in api.BarrierCreateHooks ) {
				hook.Invoke( barrier );
			}
		}

		internal static void RunBarrierRemoveHooks( Barrier barrier ) {
			var api = ModContent.GetInstance<SoulBarriersAPI>();

			foreach( Action<Barrier> hook in api.BarrierRemoveHooks ) {
				hook.Invoke( barrier );
			}
		}



		////////////////

		private IList<Action<Barrier>> BarrierCreateHooks = new List<Action<Barrier>>();

		private IList<Action<Barrier>> BarrierRemoveHooks = new List<Action<Barrier>>();
	}
}

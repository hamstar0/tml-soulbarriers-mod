using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.TModLoader;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers.Barriers {
	partial class BarrierManager : ILoadable {
		public static BarrierManager Instance => ModContent.GetInstance<BarrierManager>();



		////////////////

		private IDictionary<int, Barrier> PlayerBarriers = new Dictionary<int, Barrier>();

		private IDictionary<int, Barrier> NPCBarriers = new Dictionary<int, Barrier>();

		private IDictionary<Rectangle, Barrier> TileBarriers = new Dictionary<Rectangle, Barrier>();

		private IDictionary<string, Barrier> BarriersByID = new Dictionary<string, Barrier>();

		////

		private IDictionary<string, IBarrierFactory> BarrierFactories = new Dictionary<string, IBarrierFactory>();



		////////////////

		void ILoadable.OnModsLoad() {
			this.RegisterBarrierFactories();
		}

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		private void RegisterBarrierFactories() {
			Type iBarrierFactoryType = typeof( IBarrierFactory );
			IEnumerable<Assembly> asses = ModLoader.Mods
				.SafeSelect( mod => mod.Code )
				.SafeWhere( code => code != null );

			foreach( Assembly ass in asses ) {
				foreach( Type classType in ass.GetTypes() ) {
					try {
						if( !classType.IsClass || classType.IsAbstract ) {
							continue;
						}
						if( !iBarrierFactoryType.IsAssignableFrom(classType) ) {
							continue;
						}

						object obj = TmlLibraries.SafelyGetInstanceForType( classType );

						if( obj is IBarrierFactory ) {
							string objName = obj.GetType().FullName;

							this.BarrierFactories[ objName ] = obj as IBarrierFactory;
						}
					} catch { }
				}
			}
		}


		////////////////

		public Barrier FactoryCreateBarrier(
					string barrierTypeName,
					string id,
					BarrierHostType hostType,
					int hostWhoAmI,
					object data,
					double strength,
					double maxRegenStrength,
					double strengthRegenPerTick,
					Color color,
					bool isSaveable ) {
			IBarrierFactory factoryBarrier = this.BarrierFactories.GetOrDefault( barrierTypeName );

			return factoryBarrier?.FactoryCreate(
				id: id,
				hostType: hostType,
				hostWhoAmI: hostWhoAmI,
				data: data,
				strength: strength,
				maxRegenStrength: maxRegenStrength,
				strengthRegenPerTick: strengthRegenPerTick,
				color: color,
				isSaveable: isSaveable
			);
		}
	}
}
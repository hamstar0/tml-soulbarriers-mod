using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers.BarrierTypes;


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
					Rectangle worldArea,
					int strength,
					int maxRegenStrength,
					float strengthRegenPerTick,
					BarrierColor color ) {
			this.WorldBarriers[worldArea] = new RectangularBarrier(
				hostType: BarrierHostType.None,
				hostWhoAmI: 0,
				strength: strength,
				maxRegenStrength: maxRegenStrength,
				strengthRegenPerTick: strengthRegenPerTick,
				worldArea: worldArea,
				color: color
			);
			return this.WorldBarriers[worldArea];
		}

		public bool RemoveWorldBarrier( Rectangle worldArea ) {
			return this.WorldBarriers.Remove( worldArea );
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
			this.WorldBarriers.Clear();
		}
	}
}
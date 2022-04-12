using System;
using Terraria;
using Microsoft.Xna.Framework;


namespace SoulBarriers.Barriers.BarrierTypes {
	public interface IBarrierFactory {
		Barrier FactoryCreate(
				string id,
				BarrierHostType hostType,
				int hostWhoAmI,
				object data,
				double strength,
				double maxRegenStrength,
				double strengthRegenPerTick,
				Color color,
				bool isSaveable );
	}
}
﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


namespace SoulBarriers {
	class SoulBarriersWorld : ModWorld {
		public override void Load( TagCompound tag ) {
			var mngr = BarrierManager.Instance;
			mngr.RemoveAllWorldBarriers();

			if( !tag.ContainsKey("barrier_count") ) {
				return;
			}

			int count = tag.GetInt( "barrier_count" );

			for( int i=0; i<count; i++ ) {
				int x = tag.GetInt( "barrier_"+i+"_area_x" );
				int y = tag.GetInt( "barrier_"+i+"_area_y" );
				int w = tag.GetInt( "barrier_"+i+"_area_w" );
				int h = tag.GetInt( "barrier_"+i+"_area_h" );
				int c = tag.GetInt( "barrier_"+i+"_color" );
				int maxStr = tag.GetInt( "barrier_"+i+"_max_str" );
				float strRegen = tag.GetInt( "barrier_"+i+"_str_regen" );
				
				mngr.CreateAndDeclareWorldBarrier(
					worldArea: new Rectangle(x, y, w, h),
					strength: maxStr,
					maxRegenStrength: maxStr,
					strengthRegenPerTick: strRegen,
					color: (BarrierColor)c
				);
			}
		}


		public override TagCompound Save() {
			var mngr = BarrierManager.Instance;
			int count = mngr.GetWorldBarrierCount();
			var tag = new TagCompound { { "barrier_count", count } };

			int i = 0;
			foreach( (Rectangle rect, Barrier barrier) in mngr.GetWorldBarriers() ) {
				tag[ "barrier_"+i+"_area_x" ] = rect.X;
				tag[ "barrier_"+i+"_area_y" ] = rect.Y;
				tag[ "barrier_"+i+"_area_w" ] = rect.Width;
				tag[ "barrier_"+i+"_area_h" ] = rect.Height;
				tag[ "barrier_"+i+"_color" ] = (int)barrier.BarrierColor;
				tag[ "barrier_"+i+"_max_str" ] = barrier.MaxRegenStrength;
				tag[ "barrier_"+i+"_str_regen" ] = barrier.StrengthRegenPerTick;
				i++;
			}

			return tag;
		}


		////

		public override void NetReceive( BinaryReader reader ) {
			var mngr = BarrierManager.Instance;

			int count = reader.ReadInt32();

			for( int i=0; i<count; i++ ) {
				var rect = new Rectangle(
					reader.ReadInt32(),
					reader.ReadInt32(),
					reader.ReadInt32(),
					reader.ReadInt32()
				);
				int strength = reader.ReadInt32();
				int maxRegenStrength = reader.ReadInt32();
				float strengthRegen = reader.ReadSingle();
				int color = reader.ReadInt32();

				mngr.CreateAndDeclareWorldBarrier(
					rect,
					strength,
					maxRegenStrength,
					strengthRegen,
					(BarrierColor)color
				);
			}
		}


		public override void NetSend( BinaryWriter writer ) {
			var mngr = BarrierManager.Instance;
			int count = mngr.GetWorldBarrierCount();

			writer.Write( count );

			foreach( (Rectangle rect, Barrier barrier) in mngr.GetWorldBarriers() ) {
				writer.Write( rect.X );
				writer.Write( rect.Y );
				writer.Write( rect.Width );
				writer.Write( rect.Height );
				writer.Write( barrier.Strength );
				writer.Write( barrier.MaxRegenStrength );
				writer.Write( barrier.StrengthRegenPerTick );
				writer.Write( (int)barrier.BarrierColor );
			}
		}
	}
}
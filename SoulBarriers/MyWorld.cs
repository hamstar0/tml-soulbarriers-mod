using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;


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
				byte cR = tag.GetByte( "barrier_"+i+"_color_r" );
				byte cG = tag.GetByte( "barrier_"+i+"_color_g" );
				byte cB = tag.GetByte( "barrier_"+i+"_color_b" );
				double maxStr = tag.GetDouble( "barrier_"+i+"_max_str" );
				double strRegen = tag.GetDouble( "barrier_"+i+"_str_regen" );
				
				var barrier = new AccessBarrier(
					hostType: BarrierHostType.None,
					hostWhoAmI: -1,
					worldArea: new Rectangle(x, y, w, h),
					strength: maxStr,
					maxRegenStrength: maxStr,
					strengthRegenPerTick: strRegen,
					color: new Color(cR, cG, cB),
					isSaveable: true
				);
				mngr.DeclareWorldBarrierUnsynced( barrier );
			}
		}


		public override TagCompound Save() {
			var mngr = BarrierManager.Instance;
			var tag = new TagCompound();

			int i = 0;
			foreach( (Rectangle rect, Barrier barrier) in mngr.GetWorldBarriers() ) {
				if( !barrier.CanSave() ) {
					continue;
				}

				tag[ "barrier_"+i+"_area_x" ] = (int)rect.X;
				tag[ "barrier_"+i+"_area_y" ] = (int)rect.Y;
				tag[ "barrier_"+i+"_area_w" ] = (int)rect.Width;
				tag[ "barrier_"+i+"_area_h" ] = (int)rect.Height;
				tag[ "barrier_"+i+"_color_r" ] = barrier.Color.R;
				tag[ "barrier_"+i+"_color_g" ] = barrier.Color.G;
				tag[ "barrier_"+i+"_color_b" ] = barrier.Color.B;
				tag[ "barrier_"+i+"_max_str" ] = (double)barrier.MaxRegenStrength;
				tag[ "barrier_"+i+"_str_regen" ] = (double)barrier.StrengthRegenPerTick;
				i++;
			}

			tag["barrier_count"] = (int)i;

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
				double strength = reader.ReadDouble();
				double maxRegenStrength = reader.ReadDouble();
				double strengthRegen = reader.ReadDouble();
				byte colorR = reader.ReadByte();
				byte colorG = reader.ReadByte();
				byte colorB = reader.ReadByte();

				var barrier = new AccessBarrier(
					worldArea: rect,
					strength: strength,
					maxRegenStrength: maxRegenStrength == -1 ? (int?)null : (int?)maxRegenStrength,
					strengthRegenPerTick: strengthRegen,
					color: new Color(colorR, colorG, colorB),
					isSaveable: true,
					hostType: BarrierHostType.None,
					hostWhoAmI: -1
				);
				mngr.DeclareWorldBarrierUnsynced( barrier );
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
				writer.Write( (double)barrier.Strength );
				writer.Write( (double)(barrier.MaxRegenStrength.HasValue ? barrier.MaxRegenStrength.Value : -1d) );;
				writer.Write( (double)barrier.StrengthRegenPerTick );
				writer.Write( barrier.Color.R );
				writer.Write( barrier.Color.G );
				writer.Write( barrier.Color.B );
			}
		}


		////////////////

		public override void PostDrawTiles() {
			var plrRect = Main.LocalPlayer.getRect();
			plrRect.X -= 80 * 16;
			plrRect.Y -= 60 * 16;
			plrRect.Width += 160 * 16;
			plrRect.Height += 120 * 16;

			foreach( (Rectangle rect, Barrier barrier) in BarrierManager.Instance.GetWorldBarriers() ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( !plrRect.Intersects(rect) ) {
					continue;
				}

				int particles = barrier.ComputeCurrentMaxAnimatedParticleCount();

				barrier.Animate( particles );
//DebugLibraries.Print( "worldbarrier "+rect, "has:"+barrier.ParticleOffsets.Count+", of:"+particles );
			}
		}
	}
}

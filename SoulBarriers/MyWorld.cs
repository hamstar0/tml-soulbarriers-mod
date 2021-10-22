using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers {
	class SoulBarriersWorld : ModWorld {
		public override void Load( TagCompound tag ) {
			var mngr = BarrierManager.Instance;
			mngr.RemoveAllWorldBarriers();

			if( !tag.ContainsKey("barrier_count2") ) {
				int count = tag.GetInt( "barrier_count2" );

				for( int i=0; i<count; i++ ) {
					string typeName = tag.GetString( "barrier_"+i+"_type" );
					int x = tag.GetInt( "barrier_"+i+"_area_x" );
					int y = tag.GetInt( "barrier_"+i+"_area_y" );
					int w = tag.GetInt( "barrier_"+i+"_area_w" );
					int h = tag.GetInt( "barrier_"+i+"_area_h" );
					byte cR = tag.GetByte( "barrier_"+i+"_color_r" );
					byte cG = tag.GetByte( "barrier_"+i+"_color_g" );
					byte cB = tag.GetByte( "barrier_"+i+"_color_b" );
					double maxStr = tag.GetDouble( "barrier_"+i+"_max_str" );
					double strRegen = tag.GetDouble( "barrier_"+i+"_str_regen" );

					Barrier barrier = BarrierManager.Instance.FactoryCreateBarrier(
						barrierTypeName: typeName,
						hostType: BarrierHostType.None,
						hostWhoAmI: -1,
						data: new Rectangle(x, y, w, h),
						strength: maxStr,
						maxRegenStrength: maxStr,
						strengthRegenPerTick: strRegen,
						color: new Color(cR, cG, cB),
						isSaveable: true
					);

					// TODO
					if( barrier is RectangularBarrier ) {
						mngr.DeclareWorldBarrierUnsynced( barrier as RectangularBarrier );
					}
				}
			}
		}


		public override TagCompound Save() {
			var mngr = BarrierManager.Instance;
			var tag = new TagCompound();

			int i = 0;
			foreach( (Rectangle tileRect, Barrier barrier) in mngr.GetTileBarriers() ) {
				if( !barrier.CanSave() ) {
					continue;
				}

				tag[ "barrier_"+i+"_type" ] = barrier.GetType().FullName;
				tag[ "barrier_"+i+"_area_x" ] = (int)tileRect.X;
				tag[ "barrier_"+i+"_area_y" ] = (int)tileRect.Y;
				tag[ "barrier_"+i+"_area_w" ] = (int)tileRect.Width;
				tag[ "barrier_"+i+"_area_h" ] = (int)tileRect.Height;
				tag[ "barrier_"+i+"_color_r" ] = barrier.Color.R;
				tag[ "barrier_"+i+"_color_g" ] = barrier.Color.G;
				tag[ "barrier_"+i+"_color_b" ] = barrier.Color.B;
				tag[ "barrier_"+i+"_max_str" ] = (double)barrier.MaxRegenStrength;
				tag[ "barrier_"+i+"_str_regen" ] = (double)barrier.StrengthRegenPerTick;
				i++;
			}

			tag["barrier_count2"] = (int)i;

			return tag;
		}


		////

		/*	<- World barriers aren't yet 'generalized' for this use; manual sync required
		
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
					tileArea: rect,
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
		}*/


		////////////////

		public override void PostDrawTiles() {
			int tileDistBuffer = 8 * 16;

			Rectangle plrWldRect = Main.LocalPlayer.getRect();
			plrWldRect.X -= 80 * 16 + tileDistBuffer;
			plrWldRect.Y -= 60 * 16 + tileDistBuffer;
			plrWldRect.Width += 160 * 16 + (tileDistBuffer * tileDistBuffer);
			plrWldRect.Height += 120 * 16 + (tileDistBuffer * tileDistBuffer);

			Rectangle plrTileRect = new Rectangle(
				plrWldRect.X / 16,
				plrWldRect.Y / 16,
				plrWldRect.Width / 16,
				plrWldRect.Height / 16
			);

			foreach( (Rectangle tileRect, Barrier barrier) in BarrierManager.Instance.GetTileBarriers() ) {
				if( !barrier.IsActive ) {
					continue;
				}
				if( !plrTileRect.Intersects(tileRect) ) {
					continue;
				}

				int particles = barrier.ComputeCappedNormalParticleCount();

				barrier.Animate( particles );
//DebugLibraries.Print( "worldbarrier "+rect, "has:"+barrier.ParticleOffsets.Count+", of:"+particles );
			}
		}
	}
}

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
				float strRegen = tag.GetFloat( "barrier_"+i+"_str_regen" );
				
				mngr.CreateAndDeclareWorldBarrier(
					hostType: BarrierHostType.None,
					hostWhoAmI: -1,
					worldArea: new Rectangle(x, y, w, h),
					strength: maxStr,
					maxRegenStrength: maxStr,
					strengthRegenPerTick: strRegen,
					color: (BarrierColor)c,
					isSaveable: true,
					syncFromServer: false
				);
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
				tag[ "barrier_"+i+"_color" ] = (int)barrier.BarrierColor;
				tag[ "barrier_"+i+"_max_str" ] = (int)barrier.MaxRegenStrength;
				tag[ "barrier_"+i+"_str_regen" ] = (float)barrier.StrengthRegenPerTick;
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
				int strength = reader.ReadInt32();
				int maxRegenStrength = reader.ReadInt32();
				float strengthRegen = reader.ReadSingle();
				int color = reader.ReadInt32();

				mngr.CreateAndDeclareWorldBarrier(
					hostType: BarrierHostType.None,
					hostWhoAmI: -1,
					rect,
					strength,
					maxRegenStrength,
					strengthRegen,
					(BarrierColor)color,
					isSaveable: true,
					false
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


		////////////////

		public override void PostDrawTiles() {
			var plrRect = Main.LocalPlayer.getRect();
			plrRect.X -= 80 * 16;
			plrRect.Y -= 60 * 16;
			plrRect.Width += 160 * 16;
			plrRect.Height += 120 * 16;

			foreach( (Rectangle rect, Barrier barrier) in BarrierManager.Instance.GetWorldBarriers() ) {
				if( !plrRect.Intersects( rect ) ) {
					continue;
				}

				int particles = barrier.GetParticleCount();

				barrier.Animate( particles );
//DebugLibraries.Print( "worldbarrier "+rect, "has:"+barrier.ParticleOffsets.Count+", of:"+particles );
			}
		}
	}
}

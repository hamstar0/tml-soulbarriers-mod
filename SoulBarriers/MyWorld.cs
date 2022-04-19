using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace SoulBarriers {
	class SoulBarriersWorld : ModWorld {
		public override void Load( TagCompound tag ) {
			var mngr = BarrierManager.Instance;

			//

			mngr.RemoveAllWorldBarriers( false );

			//

			if( !tag.ContainsKey("barrier_count3") ) {
				return;
			}
			
			//

			int count = tag.GetInt( "barrier_count3" );

			for( int i=0; i<count; i++ ) {
				string typeName = tag.GetString( $"barrier_{i}_type" );
				string id = tag.GetString( $"barrier_{i}_id" );
				int x = tag.GetInt( $"barrier_{i}_area_x" );
				int y = tag.GetInt( $"barrier_{i}_area_y" );
				int w = tag.GetInt( $"barrier_{i}_area_w" );
				int h = tag.GetInt( $"barrier_{i}_area_h" );
				byte cR = tag.GetByte( $"barrier_{i}_color_r" );
				byte cG = tag.GetByte( $"barrier_{i}_color_g" );
				byte cB = tag.GetByte( $"barrier_{i}_color_b" );
				double maxStr = tag.GetDouble( $"barrier_{i}_max_str" );
				double strRegen = tag.GetDouble( $"barrier_{i}_str_regen" );

				//

				Barrier barrier = BarrierManager.Instance.FactoryCreateBarrier(
					barrierTypeName: typeName,
					id: id,
					hostType: BarrierHostType.None,
					hostWhoAmI: -1,
					data: new Rectangle(x, y, w, h),
					strength: maxStr,
					maxRegenStrength: maxStr,
					strengthRegenPerTick: strRegen,
					color: new Color(cR, cG, cB),
					isSaveable: true	// obvious?
				);

				//
				
				if( barrier is RectangularBarrier ) {
					mngr.DeclareWorldBarrier_Unsynced( barrier as RectangularBarrier );
				}
			}
		}


		public override TagCompound Save() {
			var mngr = BarrierManager.Instance;
			var tag = new TagCompound();

			//

			int i = 0;
			foreach( (Rectangle tileRect, Barrier barrier) in mngr.GetWorldBarriers() ) {
				if( !barrier.CanSave() ) {
					continue;
				}

				//

				tag[ $"barrier_{i}_type" ] = barrier.GetType().FullName;
				tag[ $"barrier_{i}_id" ] = barrier.ID;
				tag[ $"barrier_{i}_area_x" ] = (int)tileRect.X;
				tag[ $"barrier_{i}_area_y" ] = (int)tileRect.Y;
				tag[ $"barrier_{i}_area_w" ] = (int)tileRect.Width;
				tag[ $"barrier_{i}_area_h" ] = (int)tileRect.Height;
				tag[ $"barrier_{i}_color_r" ] = barrier.Color.R;
				tag[ $"barrier_{i}_color_g" ] = barrier.Color.G;
				tag[ $"barrier_{i}_color_b" ] = barrier.Color.B;
				tag[ $"barrier_{i}_max_str" ] = (double)barrier.MaxRegenStrength;
				tag[ $"barrier_{i}_str_regen" ] = (double)barrier.StrengthRegenPerTick;
				i++;
			}

			tag["barrier_count3"] = (int)i;

			//

			return tag;
		}


		////////////////

		public override void NetReceive( BinaryReader reader ) {
			var mngr = BarrierManager.Instance;

			int count = reader.ReadInt32();

			//

			for( int i=0; i<count; i++ ) {
				Barrier barrier = mngr.NetReceiveWorldBarrier( reader );

				Barrier existingBarrier = mngr.GetBarrierByID( barrier.ID );

				//

				if( existingBarrier != null ) {
					existingBarrier.CopyFrom( barrier );
				} else {
					if( barrier is RectangularBarrier ) {
						mngr.DeclareWorldBarrier_Unsynced( barrier as RectangularBarrier );
					} else {
						throw new ModLibsException(
							$"Cannot add {barrier.ToString()} ({barrier.GetType().Name}) to world."
						);
					}
				}
			}
		}


		public override void NetSend( BinaryWriter writer ) {
			var mngr = BarrierManager.Instance;
			int count = mngr.GetWorldBarriers()
				.Where( kv => (kv.Value as IBarrierFactory)?.CanSync() ?? false )
				.Count();

			//

			writer.Write( count );

			//

			foreach( (Rectangle rect, Barrier barrier) in mngr.GetWorldBarriers() ) {
				IBarrierFactory barrierFac = barrier as IBarrierFactory;
				if( barrierFac == null ) {
					continue;
				}

				//

				writer.Write( barrier.GetType().FullName );

				barrierFac.NetSend( writer );
			}
		}
	}
}

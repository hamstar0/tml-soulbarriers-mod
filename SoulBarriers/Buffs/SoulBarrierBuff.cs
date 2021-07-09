using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Buffs {
    public class SoulBarrierBuff : ModBuff {
        public override void SetDefaults() {
            this.DisplayName.SetDefault( "Soul Barrier" );
            this.Description.SetDefault( "Protects against projectiles and spirital attacks" );

            Main.buffNoTimeDisplay[this.Type] = true;
            Main.debuff[this.Type] = false;
		}


		////////////////

		public override void ModifyBuffTip( ref string tip, ref int rare ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<SoulBarriersPlayer>();

			tip += "Current barrier strength: " + myplayer.Barrier.Strength;
		}
	}
}

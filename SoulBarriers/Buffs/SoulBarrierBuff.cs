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

			tip += "\nCurrent barrier strength: " + myplayer.Barrier.Strength;
		}


		////////////////

		public override void Update( Player player, ref int buffIndex ) {
			player.buffTime[buffIndex] = 3;
		}

		public override void Update( NPC npc, ref int buffIndex ) {
			npc.buffTime[buffIndex] = 3;
		}
	}
}

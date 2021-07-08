using Terraria;
using Terraria.ModLoader;


namespace SoulBarriers.Buffs {
	public class PBGOverheatedDeBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "P.B.G Overheated" );
			this.Description.SetDefault( "You must wait a while before the P.B.G can be re-activated" );
			//Main.debuff[this.Type] = true;
			Main.pvpBuff[this.Type] = true;
			Main.buffNoSave[this.Type] = false;
			this.longerExpertDebuff = true;
		}
	}
}

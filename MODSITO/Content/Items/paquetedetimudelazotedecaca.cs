using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MODSITO.Content.Items
{
    // El nombre de la clase debe ser idéntico al del archivo .cs
    public class paquetedetimudelazotedecaca : ModItem
    {
        public override void SetStaticDefaults() {
            // Este es el nombre que verás flotando en el juego
            // DisplayName.SetDefault("Paquete de Timu del Azote de Caca");
        }

        public override void SetDefaults() {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Orange; 
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot) {
            // Suelta tu arma estilo Rogue (Jabalina de Caca)
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<JabalinaDeCaca>()));

            // Relleno de Timu
            itemLoot.Add(ItemDropRule.Common(ItemID.DirtBlock, 1, 50, 100));
            itemLoot.Add(ItemDropRule.Common(ItemID.Wood, 1, 50, 100));

            // El dinero
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 1, 2, 5));
        }
    }
}
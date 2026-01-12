using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MODSITO.Content.Items
{
    public class GordiatBag : ModItem
    {
        public override void SetStaticDefaults() {
        
        }

        public override void SetDefaults() {
            Item.maxStack = 999; // Corregido el error de 'stacking'
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Orange; 
        }

        public override bool CanRightClick() {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot) {
            // Suelta entre 10 y 20 aguijones siempre
            itemLoot.Add(ItemDropRule.Common(ItemID.Stinger, 1, 10, 20));  
          itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MalaKaka>(), 1));
          itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PistolaGrasa>(), 1));
        }
    }
}
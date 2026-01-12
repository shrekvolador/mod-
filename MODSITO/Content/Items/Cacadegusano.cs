using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.Items
{
    public class Cacadegusano : ModItem
    {
        public override void SetStaticDefaults() {
            // Nombre y descripción (opcional, tModLoader lo saca del archivo de lenguaje por defecto)
            // DisplayName.SetDefault("Caca de Gusano"); 
            // Tooltip.SetDefault("Invoca al Azote de Caca");
        }

        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI == Main.myPlayer) {
                // Sonido al usarlo
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position);
                
                // Invoca al jefe
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.AzoteDeCaca>());
            }
            return true;
        }

        // --- AQUÍ ESTÁ EL CRAFTEO ---
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 20) // Nota: Terraria no tiene "Bloque de Caca" base, usé Tierra. 
                                                     // Si tu mod tiene un bloque llamado 'BloqueDeCaca', 
                                                     // cambia ItemID.DirtBlock por ModContent.ItemType<TuBloque>()
                .AddIngredient(ItemID.Wood, 50)
                .AddTile(TileID.WorkBenches) // Se hace en el banco de madera
                .Register();
        }
    }
}
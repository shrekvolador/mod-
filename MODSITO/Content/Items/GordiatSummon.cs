using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MODSITO.Content.Items
{
    public class GordiatSummon : ModItem
    {
        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 5);
            Item.consumable = false; // INFINITO
        }

        public override bool CanUseItem(Player player) {
            // Solo se usa si el jefe no está vivo
            return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Gordiat>());
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI == Main.myPlayer) {
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    // Si juegas solo, aparece normal
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Gordiat>());
                } else {
                    // Si juegas con gente, le avisa al servidor sin usar MessageID (más seguro)
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<NPCs.Gordiat>());
                }

                Main.NewText("La Lata de Grasa Sospechosa ha atraído a Gordiat...", Color.Orange);
            }
            return true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Burger, 3)          
                .AddIngredient(ItemID.BeetleHusk, 2)      
                .AddIngredient(ItemID.DirtBlock, 400)      
                .AddTile(TileID.MythrilAnvil)             
                .Register();
        }
    }
}
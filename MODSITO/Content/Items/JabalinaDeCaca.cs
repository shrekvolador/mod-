using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.Items
{
    public class JabalinaDeCaca : ModItem
    {
        public override void SetDefaults() {
            // Estadísticas estilo Rogue (Scourge of the Desert Parodia)
            Item.damage = 14; 
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Green;
            
            // MECÁNICA INFINITA
            Item.consumable = false; // NO SE GASTA
            Item.maxStack = 1;      // Solo necesitas una
            
            // PROYECTIL
            Item.shoot = ModContent.ProjectileType<Projectiles.JabalinaDeCacaProj>(); 
            Item.shootSpeed = 9f;
            
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            Item.autoReuse = true; 
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 20)
                .AddIngredient(ItemID.Stinger, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

// El namespace debe seguir la ruta de tus carpetas: NombreDelMod.Carpeta.Subcarpeta
namespace MODSITO.Content.Items.Weapons 
{
    public class EspadaTutorial : ModItem
    {
        public override void SetDefaults() {
            Item.damage = 25;            
            Item.DamageType = DamageClass.Melee; 
            Item.width = 40;             
            Item.height = 40;            
            Item.useTime = 15;           
            Item.useAnimation = 15;      
            Item.useStyle = ItemUseStyleID.Swing; 
            Item.knockBack = 4;          
            Item.value = Item.buyPrice(silver: 50); 
            Item.rare = ItemRarityID.Blue; 
            Item.UseSound = SoundID.Item1; 
            Item.autoReuse = true;       
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10) 
                .AddTile(TileID.WorkBenches)    
                .Register();
        }
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.Items
{
    public class MalaKaka : ModItem
    {
        public override void SetDefaults() {
            Item.damage = 95; 
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 12; 
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.MalaKakaProj>(); 
            Item.shootSpeed = 14f;
        }
    }
}
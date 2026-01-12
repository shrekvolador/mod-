using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MODSITO.Content.Items
{
    public class PistolaGrasa : ModItem
    {
        public override void SetDefaults() {
            Item.damage = 65; // Un poco más de daño por ser industrial
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46; // Más grande para que parezca industrial
            Item.height = 24;
            Item.useTime = 7; // ¡Aún más rápida!
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 4, 50, 0);
            Item.rare = ItemRarityID.LightRed; // Rareza más alta
            Item.UseSound = SoundID.Item31; // Sonido de ametralladora           
            Item.shoot = ProjectileID.CultistBossIceMist; 
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet; // Usa balas normales
        }

        // PARODIA: Ahorro de munición (Garantiza que no gastes tantas balas)
        public override bool CanConsumeAmmo(Item ammo, Player player) {
            return Main.rand.NextFloat() >= 0.25f; // 25% de probabilidad de no gastar bala
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            // La grasa hace que la puntería sea un poco inestable
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
        }

        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
    }
}
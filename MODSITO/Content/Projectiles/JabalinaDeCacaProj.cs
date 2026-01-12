using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.Projectiles
{
    public class JabalinaDeCacaProj : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 1; // IA de flecha/jabalina
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            // Parodia de Scourge of the Desert: Suelta pedacitos al chocar
            for (int i = 0; i < 3; i++) {
                Vector2 speed = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-8f, -4f));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, speed, ProjectileID.Nail, damageDone / 2, 0f, Projectile.owner);
            }
            
            // Efecto de partículas marrón
            for (int i = 0; i < 10; i++) {
                Dust.NewDust(target.position, target.width, target.height, DustID.Dirt, 0f, 0f, 0, new Color(100, 50, 0), 1.2f);
            }
        }
    }
}
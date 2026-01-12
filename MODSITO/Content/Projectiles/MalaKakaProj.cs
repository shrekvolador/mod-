using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.Projectiles
{
    public class MalaKakaProj : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1; // Para que se vea veloz
            Projectile.aiStyle = 1; 
        }

        public override void AI() {
            // Rastro de partículas verdes/marrones (Infección)
            if (Main.rand.NextBool(2)) {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenBlood, 0f, 0f, 150, default, 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Ichor, 300); 
            
            // Explosión de pedacitos
            for (int i = 0; i < 5; i++) {
                Dust.NewDust(target.position, target.width, target.height, DustID.Dirt);
            }
        }
    }
}
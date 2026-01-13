using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Effects;

namespace MODSITO.Content.NPCs
{
    public class Stymgeos : ModNPC
    {
        public override void SetStaticDefaults() {
            NPCID.Sets.TrailingMode[NPC.type] = 3; 
            NPCID.Sets.TrailCacheLength[NPC.type] = 15; 
        }

        public override void SetDefaults() {
            NPC.width = 64;
            NPC.height = 64;
            NPC.damage = 45; 
            NPC.lifeMax = 7000;
            NPC.boss = true; 
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.color = Color.Cyan;
        }

        int aiTimer = 0;

        public override void AI() {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            aiTimer++;

            // ACTIVAR CIELO CADA FRAME
            if (!Main.dedServ) {
                if (SkyManager.Instance["MODSITO:StymgeosSky"] != null && !SkyManager.Instance["MODSITO:StymgeosSky"].IsActive()) {
                    SkyManager.Instance.Activate("MODSITO:StymgeosSky");
                }
            }

            // MOVIMIENTO LERP (Suave cada frame)
            Vector2 orbit = new Vector2(0, -400).RotatedBy(MathHelper.ToRadians(aiTimer * 2.5f));
            Vector2 target = player.Center + orbit;
            NPC.velocity = Vector2.Lerp(NPC.velocity, (target - NPC.Center).SafeNormalize(Vector2.Zero) * 15f, 0.07f);

            // Rotación suave mirando al jugador
            NPC.rotation = NPC.rotation.AngleLerp((player.Center - NPC.Center).ToRotation() - MathHelper.PiOver2, 0.12f);

            // ATAQUE
            if (Main.netMode != NetmodeID.MultiplayerClient && aiTimer % 80 == 0) {
                Vector2 shootVel = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * 12f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootVel, ProjectileID.CultistBossIceMist, 35, 1f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item28, NPC.position);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() * 0.5f;
            for (int k = 0; k < NPC.oldPos.Length; k++) {
                Vector2 drawPos = NPC.oldPos[k] - screenPos + origin + new Vector2(0f, NPC.gfxOffY);
                Color color = Color.Cyan * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) * 0.4f;
                spriteBatch.Draw(texture, drawPos, null, color, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
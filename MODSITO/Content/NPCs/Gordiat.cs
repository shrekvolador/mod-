using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules; // Librería necesaria para el drop de la bolsa
using MODSITO.Content.Items;             // Para que reconozca la bolsa GordiatBag

namespace MODSITO.Content.NPCs
{
    [AutoloadBossHead]
    public class Gordiat : ModNPC
    {
        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 1; 
        }

        public override void SetDefaults() {
            NPC.width = 140;   
            NPC.height = 140;
            
            // ESTADÍSTICAS POST-GOLEM
            NPC.damage = 45;      
            NPC.defense = 15;     
            NPC.lifeMax = 65000;  
            
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit4;     
            NPC.DeathSound = SoundID.NPCDeath14; 
            NPC.aiStyle = -1; 
            NPC.value = Item.buyPrice(gold: 50);

            if (!Main.dedServ) {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Gordiat");
            }
        }

        public override void AI() {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (player.dead || !player.active) {
                NPC.velocity.Y -= 0.5f; 
                NPC.EncourageDespawn(10);
                return;
            }

            // --- IA DE PARODIA POST-GOLEM ---
            bool faseFuria = NPC.life < (NPC.lifeMax * 0.4f);
            
            // ESTADO 0: Embestidas de "Lag"
            if (NPC.ai[0] == 0) {
                NPC.ai[1]++;
                if (NPC.ai[1] < 50) {
                    Vector2 haciaJugador = player.Center - NPC.Center;
                    haciaJugador.Normalize();
                    NPC.velocity = haciaJugador * 4f;
                } else if (NPC.ai[1] == 60) {
                    SoundEngine.PlaySound(SoundID.Roar, NPC.position); 
                    Vector2 embestida = player.Center - NPC.Center;
                    embestida.Normalize();
                    NPC.velocity = embestida * (faseFuria ? 24f : 18f);
                } else if (NPC.ai[1] > 100) {
                    NPC.ai[1] = 0; NPC.ai[2]++;
                    if (NPC.ai[2] >= 3) { NPC.ai[0] = 1; NPC.ai[1] = 0; NPC.ai[2] = 0; }
                }
            } 
            // ESTADO 1: Bombardeo de Spam
            else if (NPC.ai[0] == 1) {
                Vector2 posicionDeseada = player.Center + new Vector2(0, -300);
                NPC.velocity = (posicionDeseada - NPC.Center) * 0.1f;
                
                NPC.ai[1]++;
                if (NPC.ai[1] % (faseFuria ? 10 : 20) == 0) {
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-5, 6), 10), ProjectileID.EyeBeam, 40, 0f, Main.myPlayer);
                    }
                }
                if (NPC.ai[1] > 180) { NPC.ai[0] = 0; NPC.ai[1] = 0; }
            }
            
            NPC.rotation = NPC.velocity.X * 0.1f;
        }

        public override void OnKill() {
            Main.NewText("Gordiat ha dejado de funcionar (Error 404).", Color.OrangeRed);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            // Suelta la bolsa de Temu
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GordiatBag>()));
        }
    }
}
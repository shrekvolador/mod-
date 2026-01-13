using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace MODSITO.Content.NPCs
{
    [AutoloadBossHead]
    public class AzoteDeCaca : ModNPC
    {
        public override void SetStaticDefaults() {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.width = 105;
            NPC.height = 123;
            NPC.aiStyle = -1; // IA personalizada pero equilibrada
            NPC.boss = true;
            NPC.lifeMax = 2000 ;
            NPC.damage = 20;
            NPC.defense = 3;
            NPC.noGravity = true;       
            NPC.noTileCollide = true;   
            NPC.behindTiles = true;     
            NPC.netAlways = true;

            if (!Main.dedServ) { 
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CacaMusic");
            }
        }

        public override void AI() {
            // 1. BUSCAR JUGADOR
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];

            // 2. LÓGICA DE ESCAPE (Si el jugador se aleja mucho o muere, el jefe se va)
            float dist = Vector2.Distance(NPC.Center, player.Center);
            if (player.dead || !player.active || dist > 3000f) { // Si te alejas a más de 3000 bloques, se va
                NPC.velocity.Y += 0.5f; 
                NPC.EncourageDespawn(10);
                return; 
            }

            // 3. MOVIMIENTO SUAVE (Permite esquivarlo)
            Vector2 targetPos = player.Center;
            float speed = 6f;           // Velocidad moderada
            float turnResistance = 35f;  // Alta resistencia al giro = Giros más grandes y fáciles de esquivar

            Vector2 move = targetPos - NPC.Center;
            move.Normalize();
            move *= speed;

            // Esta línea hace que el movimiento sea fluido y no "telepático"
            NPC.velocity = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

            // 4. CREAR CUERPO
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f) {
                int anterior = NPC.whoAmI;
                int totalSegmentos = 20; // Longitud normal
                for (int i = 0; i < totalSegmentos; i++) {
                    int tipo = (i == totalSegmentos - 1) ? ModContent.NPCType<AzoteDeCacaCola>() : ModContent.NPCType<AzoteDeCacaCuerpo>();
                    int actual = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, tipo, NPC.whoAmI);
                    Main.npc[actual].realLife = NPC.whoAmI; 
                    Main.npc[actual].ai[1] = (float)anterior; 
                    Main.npc[anterior].ai[0] = (float)actual; 
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, actual);
                    anterior = actual;
                }
                NPC.localAI[0] = 1f; 
                Main.NewText("¡El Azote de Caca quiere entregarte algo!", 175, 75, 255);
            }
        }

        // NO DAR PREMIO SI EL JUGADOR MUERE
        public override bool CheckDead() {
            Player player = Main.player[NPC.target];
            if (player.dead || !player.active) {
                NPC.active = false;
                return false; 
            }
            return true;
        }

        public override void OnKill() {
            Player player = Main.player[NPC.target];
            if (player.active && !player.dead) {
                if (Main.netMode != NetmodeID.Server) {
                    Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.paquetedetimudelazotedecaca>());
                }
                Main.NewText("¡Paquete entregado con éxito!", 255, 165, 0);
            }
        }
    }
}
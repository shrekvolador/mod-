using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.NPCs
{
    public class AzoteDeCacaCola : ModNPC
    {
        public override void SetStaticDefaults() {
            // Ocultar la cola del Bestiario para que no se vea desordenado
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 10;
            NPC.aiStyle = 6;
            NPC.dontCountMe = true;
            NPC.netAlways = true;

            // --- AJUSTES PARA VOLAR LIBREMENTE ---
            NPC.noGravity = true;       // Evita que la cola se caiga al vacío
            NPC.noTileCollide = true;   // Permite que la cola ignore las paredes
            NPC.behindTiles = true;     // Se asegura de que se vea bien al cruzar el suelo
        }

        public override bool PreAI() {
            // Engañamos a la IA para que no busque bloques y siga volando
            NPC.localAI[1] = 1f;

            // Si el segmento que tiene delante (cuerpo) desaparece, la cola muere
            if (NPC.ai[1] < 0 || !Main.npc[(int)NPC.ai[1]].active) {
                NPC.life = 0;
                NPC.active = false;
                NPC.netUpdate = true;
            }
            return true;
        }
    }
}
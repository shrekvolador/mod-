using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MODSITO.Content.NPCs
{
    public class AzoteDeCacaCuerpo : ModNPC
    {
        public override void SetStaticDefaults() {
            // Ocultar del bestiario
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        
        public override void SetDefaults() {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 15;
            NPC.aiStyle = 6;
            NPC.dontCountMe = true;
            NPC.netAlways = true;

            // Ajustes para que vuele y no necesite bloques
            NPC.noGravity = true;       
            NPC.noTileCollide = true;   
            NPC.behindTiles = true;     
        }

        public override bool PreAI() {
            // Truco para que vuele libremente
            NPC.localAI[1] = 1f;

            // Si la parte de adelante muere, esta también
            if (NPC.ai[1] < 0 || !Main.npc[(int)NPC.ai[1]].active) {
                NPC.life = 0;
                NPC.active = false;
                NPC.netUpdate = true;
            }
            return true;
        }
    } // Aquí se cierra la clase
} // Aquí se cierra el namespace
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using MODSITO.Content.NPCs;

namespace MODSITO.Content.Skies
{
    public class StymgeosScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

        // Se activa SIEMPRE que el NPC "Stymgeos" esté en pantalla o vivo
        public override bool IsSceneEffectActive(Player player) => NPC.AnyNPCs(ModContent.NPCType<Stymgeos>());

        public override void SpecialVisuals(Player player, bool isActive) {
            if (isActive) {
                if (!SkyManager.Instance["MODSITO:StymgeosSky"].IsActive())
                    SkyManager.Instance.Activate("MODSITO:StymgeosSky");
            } else {
                if (SkyManager.Instance["MODSITO:StymgeosSky"].IsActive())
                    SkyManager.Instance.Deactivate("MODSITO:StymgeosSky");
            }
        }
    }
}
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using MODSITO.Content.Skies;
using System;

namespace MODSITO
{
    public class MODSITO : Mod
    {
        public override void Load() {
            if (!Main.dedServ) {
                // 1. Registrar el cielo base
                SkyManager.Instance["MODSITO:StymgeosSky"] = new StymgeosSky();

                try {
                    // 2. Cargar el Asset del efecto (Debe estar en Assets/Effects/CieloEfecto.fx)
                    Asset<Effect> shaderAsset = ModContent.Request<Effect>("MODSITO/Assets/Effects/CieloEfecto", AssetRequestMode.ImmediateLoad);
                    
                    // 3. Registrar el filtro usando el nombre de la técnica de tu archivo .fx
                    // Nota: Cambié "Technique1" por "FiltroCosmicoPass" para que coincida con el shader típico
                    Filters.Scene["MODSITO:FiltroCosmico"] = new Filter(
                        new ScreenShaderData(shaderAsset, "FiltroCosmicoPass"), 
                        EffectPriority.VeryHigh
                    );
                    Filters.Scene["MODSITO:FiltroCosmico"].Load();

                    // 4. Registrar el cielo cósmico
                    SkyManager.Instance["MODSITO:CosmicSky"] = new CosmicSky();

                } catch (Exception e) {
                    Logger.Error("Error al cargar el shader: " + e.Message);
                }
            }
        }
    }
}
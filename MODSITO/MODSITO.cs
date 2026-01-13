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
                // Registrar los cielos (Asegúrate que estas clases existan en tu carpeta Skies)
                SkyManager.Instance["MODSITO:StymgeosSky"] = new StymgeosSky();
                SkyManager.Instance["MODSITO:CosmicSky"] = new CosmicSky();

                try {
                    // CARGAMOS EL ARCHIVO "Cielo.fx"
                    // Nota: No se pone la extensión .fx en el código
                    Asset<Effect> shaderAsset = ModContent.Request<Effect>("MODSITO/Assets/Effects/Cielo", AssetRequestMode.ImmediateLoad);
                    
                    // REGISTRAMOS EL FILTRO
                    Filters.Scene["MODSITO:FiltroCosmico"] = new Filter(
                        new ScreenShaderData(shaderAsset, "FiltroCosmicoPass"), 
                        EffectPriority.VeryHigh
                    );
                    
                    Filters.Scene["MODSITO:FiltroCosmico"].Load();
                    
                    Logger.Info("MODSITO: Shader 'Cielo.fx' cargado con éxito.");

                } catch (Exception e) {
                    Logger.Error("MODSITO: Error crítico al cargar el shader: " + e.Message);
                }
            }
        }

        public override void Unload() { }
    }
}
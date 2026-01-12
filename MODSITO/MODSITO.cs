using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using MODSITO.Content.Skies;
using System;

namespace MODSITO
{
    public class MODSITO : Mod
    {
        public override void Load() {
            // Solo ejecutar código visual si no es un servidor dedicado
            if (!Main.dedServ) {
                // 1. Registrar el cielo base
                SkyManager.Instance["MODSITO:StymgeosSky"] = new StymgeosSky();

                try {
                    // 2. Leer los bytes del archivo .fx
                    // Asegúrate de que el archivo en la carpeta se llame exactamente CieloEfecto.fx
                    byte[] shaderData = GetFileBytes("Assets/Effects/CieloEfecto.fx");
                    
                    // 3. Crear el efecto
                    Effect shader = new Effect(Main.instance.GraphicsDevice, shaderData);
                    
                    // 4. Registrar el filtro (FiltroCosmico)
                    Filters.Scene["MODSITO:FiltroCosmico"] = new Filter(
                        new ScreenShaderData(new Ref<Effect>(shader), "Technique1"), 
                        EffectPriority.VeryHigh
                    );
                    
                    // 5. Registrar el cielo cósmico
                    SkyManager.Instance["MODSITO:CosmicSky"] = new CosmicSky();

                } catch (Exception e) {
                    // Si algo falla, se verá en el log sin cerrar el juego
                    this.Logger.Error("Error al cargar el shader CieloEfecto: " + e.Message);
                }
            }
        }
    }
}
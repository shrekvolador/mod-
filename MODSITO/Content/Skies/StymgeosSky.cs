using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using System;

namespace MODSITO.Content.Skies
{
    public class StymgeosSky : CustomSky
    {
        private bool isActive = false;
        private float intensity;

        public override void Update(GameTime gameTime) 
        {
            // Verificamos si el otro cielo existe para evitar errores de referencia nula
            // Guardamos la referencia en una variable para no buscarla dos veces
            var cosmicSky = SkyManager.Instance["MODSITO:CosmicSky"];

            if (isActive)
            {
                intensity = MathHelper.Min(1f, intensity + 0.01f);
                
                // ACTIVADOR: Enciende el otro cielo solo si existe y no está activo
                if (cosmicSky != null && !cosmicSky.IsActive())
                {
                    SkyManager.Instance.Activate("MODSITO:CosmicSky");
                }
            }
            else
            {
                intensity = MathHelper.Max(0f, intensity - 0.01f);
                
                // DESACTIVADOR: Apaga el otro cuando este termina de desvanecerse
                if (intensity <= 0f && cosmicSky != null && cosmicSky.IsActive())
                {
                    SkyManager.Instance.Deactivate("MODSITO:CosmicSky");
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) 
        {
            // Solo dibujamos en la capa de fondo (profundidad infinita)
            if (intensity <= 0f || !(maxDepth >= 0f && minDepth < 0f)) 
            {
                return;
            }

            // Verificación segura del filtro
            if (!Filters.Scene["MODSITO:FiltroCosmico"].IsActive())
            {
                // Opcional: Podrías activar el filtro aquí si es necesario
            }

            var shaderData = Filters.Scene["MODSITO:FiltroCosmico"]?.GetShader();
            if (shaderData == null) 
            {
                return;
            }

            Effect shader = shaderData.Shader;
            float time = Main.GlobalTimeWrappedHourly;

            // Matriz de proyección para que el shader cubra la pantalla correctamente
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            
            // Pasar parámetros al shader (.fx)
            shader.Parameters["globalTime"]?.SetValue(time);
            shader.Parameters["uIntensity"]?.SetValue(intensity);
            shader.Parameters["uScreenResolution"]?.SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            shader.Parameters["uWorldViewProjection"]?.SetValue(projection);
            shader.Parameters["uOffset"]?.SetValue(Main.screenPosition * 0.0003f);

            // Terminamos el SpriteBatch de Terraria para iniciar el nuestro con el Shader
            spriteBatch.End();

            // Dibujo Procedural: Usamos MagicPixel (una textura blanca de 1x1) estirada a toda la pantalla
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, shader, Matrix.Identity);
            
            Texture2D blankTexture = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * intensity);

            spriteBatch.End();
            
            // IMPORTANTE: Regresar al estado original de SpriteBatch para no romper el dibujo del resto del juego
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void Activate(Vector2 position, params object[] args) 
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args) 
        {
            isActive = false;
        }

        public override void Reset() 
        {
            isActive = false;
            intensity = 0f;
        }

        public override bool IsActive() 
        {
            return isActive || intensity > 0f;
        }
    }
}
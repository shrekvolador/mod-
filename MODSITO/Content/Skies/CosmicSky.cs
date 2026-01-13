using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using System;

namespace MODSITO.Content.Skies
{
    public class CosmicSky : CustomSky
    {
        private bool isActive = false;
        private float intensity;

        public override void Update(GameTime gameTime) 
        {
            if (isActive) {
                intensity = MathHelper.Min(1f, intensity + 0.01f);
            }
            else {
                intensity = MathHelper.Max(0f, intensity - 0.01f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) 
        {
            // Verificación de profundidad y visibilidad
            if (intensity <= 0f || !(maxDepth >= 0f && minDepth < 0f)) {
                return;
            }

            // SEGURIDAD: Intentamos obtener el shader del filtro
            var shaderData = Filters.Scene["MODSITO:FiltroCosmico"]?.GetShader();
            if (shaderData == null) {
                return;
            }

            Effect shader = shaderData.Shader;
            float time = Main.GlobalTimeWrappedHourly;

            // Configuración de la matriz y parámetros del shader
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            
            shader.Parameters["globalTime"]?.SetValue(time);
            shader.Parameters["uIntensity"]?.SetValue(intensity);
            shader.Parameters["uScreenResolution"]?.SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            shader.Parameters["uWorldViewProjection"]?.SetValue(projection);
            shader.Parameters["uOffset"]?.SetValue(Main.screenPosition * 0.0005f); // Un poco más rápido que el otro

            spriteBatch.End();
            
            // Dibujamos usando el shader
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, shader, Matrix.Identity);
            
            Texture2D blankTexture = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.DeepSkyBlue * intensity);

            spriteBatch.End();
            
            // Restaurar el estado de Terraria
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
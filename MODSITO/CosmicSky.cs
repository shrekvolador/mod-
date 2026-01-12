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

        public override void Update(GameTime gameTime) {
            intensity = isActive ? MathHelper.Min(1f, intensity + 0.01f) : MathHelper.Max(0f, intensity - 0.01f);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
            if (intensity <= 0f || !(maxDepth >= 0f && minDepth < 0f)) return;

            var shaderData = Filters.Scene["MODSITO:FiltroCosmico"]?.GetShader();
            if (shaderData == null) return;

            Effect shader = shaderData.Shader;
            float time = Main.GlobalTimeWrappedHourly;

            // Matriz de proyección ortográfica para cubrir toda la ventana
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            
            shader.Parameters["globalTime"]?.SetValue(time);
            shader.Parameters["uIntensity"]?.SetValue(intensity);
            shader.Parameters["uScreenResolution"]?.SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            shader.Parameters["uWorldViewProjection"]?.SetValue(projection);
            
            // MOVIMIENTO PARALLAX INFINITO:
            // Dividimos la posición de la pantalla por un valor para que el cielo sea "enorme".
            Vector2 parallaxOffset = Main.screenPosition * 0.0003f; 
            shader.Parameters["uOffset"]?.SetValue(parallaxOffset);

            spriteBatch.End();

            // Dibujamos usando MagicPixel (una textura de 1x1 blanca interna de Terraria)
            // El shader ignorará el color blanco y generará su propio ruido matemático.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, shader, Matrix.Identity);
            
            Texture2D blankTexture = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            spriteBatch.End();
            
            // Restauramos el estado para que Terraria no se rompa
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void Activate(Vector2 position, params object[] args) => isActive = true;
        public override void Deactivate(params object[] args) => isActive = false;
        public override void Reset() => isActive = false;
        public override bool IsActive() => isActive || intensity > 0f;
    }
}
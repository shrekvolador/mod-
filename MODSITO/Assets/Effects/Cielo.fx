sampler uImage0 : register(s0);
float globalTime;
float uIntensity;
float2 uOffset;
float2 uScreenResolution;
matrix uWorldViewProjection;

// Generador de números aleatorios para el ruido
float hash(float2 p) {
    p = frac(p * float2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return frac(p.x * p.y);
}

// Ruido suave (Bilineal)
float noise(float2 p) {
    float2 i = floor(p);
    float2 f = frac(p);
    float a = hash(i);
    float b = hash(i + float2(1.0, 0.0));
    float c = hash(i + float2(0.0, 1.0));
    float d = hash(i + float2(1.0, 1.0));
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

// FBM (Fractal Brownian Motion)
float fbm(float2 p) {
    float v = 0.0;
    float a = 0.5;
    float2 shift = float2(100.0, 100.0);
    for (int i = 0; i < 6; i++) {
        v += a * noise(p);
        p = p * 2.0 + shift;
        a *= 0.5;
    }
    return v;
}

// ESTRUCTURAS AJUSTADAS PARA XNA/TMODLOADER
struct VertexShaderInput {
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0; // Cambiado de float3 a float2 para compatibilidad
};

struct VertexShaderOutput {
    float4 Position : SV_POSITION;
    float2 uv : TEXCOORD0;
};

// VERTEX SHADER - Esencial para procesar la posición en pantalla
VertexShaderOutput VertexShaderFunction(in VertexShaderInput input) {
    VertexShaderOutput output;
    output.Position = mul(input.Position, uWorldViewProjection);
    output.uv = input.TexCoord;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0 {
    float aspect = uScreenResolution.x / uScreenResolution.y;
    float2 uv = input.uv;
    
    // Corregimos aspect ratio para que no se estire el shader
    float2 p = uv;
    p.x *= aspect;
    p = p * 2.5 + uOffset; 
    
    // Domain Warping (Gas/Nebulosa)
    float2 q = float2(fbm(p + globalTime * 0.1), fbm(p + float2(5.2, 1.3)));
    float2 r = float2(fbm(p + 4.0 * q + float2(1.7, 9.2) + globalTime * 0.15), fbm(p + 4.0 * q + float2(8.3, 2.8)));
    
    float f = fbm(p + 4.0 * r);

    // Colores
    float4 colorBase = float4(0.02, 0.0, 0.1, 1.0);
    float4 colorNebula = float4(0.1, 0.3, 0.6, 1.0);
    float4 colorBrillo = float4(0.5, 0.0, 0.4, 1.0);

    float4 finalColor = lerp(colorBase, colorNebula, clamp(f * f * 4.0, 0.0, 1.0));
    finalColor = lerp(finalColor, colorBrillo, clamp(length(q), 0.0, 1.0));
    
    // Estrellas
    float starNoise = pow(hash(uv + floor(uOffset * 20.0) / 20.0), 300.0);
    float3 stars = starNoise * 40.0;

    return (finalColor + float4(stars, 0.0)) * uIntensity;
}

technique Technique1
{
    pass FiltroCosmicoPass
    {
        // IMPORTANTE: Incluir el VertexShader para que el filtro se posicione bien
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}